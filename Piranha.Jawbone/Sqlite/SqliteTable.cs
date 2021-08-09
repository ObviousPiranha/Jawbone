using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using Piranha.Jawbone.Tools;

namespace Piranha.Jawbone.Sqlite
{
    public sealed class SqliteTable<T> where T : class
    {
        public string TableName { get; }
        public int FieldCount => _properties.Length;

        private readonly ImmutableArray<SqliteProperty<T>> _properties;
        private readonly ImmutableArray<SqliteProperty<T>> _keyProperties;

        public SqliteTable() : this(typeof(T).Name, s => s)
        {
        }

        public SqliteTable(string tableName) : this(tableName, s => s)
        {
        }
        
        public SqliteTable(Func<string, string> namingPolicy) : this(typeof(T).Name, namingPolicy)
        {
        }

        public SqliteTable(string tableName, Func<string, string> namingPolicy)
        {
            TableName = tableName;

            var keyArrayBuilder = ImmutableArray.CreateBuilder<SqliteProperty<T>>();
            var propArrayBuilder = ImmutableArray.CreateBuilder<SqliteProperty<T>>();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var propertyHandlerType = typeof(PropertyHandler<,>).MakeGenericType(typeof(T), propertyInfo.PropertyType);
                var typeHandlerType = typeof(ITypeHandler<>).MakeGenericType(propertyInfo.PropertyType);
                var typeHandler = TypeHandler.Get(propertyInfo.PropertyType);
                var constructor = propertyHandlerType.GetConstructor(new Type[] {typeof(PropertyInfo), typeHandlerType});

                if (constructor is null)
                    throw new NullReferenceException("Failed to locate appropriate constructor");
                
                var propertyHandler = (IPropertyHandler<T>)constructor.Invoke(new[] {propertyInfo, typeHandler});

                if (propertyInfo.GetCustomAttribute<SqliteIgnore>() is null)
                {
                    var property = new SqliteProperty<T>(propertyInfo, propertyHandler, namingPolicy);
                    propArrayBuilder.Add(property);

                    if (property.Key != null)
                        keyArrayBuilder.Add(property);
                }
            }

            _properties = propArrayBuilder.ToImmutable();

            keyArrayBuilder.Sort(SqliteProperty<T>.CompareKeys);
            _keyProperties = keyArrayBuilder.ToImmutable();
        }

        private SqliteTable(
            string tableName,
            ImmutableArray<SqliteProperty<T>> properties,
            ImmutableArray<SqliteProperty<T>> keyProperties)
        {
            TableName = tableName;
            _properties = properties;
            _keyProperties = keyProperties;
        }

        public bool Exists(SqliteDatabase database)
        {
            string sql = "SELECT 1 FROM sqlite_master WHERE type='table' AND name=?";

            using (var statement = database.Prepare(sql))
            {
                statement.BindText(1, TableName);

                using (var reader = statement.GetReader())
                    return reader.TryRead();
            }
        }

        public SqliteTable<T> WithoutMissingColumns(SqliteDatabase database)
        {
            var columns = new List<string>();

            using (var reader = database.Read($"PRAGMA table_info({TableName})"))
            {
                while (reader.TryRead())
                {
                    var column = reader.ColumnText(1);

                    if (column != null)
                        columns.Add(column);
                }
            }

            if (columns.Count < 1)
                throw new MissingTableException($"Table '{TableName}' does not exist.");
            
            var properties = ImmutableArray.CreateRange(_properties.Where(p => columns.Contains(p.ColumnName)));
            var keyProperties = ImmutableArray.CreateRange(_keyProperties.Where(p => columns.Contains(p.ColumnName)));

            if (properties.IsDefaultOrEmpty)
                throw new MissingTableException($"Table '{TableName}' has the wrong schema.");

            return new SqliteTable<T>(TableName, properties, keyProperties);
        }

        public void AddMissingColumns(SqliteDatabase database)
        {
            CreateTable(database);

            var currentFields = new HashSet<string>();

            using (var reader = database.Read($"PRAGMA table_info({TableName})"))
            {
                while (reader.TryRead())
                {
                    var column = reader.ColumnText(1);

                    if (column != null)
                        currentFields.Add(column);
                }
            }

            var missingFields = new List<SqliteProperty<T>>(_properties);
            missingFields.RemoveAll(p => currentFields.Contains(p.ColumnName));

            var builder = new StringBuilder();
            foreach (var field in missingFields)
            {
                builder.Length = 0;
                builder
                    .Append("ALTER TABLE ")
                    .AppendField(TableName)
                    .Append(" ADD COLUMN ")
                    .AppendProperty(field);
                
                database.Execute(builder.ToString());
            }
        }

        public string InsertSql(int rowCount, ConflictResolution conflictResolution = ConflictResolution.Default)
        {
            var builder = new StringBuilder("INSERT");

            switch (conflictResolution)
            {
                case ConflictResolution.Replace: builder.Append(" OR REPLACE"); break;
                case ConflictResolution.Rollback: builder.Append(" OR ROLLBACK"); break;
                case ConflictResolution.Abort: builder.Append(" OR ABORT"); break;
                case ConflictResolution.Fail: builder.Append(" OR FAIL"); break;
                case ConflictResolution.Ignore: builder.Append(" OR IGNORE"); break;
                case ConflictResolution.Default:
                default: break;
            }

            builder
                .Append(" INTO ")
                .AppendField(TableName)
                .Append(" (")
                .AppendField(_properties[0].ColumnName);
            
            for (int i = 1; i < _properties.Length; ++i)
                builder.Append(", ").AppendField(_properties[i].ColumnName);
            
            builder.Append(") VALUES (?");

            for (int i = 1; i < _properties.Length; ++i)
                builder.Append(",?");
            
            for (int i = 1; i < rowCount; ++i)
            {
                builder.Append("), (?");

                for (int j = 1; j < _properties.Length; ++j)
                    builder.Append(",?");
            }

            builder.Append(")");
            return builder.ToString();
        }

        public void Bind(SqliteStatement statement, int firstIndex, T record)
        {
            int n = firstIndex;

            foreach (var property in _properties)
                property.PropertyHandler.BindRecord(statement, n++, record);
        }

        public void Insert(
            SqliteDatabase database,
            T record,
            ConflictResolution conflictResolution = ConflictResolution.Default)
        {
            var sql = InsertSql(1, conflictResolution);
            using (var statement = database.Prepare(sql))
            {
                for (int i = 0; i < _properties.Length; ++i)
                    _properties[i].PropertyHandler.BindRecord(statement, i + 1, record);
                
                statement.Execute();
            }
        }

        public void Insert(
            SqliteDatabase database,
            IEnumerable<T> records,
            ConflictResolution conflictResolution = ConflictResolution.Default)
        {
            int count = 0;
            var recordBuffer = new T[database.VariableLimit / _properties.Length];
            var sql = InsertSql(recordBuffer.Length, conflictResolution);
            var statement = database.Prepare(sql);

            try
            {
                foreach (var record in records)
                {
                    recordBuffer[count++] = record;

                    if (count == recordBuffer.Length)
                    {
                        InsertAll();
                        count = 0;
                    }
                }

                if (count > 0)
                {
                    statement.Dispose();
                    sql = InsertSql(count, conflictResolution);
                    statement = database.Prepare(sql);
                    InsertAll();
                }
            }
            finally
            {
                statement?.Dispose();
            }


            void InsertAll()
            {
                int n = 0;

                for (int i = 0; i < count; ++i)
                {
                    foreach (var property in _properties)
                        property.PropertyHandler.BindRecord(statement, ++n, recordBuffer[i]);
                }

                statement.Execute();
            }
        }

        public void InsertWithTransaction(
            SqliteDatabase database,
            IEnumerable<T> records,
            ConflictResolution conflictResolution = ConflictResolution.Default)
        {
            using (var transaction = new SqliteTransaction(database))
            {
                var sql = InsertSql(1, conflictResolution);
                using (var statement = database.Prepare(sql))
                {
                    foreach (var record in records)
                    {
                        for (int i = 0; i < _properties.Length; ++i)
                            _properties[i].PropertyHandler.BindRecord(statement, i + 1, record);
                        
                        statement.Execute();
                    }
                }

                transaction.Commit();
            }
        }

        public IEnumerable<KeyValuePair<long, T2>> ValuesWithRowId<T2>(
            SqliteDatabase database,
            Func<T2> generator,
            Action<QueryBuilder>? extendQuery = null) where T2 : T
        {
            var builder = new QueryBuilder($"`{TableName}`")
                .Select(p => $"`{p.ColumnName}`", _properties)
                .Select("`rowid`");
            
            extendQuery?.Invoke(builder);
            
            using (var reader = database.Read(builder.ToString()))
            {
                while (reader.TryRead())
                {
                    var item = generator();
                    int index = 0;
                    foreach (var property in _properties)
                        property.PropertyHandler.LoadRecord(reader, index++, item);
                    
                    long rowId = reader.ColumnInt64(index++);
                    yield return KeyValuePair.Create(rowId, item);
                }
            }
        }

        public IEnumerable<T2> Values<T2>(
            SqliteDatabase database,
            Func<T2> generator,
            Action<QueryBuilder>? extendQuery = null) where T2 : T
        {
            var builder = new QueryBuilder($"`{TableName}`")
                .Select(p => $"`{p.ColumnName}`", _properties);
            
            extendQuery?.Invoke(builder);

            using (var reader = database.Read(builder.ToString()))
            {
                while (reader.TryRead())
                {
                    var item = generator();
                    int index = 0;
                    foreach (var property in _properties)
                        property.PropertyHandler.LoadRecord(reader, index++, item);
                    yield return item;
                }
            }
        }

        public void CreateTable(SqliteDatabase database)
        {
            var sql = CreateTableSql();
            database.Execute(sql);
        }

        public string CreateTableSql()
        {
            var builder = new StringBuilder("CREATE TABLE IF NOT EXISTS ")
                .AppendField(TableName)
                .Append(" (")
                .AppendProperty(_properties[0]);
            
            for (int i = 1; i < _properties.Length; ++i)
                builder.Append(", ").AppendProperty(_properties[i]);
            
            if (!_keyProperties.IsDefaultOrEmpty)
            {
                builder
                    .Append(", PRIMARY KEY (")
                    .AppendKey(_keyProperties[0]);
                
                for (int i = 1; i < _keyProperties.Length; ++i)
                    builder.Append(", ").AppendKey(_keyProperties[i]);
                
                builder.Append(")");
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
