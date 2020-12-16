using System;
using System.Reflection.Emit;

namespace Piranha.Tools.ReflectionExtensions
{
    public static class ReflectionExtensions
    {
        private static readonly OpCode[] ShortLdarg = new[]
        {
            OpCodes.Ldarg_0,
            OpCodes.Ldarg_1,
            OpCodes.Ldarg_2,
            OpCodes.Ldarg_3
        };

        public static void EmitPtr(
            this ILGenerator generator,
            IntPtr ptr)
        {
            if (Environment.Is64BitProcess)
                generator.Emit(OpCodes.Ldc_I8, ptr.ToInt64());
            else
                generator.Emit(OpCodes.Ldc_I4, ptr.ToInt32());
        }
        
        public static void EmitLdarg(
            this ILGenerator generator,
            int index)
        {
            if (0 <= index && index < ShortLdarg.Length)
                generator.Emit(ShortLdarg[index]);
            else
                generator.Emit(OpCodes.Ldarg, index);
        }
    }
}