using Piranha.Jawbone.Sqlite;
using Piranha.Jawbone.Stb;
using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone;

sealed class JawboneNative : IDisposable
{
    private readonly nint _handle;

    public Sqlite3Library Sqlite3 { get; }
    public StbImageLibrary StbImage { get; }
    public StbImageWriteLibrary StbImageWrite { get; }
    public StbTrueTypeLibrary StbTrueType { get; }
    public StbVorbisLibrary StbVorbis { get; }

    public JawboneNative(string libraryPath)
    {
        _handle = NativeLibrary.Load(libraryPath);

        try
        {
            Sqlite3 = new Sqlite3Library(
                methodName => NativeLibrary.GetExport(
                    _handle, PascalCase.ToSnakeCase("sqlite3", methodName)));
            StbImage = new StbImageLibrary(
                methodName => NativeLibrary.GetExport(
                    _handle, PascalCase.ToSnakeCase("stbi", methodName)));
            StbImageWrite = new StbImageWriteLibrary(
                methodName => NativeLibrary.GetExport(
                    _handle, PascalCase.ToSnakeCase("stbi", methodName)));
            StbTrueType = new StbTrueTypeLibrary(
                methodName => NativeLibrary.GetExport(
                    _handle, "stbtt_" + methodName));
            StbVorbis = new StbVorbisLibrary(
                methodName => NativeLibrary.GetExport(
                    _handle, PascalCase.ToSnakeCase("stb_vorbis", methodName)));
            
            Sqlite3.Initialize();
        }
        catch
        {
            NativeLibrary.Free(_handle);
            throw;
        }
    }

    public void Dispose()
    {
        Sqlite3.Shutdown();
        NativeLibrary.Free(_handle);
    }
}