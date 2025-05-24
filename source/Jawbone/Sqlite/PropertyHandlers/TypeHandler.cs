using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Jawbone.Sqlite;

interface ITypeHandler<T>
{
    string DataType { get; }
    void BindProperty(SqliteStatement statement, int index, T value);
    T LoadProperty(SqliteReader reader, int index);
}

static class TypeHandler
{
    public const string Integer = "INTEGER";
    public const string Real = "REAL";
    public const string Text = "TEXT";
    public const string Blob = "BLOB";

    public static bool IsNullableValueType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    private static readonly ImmutableDictionary<Type, object> theTypeHandlers =
        new Dictionary<Type, object>
        {
            [typeof(sbyte)] = new Int8Handler(),
            [typeof(short)] = new Int16Handler(),
            [typeof(int)] = new Int32Handler(),
            [typeof(long)] = new Int64Handler(),
            [typeof(byte)] = new UInt8Handler(),
            [typeof(ushort)] = new UInt16Handler(),
            [typeof(uint)] = new UInt32Handler(),
            [typeof(ulong)] = new UInt64Handler(),
            [typeof(float)] = new Float32Handler(),
            [typeof(double)] = new Float64Handler(),
            [typeof(string)] = new StringHandler(),
            [typeof(byte[])] = new ByteArrayHandler(),
            [typeof(ImmutableArray<byte>)] = new ImmutableByteArrayHandler(),
            [typeof(Guid)] = new GuidHandler(),
            [typeof(DateTime)] = new DateTimeHandler(),
            [typeof(DateTimeOffset)] = new DateTimeOffsetHandler()
        }.ToImmutableDictionary();

    public static object Get(Type type)
    {
        if (type.IsNullableValueType())
        {
            var innerType = type.GetGenericArguments()[0];
            var innerHandler = theTypeHandlers[innerType];
            var nullableHandlerType = typeof(NullableHandler<>).MakeGenericType(innerType);
            var result = Activator.CreateInstance(nullableHandlerType, innerHandler);

            if (result is null)
                throw new NullReferenceException("Unable to create nullable handler.");

            return result;
        }
        else
        {
            return theTypeHandlers[type];
        }
    }
}
