using System;
using System.Reflection;

namespace Piranha.Jawbone.Sqlite;

interface IPropertyHandler<T>
{
    string DataType { get; }
    void BindRecord(SqliteStatement statement, int index, T record);
    void LoadRecord(SqliteReader reader, int index, T record);
}

class PropertyHandler<TRecord, TProperty> : IPropertyHandler<TRecord>
{
    private readonly Action<TRecord, TProperty> _setter;
    private readonly Func<TRecord, TProperty> _getter;
    private readonly ITypeHandler<TProperty> _converter;

    public string DataType => _converter.DataType;

    public PropertyHandler(
        PropertyInfo property,
        ITypeHandler<TProperty> converter)
    {
        var getMethod = property.GetMethod ?? throw new ArgumentException("Property must have getter.", nameof(property));
        var setMethod = property.SetMethod ?? throw new ArgumentException("Property must have setter.", nameof(property));
        
        _setter = setMethod.CreateDelegate<Action<TRecord, TProperty>>();
        _getter = getMethod.CreateDelegate<Func<TRecord, TProperty>>();
        _converter = converter;
    }
    
    public void BindRecord(SqliteStatement statement, int index, TRecord record)
    {
        var value = _getter.Invoke(record);
        _converter.BindProperty(statement, index, value);
    }

    public void LoadRecord(SqliteReader reader, int index, TRecord record)
    {
        var value = _converter.LoadProperty(reader, index);
        _setter.Invoke(record, value);
    }
}
