using System;

namespace Piranha.Jawbone.OpenAl
{
    public interface IOpenAl
    {
        //al functions
        void GenSources(int n, out uint sources);
        void GenBuffers(int n, out uint buffers);
        void BufferData(
            uint buffer,
            uint format,
            IntPtr data,
            int size,
            int freq);
        void BufferData(
            uint buffer,
            uint format,
            in byte data,
            int size,
            int freq);
        void DeleteSources(int n, in uint buffers);
        void GetSourcei(uint source, uint pname, out int value);
        void Sourcei(uint source, uint param, int value);
        void Sourcei(uint source, uint param, uint value);
        void Sourcef (uint source, uint param, float value);
        void SourcePlay (uint source);
        void SourceStop (uint source);
        void Listenerf (uint param, float value);

        //alc functions
        sbyte CloseDevice(IntPtr device);
        IntPtr CreateContext(IntPtr device, int[]? attrlist);
        void DestroyContext(IntPtr context);
        sbyte MakeContextCurrent(IntPtr context);
        IntPtr OpenDevice(string? devicename);

    }
}
