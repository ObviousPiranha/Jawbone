using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Tools.CollectionExtensions;
using Piranha.Jawbone.Tools.ReflectionExtensions;

namespace Piranha.Jawbone.Tools
{
    public static class NativeLibraryInterface
    {
        private const MethodAttributes MyMethodAttributes =
            MethodAttributes.Public |
            MethodAttributes.Final |
            MethodAttributes.HideBySig |
            MethodAttributes.NewSlot |
            MethodAttributes.Virtual;
        
        public static string? GetCString(IntPtr ptr)
        {
            return ptr.IsInvalid() ? null : Marshal.PtrToStringUTF8(ptr);
        }

        public static NativeLibraryInterface<T> FromFile<T>(
            string libraryPath,
            Func<string, string> methodNameToFunctionName,
            Action<T>? afterLoad = null,
            Action<T>? beforeDispose = null)
            where T : class
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !libraryPath.Contains('/'))
                libraryPath = "./" + libraryPath;
            
            var libraryName = "Native." + Path.GetFileNameWithoutExtension(libraryPath);
            var libraryHandle = NativeLibrary.Load(libraryPath);
            
            if (libraryHandle.IsInvalid())
                throw new Exception("Unable to load library " + libraryPath);
            
            try
            {
                return Create<T>(
                    libraryName,
                    libraryHandle,
                    methodNameToFunctionName,
                    NativeLibrary.GetExport,
                    afterLoad,
                    beforeDispose);
            }
            catch
            {
                NativeLibrary.Free(libraryHandle);
                throw;
            }
        }
        
        public static NativeLibraryInterface<T> Create<T>(
            string libraryName,
            IntPtr libraryHandle,
            Func<string, string> methodNameToFunctionName,
            Func<IntPtr, string, IntPtr> procAddressLoader,
            Action<T>? afterLoad = null,
            Action<T>? beforeDispose = null)
            where T : class
        {
            if (!typeof(T).IsInterface)
                throw new Exception($"Type T must be an interface. Type {typeof(T)} is not an interface.");

            var assemblyName = new AssemblyName
            {
                Name = libraryName
            };

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType(
                "NativeLibrary",
                TypeAttributes.Class | TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(typeof(T));
            
            var constructorBuilder = typeBuilder.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.HideBySig |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);
            var getCStringMethod = typeof(NativeLibraryInterface).GetMethod(nameof(GetCString)) ?? throw new NullReferenceException();
            var interfaceMethods = typeof(T).GetMethods();

            foreach (var interfaceMethod in interfaceMethods)
            {
                var parameters = interfaceMethod.GetParameters();

                var returnParameter = interfaceMethod.ReturnParameter;
                var parameterTypes = Array.ConvertAll(parameters, p => p.ParameterType);
                var methodBuilder = typeBuilder.DefineMethod(
                    interfaceMethod.Name,
                    MyMethodAttributes,
                    CallingConventions.HasThis,
                    interfaceMethod.ReturnType,
                    returnParameter.GetRequiredCustomModifiers(),
                    returnParameter.GetOptionalCustomModifiers(),
                    parameterTypes,
                    Array.ConvertAll(parameters, p => p.GetRequiredCustomModifiers()),
                    Array.ConvertAll(parameters, p => p.GetOptionalCustomModifiers()));
                
                typeBuilder.DefineMethodOverride(methodBuilder, interfaceMethod);

                var functionName = methodNameToFunctionName(interfaceMethod.Name);
                var procAddress = procAddressLoader(libraryHandle, functionName);
                
                if (procAddress.IsInvalid())
                    throw new Exception("Unable to load function " + functionName);
                
                var returnsString = interfaceMethod.ReturnType == typeof(string);
                var returnType = returnsString ? typeof(IntPtr) : interfaceMethod.ReturnType;

                var generator = methodBuilder.GetILGenerator();

                // No need to pin!
                // https://stackoverflow.com/a/2218540/264712
                
                for (int j = 0; j < parameters.Length; ++j)
                    generator.EmitLdarg(j + 1);
                
                generator.EmitPtr(procAddress);
                generator.EmitCalli(
                    OpCodes.Calli,
                    CallingConvention.Cdecl,
                    returnType,
                    parameterTypes);

                if (returnsString)
                    generator.Emit(OpCodes.Call, getCStringMethod);

                generator.Emit(OpCodes.Ret);
            }

            var type = typeBuilder.CreateType() ?? throw new NullReferenceException();
            var libraryInterface = (T)(Activator.CreateInstance(type) ?? throw new NullReferenceException());
            afterLoad?.Invoke(libraryInterface);
            return new NativeLibraryInterface<T>(libraryInterface, libraryHandle, beforeDispose);
        }

        public static IServiceCollection AddNativeLibrary<T>(
            this IServiceCollection services,
            Func<IServiceProvider, NativeLibraryInterface<T>> factory)
            where T : class
        {
            return services
                .AddSingleton(factory)
                .AddSingleton<T>(
                    serviceProvider => serviceProvider.GetRequiredService<NativeLibraryInterface<T>>().Library);
        }
    }

    public sealed class NativeLibraryInterface<T> : IDisposable where T : class
    {
        public T Library { get; }

        private readonly IntPtr _handle;
        private readonly Action<T>? _beforeDispose;

        public NativeLibraryInterface(
            T library,
            IntPtr handle,
            Action<T>? beforeDispose = null)
        {
            Library = library;
            _handle = handle;
            _beforeDispose = beforeDispose;
        }

        public void Dispose()
        {
            try
            {
                _beforeDispose?.Invoke(Library);
            }
            finally
            {
                NativeLibrary.Free(_handle);
            }
        }
    }
}
