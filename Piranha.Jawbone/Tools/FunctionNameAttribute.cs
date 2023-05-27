using System;

namespace Piranha.Jawbone;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class FunctionNameAttribute : Attribute
{
    public string FunctionName { get; }

    public FunctionNameAttribute(string functionName)
    {
        FunctionName = functionName;
    }
}
