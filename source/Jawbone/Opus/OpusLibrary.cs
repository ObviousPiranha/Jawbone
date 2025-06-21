// Code generated at 2025-06-21T19:04:39.

namespace Jawbone.Opus;

public sealed unsafe class OpusLibrary
{
    private readonly nint _fp_Decode;
    private readonly nint _fp_DecodeFloat;
    private readonly nint _fp_DecoderCreate;
    private readonly nint _fp_DecoderDestroy;
    private readonly nint _fp_DecoderInit;
    private readonly nint _fp_Encode;
    private readonly nint _fp_EncodeFloat;
    private readonly nint _fp_EncoderCreate;
    private readonly nint _fp_EncoderCtl;
    private readonly nint _fp_EncoderDestroy;
    private readonly nint _fp_EncoderGetSize;
    private readonly nint _fp_EncoderInit;

    public OpusLibrary(
        System.Func<string, nint> loader)
    {
        _fp_Decode = loader.Invoke(nameof(Decode));
        _fp_DecodeFloat = loader.Invoke(nameof(DecodeFloat));
        _fp_DecoderCreate = loader.Invoke(nameof(DecoderCreate));
        _fp_DecoderDestroy = loader.Invoke(nameof(DecoderDestroy));
        _fp_DecoderInit = loader.Invoke(nameof(DecoderInit));
        _fp_Encode = loader.Invoke(nameof(Encode));
        _fp_EncodeFloat = loader.Invoke(nameof(EncodeFloat));
        _fp_EncoderCreate = loader.Invoke(nameof(EncoderCreate));
        _fp_EncoderCtl = loader.Invoke(nameof(EncoderCtl));
        _fp_EncoderDestroy = loader.Invoke(nameof(EncoderDestroy));
        _fp_EncoderGetSize = loader.Invoke(nameof(EncoderGetSize));
        _fp_EncoderInit = loader.Invoke(nameof(EncoderInit));
    }

    public int Decode(
        nint st,
        in byte data,
        int len,
        out short pcm,
        int frameSize,
        int decodeFec)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, void*, int, int, int
            >)_fp_Decode;
        fixed (void* __p_data = &data)
        fixed (void* __p_pcm = &pcm)
        {
            var __result = __fp(st, __p_data, len, __p_pcm, frameSize, decodeFec);
            return __result;
        }
    }

    public int DecodeFloat(
        nint st,
        in byte data,
        int len,
        out float pcm,
        int frameSize,
        int decodeFec)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, void*, int, int, int
            >)_fp_DecodeFloat;
        fixed (void* __p_data = &data)
        fixed (void* __p_pcm = &pcm)
        {
            var __result = __fp(st, __p_data, len, __p_pcm, frameSize, decodeFec);
            return __result;
        }
    }

    public nint DecoderCreate(
        int fs,
        int channels,
        out int error)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, void*, nint
            >)_fp_DecoderCreate;
        fixed (void* __p_error = &error)
        {
            var __result = __fp(fs, channels, __p_error);
            return __result;
        }
    }

    public void DecoderDestroy(
        nint st)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_DecoderDestroy;
        __fp(st);
    }

    public int DecoderInit(
        nint st,
        int fs,
        int channels)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, int
            >)_fp_DecoderInit;
        var __result = __fp(st, fs, channels);
        return __result;
    }

    public int Encode(
        nint st,
        in short pcm,
        int frameSize,
        out byte data,
        int maxDataBytes)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, void*, int, int
            >)_fp_Encode;
        fixed (void* __p_pcm = &pcm)
        fixed (void* __p_data = &data)
        {
            var __result = __fp(st, __p_pcm, frameSize, __p_data, maxDataBytes);
            return __result;
        }
    }

    public int EncodeFloat(
        nint st,
        in float pcm,
        int frameSize,
        out byte data,
        int maxDataBytes)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void*, int, void*, int, int
            >)_fp_EncodeFloat;
        fixed (void* __p_pcm = &pcm)
        fixed (void* __p_data = &data)
        {
            var __result = __fp(st, __p_pcm, frameSize, __p_data, maxDataBytes);
            return __result;
        }
    }

    public nint EncoderCreate(
        int fs,
        int channels,
        int application,
        out int error)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int, int, void*, nint
            >)_fp_EncoderCreate;
        fixed (void* __p_error = &error)
        {
            var __result = __fp(fs, channels, application, __p_error);
            return __result;
        }
    }

    public int EncoderCtl(
        nint st,
        int request,
        out int value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, void*, int
            >)_fp_EncoderCtl;
        fixed (void* __p_value = &value)
        {
            var __result = __fp(st, request, __p_value);
            return __result;
        }
    }

    public int EncoderCtl(
        nint st,
        int request,
        int value)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, int
            >)_fp_EncoderCtl;
        var __result = __fp(st, request, value);
        return __result;
    }

    public void EncoderDestroy(
        nint st)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, void
            >)_fp_EncoderDestroy;
        __fp(st);
    }

    public int EncoderGetSize(
        int channels)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            int, int
            >)_fp_EncoderGetSize;
        var __result = __fp(channels);
        return __result;
    }

    public int EncoderInit(
        nint st,
        int fs,
        int channels,
        int application)
    {
        var __fp = (delegate* unmanaged[Cdecl]<
            nint, int, int, int, int
            >)_fp_EncoderInit;
        var __result = __fp(st, fs, channels, application);
        return __result;
    }
}
