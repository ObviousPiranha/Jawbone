using System;

namespace Piranha.Jawbone.Tools
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FunctionNameAttribute : Attribute
    {
        public string FunctionName { get; }

        public FunctionNameAttribute(string functionName)
        {
            FunctionName = functionName;
        }
    }
}
