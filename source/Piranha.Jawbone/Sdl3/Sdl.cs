using System;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Sdl3;

#pragma warning disable CA1401

public static partial class Sdl
{
    public const string Lib = "SDL3";

    // #define SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK ((SDL_AudioDeviceID) 0xFFFFFFFFu)
    public const uint AudioDeviceDefaultPlayback = uint.MaxValue;

    public static string GetDefaultLibName()
    {
        if (OperatingSystem.IsWindows())
            return "SDL3.dll";
        else if (OperatingSystem.IsLinux())
            return "libSDL3.so";
        else if (OperatingSystem.IsMacOS())
            return "libSDL3.dylib";
        else
            throw new PlatformNotSupportedException();
    }

    [LibraryImport(Lib, EntryPoint = "SDL_malloc")]
    public static partial nint Malloc(nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_calloc")]
    public static partial nint Calloc(nuint nmemb, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_realloc")]
    public static partial nint Realloc(nint mem, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_free")]
    public static partial void Free(nint mem);

    [LibraryImport(Lib, EntryPoint = "SDL_GetOriginalMemoryFunctions")]
    public static partial void GetOriginalMemoryFunctions(nint malloc_func, nint calloc_func, nint realloc_func, nint free_func);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMemoryFunctions")]
    public static partial void GetMemoryFunctions(nint malloc_func, nint calloc_func, nint realloc_func, nint free_func);

    [LibraryImport(Lib, EntryPoint = "SDL_SetMemoryFunctions")]
    public static partial CBool SetMemoryFunctions(nint malloc_func, nint calloc_func, nint realloc_func, nint free_func);

    [LibraryImport(Lib, EntryPoint = "SDL_aligned_alloc")]
    public static partial nint AlignedAlloc(nuint alignment, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_aligned_free")]
    public static partial void AlignedFree(nint mem);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumAllocations")]
    public static partial int GetNumAllocations();

    [LibraryImport(Lib, EntryPoint = "SDL_GetEnvironment")]
    public static partial nint GetEnvironment();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateEnvironment")]
    public static partial nint CreateEnvironment(CBool populated);

    [LibraryImport(Lib, EntryPoint = "SDL_GetEnvironmentVariable")]
    public static partial nint GetEnvironmentVariable(nint env, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetEnvironmentVariables")]
    public static partial nint GetEnvironmentVariables(nint env);

    [LibraryImport(Lib, EntryPoint = "SDL_SetEnvironmentVariable")]
    public static partial CBool SetEnvironmentVariable(nint env, nint name, nint value, CBool overwrite);

    [LibraryImport(Lib, EntryPoint = "SDL_UnsetEnvironmentVariable")]
    public static partial CBool UnsetEnvironmentVariable(nint env, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyEnvironment")]
    public static partial void DestroyEnvironment(nint env);

    [LibraryImport(Lib, EntryPoint = "SDL_getenv")]
    public static partial nint Getenv(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_getenv_unsafe")]
    public static partial nint GetenvUnsafe(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_setenv_unsafe")]
    public static partial int SetenvUnsafe(nint name, nint value, int overwrite);

    [LibraryImport(Lib, EntryPoint = "SDL_unsetenv_unsafe")]
    public static partial int UnsetenvUnsafe(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_qsort")]
    public static partial void Qsort(nint @base, nuint nmemb, nuint size, nint compare);

    [LibraryImport(Lib, EntryPoint = "SDL_bsearch")]
    public static partial nint Bsearch(nint key, nint @base, nuint nmemb, nuint size, nint compare);

    [LibraryImport(Lib, EntryPoint = "SDL_qsort_r")]
    public static partial void QsortR(nint @base, nuint nmemb, nuint size, nint compare, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_bsearch_r")]
    public static partial nint BsearchR(nint key, nint @base, nuint nmemb, nuint size, nint compare, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_abs")]
    public static partial int Abs(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isalpha")]
    public static partial int Isalpha(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isalnum")]
    public static partial int Isalnum(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isblank")]
    public static partial int Isblank(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_iscntrl")]
    public static partial int Iscntrl(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isdigit")]
    public static partial int Isdigit(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isxdigit")]
    public static partial int Isxdigit(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_ispunct")]
    public static partial int Ispunct(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isspace")]
    public static partial int Isspace(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isupper")]
    public static partial int Isupper(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_islower")]
    public static partial int Islower(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isprint")]
    public static partial int Isprint(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isgraph")]
    public static partial int Isgraph(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_toupper")]
    public static partial int Toupper(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_tolower")]
    public static partial int Tolower(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_crc16")]
    public static partial ushort Crc16(ushort crc, nint data, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_crc32")]
    public static partial uint Crc32(uint crc, nint data, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_murmur3_32")]
    public static partial uint Murmur332(nint data, nuint len, uint seed);

    [LibraryImport(Lib, EntryPoint = "SDL_memcpy")]
    public static partial nint Memcpy(nint dst, nint src, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_memmove")]
    public static partial nint Memmove(nint dst, nint src, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_memset")]
    public static partial nint Memset(nint dst, int c, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_memset4")]
    public static partial nint Memset4(nint dst, uint val, nuint dwords);

    [LibraryImport(Lib, EntryPoint = "SDL_memcmp")]
    public static partial int Memcmp(nint s1, nint s2, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_wcslen")]
    public static partial nuint Wcslen(nint wstr);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsnlen")]
    public static partial nuint Wcsnlen(nint wstr, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcslcpy")]
    public static partial nuint Wcslcpy(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcslcat")]
    public static partial nuint Wcslcat(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsdup")]
    public static partial nint Wcsdup(nint wstr);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsstr")]
    public static partial nint Wcsstr(nint haystack, nint needle);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsnstr")]
    public static partial nint Wcsnstr(nint haystack, nint needle, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcscmp")]
    public static partial int Wcscmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsncmp")]
    public static partial int Wcsncmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcscasecmp")]
    public static partial int Wcscasecmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsncasecmp")]
    public static partial int Wcsncasecmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcstol")]
    public static partial long Wcstol(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strlen")]
    public static partial nuint Strlen(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strnlen")]
    public static partial nuint Strnlen(nint str, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strlcpy")]
    public static partial nuint Strlcpy(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_utf8strlcpy")]
    public static partial nuint Utf8Strlcpy(nint dst, nint src, nuint dst_bytes);

    [LibraryImport(Lib, EntryPoint = "SDL_strlcat")]
    public static partial nuint Strlcat(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strdup")]
    public static partial nint Strdup(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strndup")]
    public static partial nint Strndup(nint str, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strrev")]
    public static partial nint Strrev(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strupr")]
    public static partial nint Strupr(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strlwr")]
    public static partial nint Strlwr(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strchr")]
    public static partial nint Strchr(nint str, int c);

    [LibraryImport(Lib, EntryPoint = "SDL_strrchr")]
    public static partial nint Strrchr(nint str, int c);

    [LibraryImport(Lib, EntryPoint = "SDL_strstr")]
    public static partial nint Strstr(nint haystack, nint needle);

    [LibraryImport(Lib, EntryPoint = "SDL_strnstr")]
    public static partial nint Strnstr(nint haystack, nint needle, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strcasestr")]
    public static partial nint Strcasestr(nint haystack, nint needle);

    [LibraryImport(Lib, EntryPoint = "SDL_strtok_r")]
    public static partial nint StrtokR(nint s1, nint s2, nint saveptr);

    [LibraryImport(Lib, EntryPoint = "SDL_utf8strlen")]
    public static partial nuint Utf8Strlen(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_utf8strnlen")]
    public static partial nuint Utf8Strnlen(nint str, nuint bytes);

    [LibraryImport(Lib, EntryPoint = "SDL_itoa")]
    public static partial nint Itoa(int value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_uitoa")]
    public static partial nint Uitoa(uint value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_ltoa")]
    public static partial nint Ltoa(long value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_ultoa")]
    public static partial nint Ultoa(ulong value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_lltoa")]
    public static partial nint Lltoa(long value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_ulltoa")]
    public static partial nint Ulltoa(ulong value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_atoi")]
    public static partial int Atoi(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_atof")]
    public static partial double Atof(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strtol")]
    public static partial long Strtol(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtoul")]
    public static partial ulong Strtoul(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtoll")]
    public static partial long Strtoll(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtoull")]
    public static partial ulong Strtoull(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtod")]
    public static partial double Strtod(nint str, nint endp);

    [LibraryImport(Lib, EntryPoint = "SDL_strcmp")]
    public static partial int Strcmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_strncmp")]
    public static partial int Strncmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strcasecmp")]
    public static partial int Strcasecmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_strncasecmp")]
    public static partial int Strncasecmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strpbrk")]
    public static partial nint Strpbrk(nint str, nint breakset);

    [LibraryImport(Lib, EntryPoint = "SDL_StepUTF8")]
    public static partial uint StepUTF8(nint pstr, nint pslen);

    [LibraryImport(Lib, EntryPoint = "SDL_UCS4ToUTF8")]
    public static partial nint UCS4ToUTF8(uint codepoint, nint dst);

    [LibraryImport(Lib, EntryPoint = "SDL_sscanf")]
    public static partial int Sscanf(nint text, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_snprintf")]
    public static partial int Snprintf(nint text, nuint maxlen, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_swprintf")]
    public static partial int Swprintf(nint text, nuint maxlen, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_asprintf")]
    public static partial int Asprintf(nint strp, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_srand")]
    public static partial void Srand(ulong seed);

    [LibraryImport(Lib, EntryPoint = "SDL_rand")]
    public static partial int Rand(int n);

    [LibraryImport(Lib, EntryPoint = "SDL_randf")]
    public static partial float Randf();

    [LibraryImport(Lib, EntryPoint = "SDL_rand_bits")]
    public static partial uint RandBits();

    [LibraryImport(Lib, EntryPoint = "SDL_rand_r")]
    public static partial int RandR(nint state, int n);

    [LibraryImport(Lib, EntryPoint = "SDL_randf_r")]
    public static partial float RandfR(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_rand_bits_r")]
    public static partial uint RandBitsR(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_acos")]
    public static partial double Acos(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_acosf")]
    public static partial float Acosf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_asin")]
    public static partial double Asin(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_asinf")]
    public static partial float Asinf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_atan")]
    public static partial double Atan(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_atanf")]
    public static partial float Atanf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_atan2")]
    public static partial double Atan2(double y, double x);

    [LibraryImport(Lib, EntryPoint = "SDL_atan2f")]
    public static partial float Atan2F(float y, float x);

    [LibraryImport(Lib, EntryPoint = "SDL_ceil")]
    public static partial double Ceil(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_ceilf")]
    public static partial float Ceilf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_copysign")]
    public static partial double Copysign(double x, double y);

    [LibraryImport(Lib, EntryPoint = "SDL_copysignf")]
    public static partial float Copysignf(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_cos")]
    public static partial double Cos(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_cosf")]
    public static partial float Cosf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_exp")]
    public static partial double Exp(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_expf")]
    public static partial float Expf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_fabs")]
    public static partial double Fabs(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_fabsf")]
    public static partial float Fabsf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_floor")]
    public static partial double Floor(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_floorf")]
    public static partial float Floorf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_trunc")]
    public static partial double Trunc(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_truncf")]
    public static partial float Truncf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_fmod")]
    public static partial double Fmod(double x, double y);

    [LibraryImport(Lib, EntryPoint = "SDL_fmodf")]
    public static partial float Fmodf(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_isinf")]
    public static partial int Isinf(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_isinff")]
    public static partial int Isinff(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_isnan")]
    public static partial int Isnan(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_isnanf")]
    public static partial int Isnanf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_log")]
    public static partial double Log(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_logf")]
    public static partial float Logf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_log10")]
    public static partial double Log10(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_log10f")]
    public static partial float Log10F(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_modf")]
    public static partial double Modf(double x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_modff")]
    public static partial float Modff(float x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_pow")]
    public static partial double Pow(double x, double y);

    [LibraryImport(Lib, EntryPoint = "SDL_powf")]
    public static partial float Powf(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_round")]
    public static partial double Round(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_roundf")]
    public static partial float Roundf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_lround")]
    public static partial long Lround(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_lroundf")]
    public static partial long Lroundf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_scalbn")]
    public static partial double Scalbn(double x, int n);

    [LibraryImport(Lib, EntryPoint = "SDL_scalbnf")]
    public static partial float Scalbnf(float x, int n);

    [LibraryImport(Lib, EntryPoint = "SDL_sin")]
    public static partial double Sin(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_sinf")]
    public static partial float Sinf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_sqrt")]
    public static partial double Sqrt(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_sqrtf")]
    public static partial float Sqrtf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_tan")]
    public static partial double Tan(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_tanf")]
    public static partial float Tanf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv_open")]
    public static partial nint IconvOpen(nint tocode, nint fromcode);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv_close")]
    public static partial int IconvClose(nint cd);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv")]
    public static partial nuint Iconv(nint cd, nint inbuf, nint inbytesleft, nint outbuf, nint outbytesleft);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv_string")]
    public static partial nint IconvString(nint tocode, nint fromcode, nint inbuf, nuint inbytesleft);

    [LibraryImport(Lib, EntryPoint = "SDL_size_mul_check_overflow")]
    public static partial CBool SizeMulCheckOverflow(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_size_mul_check_overflow_builtin")]
    public static partial CBool SizeMulCheckOverflowBuiltin(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_size_add_check_overflow")]
    public static partial CBool SizeAddCheckOverflow(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_size_add_check_overflow_builtin")]
    public static partial CBool SizeAddCheckOverflowBuiltin(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_ReportAssertion")]
    public static partial SdlAssertState ReportAssertion(nint data, nint func, nint file, int line);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAssertionHandler")]
    public static partial void SetAssertionHandler(nint handler, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDefaultAssertionHandler")]
    public static partial nint GetDefaultAssertionHandler();

    [LibraryImport(Lib, EntryPoint = "SDL_GetAssertionHandler")]
    public static partial nint GetAssertionHandler(nint puserdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAssertionReport")]
    public static partial nint GetAssertionReport();

    [LibraryImport(Lib, EntryPoint = "SDL_ResetAssertionReport")]
    public static partial void ResetAssertionReport();

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockSpinlock")]
    public static partial CBool TryLockSpinlock(nint @lock);

    [LibraryImport(Lib, EntryPoint = "SDL_LockSpinlock")]
    public static partial void LockSpinlock(nint @lock);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockSpinlock")]
    public static partial void UnlockSpinlock(nint @lock);

    [LibraryImport(Lib, EntryPoint = "SDL_MemoryBarrierReleaseFunction")]
    public static partial void MemoryBarrierReleaseFunction();

    [LibraryImport(Lib, EntryPoint = "SDL_MemoryBarrierAcquireFunction")]
    public static partial void MemoryBarrierAcquireFunction();

    [LibraryImport(Lib, EntryPoint = "SDL_CompareAndSwapAtomicInt")]
    public static partial CBool CompareAndSwapAtomicInt(nint a, int oldval, int newval);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAtomicInt")]
    public static partial int SetAtomicInt(nint a, int v);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAtomicInt")]
    public static partial int GetAtomicInt(nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_AddAtomicInt")]
    public static partial int AddAtomicInt(nint a, int v);

    [LibraryImport(Lib, EntryPoint = "SDL_CompareAndSwapAtomicU32")]
    public static partial CBool CompareAndSwapAtomicU32(nint a, uint oldval, uint newval);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAtomicU32")]
    public static partial uint SetAtomicU32(nint a, uint v);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAtomicU32")]
    public static partial uint GetAtomicU32(nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_CompareAndSwapAtomicPointer")]
    public static partial CBool CompareAndSwapAtomicPointer(nint a, nint oldval, nint newval);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAtomicPointer")]
    public static partial nint SetAtomicPointer(nint a, nint v);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAtomicPointer")]
    public static partial nint GetAtomicPointer(nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_SwapFloat")]
    public static partial float SwapFloat(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_SetError")]
    public static partial CBool SetError(nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_OutOfMemory")]
    public static partial CBool OutOfMemory();

    [LibraryImport(Lib, EntryPoint = "SDL_GetError")]
    public static partial nint GetError();

    [LibraryImport(Lib, EntryPoint = "SDL_ClearError")]
    public static partial CBool ClearError();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGlobalProperties")]
    public static partial uint GetGlobalProperties();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateProperties")]
    public static partial uint CreateProperties();

    [LibraryImport(Lib, EntryPoint = "SDL_CopyProperties")]
    public static partial CBool CopyProperties(uint src, uint dst);

    [LibraryImport(Lib, EntryPoint = "SDL_LockProperties")]
    public static partial CBool LockProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockProperties")]
    public static partial void UnlockProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_SetPointerPropertyWithCleanup")]
    public static partial CBool SetPointerPropertyWithCleanup(uint props, nint name, nint value, nint cleanup, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetPointerProperty")]
    public static partial CBool SetPointerProperty(uint props, nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetStringProperty")]
    public static partial CBool SetStringProperty(uint props, nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetNumberProperty")]
    public static partial CBool SetNumberProperty(uint props, nint name, long value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetFloatProperty")]
    public static partial CBool SetFloatProperty(uint props, nint name, float value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetBooleanProperty")]
    public static partial CBool SetBooleanProperty(uint props, nint name, CBool value);

    [LibraryImport(Lib, EntryPoint = "SDL_HasProperty")]
    public static partial CBool HasProperty(uint props, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPropertyType")]
    public static partial SdlPropertyType GetPropertyType(uint props, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPointerProperty")]
    public static partial nint GetPointerProperty(uint props, nint name, nint default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStringProperty")]
    public static partial nint GetStringProperty(uint props, nint name, nint default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumberProperty")]
    public static partial long GetNumberProperty(uint props, nint name, long default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetFloatProperty")]
    public static partial float GetFloatProperty(uint props, nint name, float default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetBooleanProperty")]
    public static partial CBool GetBooleanProperty(uint props, nint name, CBool default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearProperty")]
    public static partial CBool ClearProperty(uint props, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_EnumerateProperties")]
    public static partial CBool EnumerateProperties(uint props, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyProperties")]
    public static partial void DestroyProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateThreadRuntime")]
    public static partial nint CreateThreadRuntime(nint fn, nint name, nint data, nint pfnBeginThread, nint pfnEndThread);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateThreadWithPropertiesRuntime")]
    public static partial nint CreateThreadWithPropertiesRuntime(uint props, nint pfnBeginThread, nint pfnEndThread);

    [LibraryImport(Lib, EntryPoint = "SDL_GetThreadName")]
    public static partial nint GetThreadName(nint thread);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentThreadID")]
    public static partial ulong GetCurrentThreadID();

    [LibraryImport(Lib, EntryPoint = "SDL_GetThreadID")]
    public static partial ulong GetThreadID(nint thread);

    [LibraryImport(Lib, EntryPoint = "SDL_SetCurrentThreadPriority")]
    public static partial CBool SetCurrentThreadPriority(SdlThreadPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitThread")]
    public static partial void WaitThread(nint thread, nint status);

    [LibraryImport(Lib, EntryPoint = "SDL_DetachThread")]
    public static partial void DetachThread(nint thread);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTLS")]
    public static partial nint GetTLS(nint id);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTLS")]
    public static partial CBool SetTLS(nint id, nint value, nint destructor);

    [LibraryImport(Lib, EntryPoint = "SDL_CleanupTLS")]
    public static partial void CleanupTLS();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateMutex")]
    public static partial nint CreateMutex();

    [LibraryImport(Lib, EntryPoint = "SDL_LockMutex")]
    public static partial void LockMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockMutex")]
    public static partial CBool TryLockMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockMutex")]
    public static partial void UnlockMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyMutex")]
    public static partial void DestroyMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateRWLock")]
    public static partial nint CreateRWLock();

    [LibraryImport(Lib, EntryPoint = "SDL_LockRWLockForReading")]
    public static partial void LockRWLockForReading(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_LockRWLockForWriting")]
    public static partial void LockRWLockForWriting(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockRWLockForReading")]
    public static partial CBool TryLockRWLockForReading(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockRWLockForWriting")]
    public static partial CBool TryLockRWLockForWriting(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockRWLock")]
    public static partial void UnlockRWLock(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyRWLock")]
    public static partial void DestroyRWLock(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSemaphore")]
    public static partial nint CreateSemaphore(uint initial_value);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroySemaphore")]
    public static partial void DestroySemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitSemaphore")]
    public static partial void WaitSemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_TryWaitSemaphore")]
    public static partial CBool TryWaitSemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitSemaphoreTimeout")]
    public static partial CBool WaitSemaphoreTimeout(nint sem, int timeoutMS);

    [LibraryImport(Lib, EntryPoint = "SDL_SignalSemaphore")]
    public static partial void SignalSemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSemaphoreValue")]
    public static partial uint GetSemaphoreValue(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateCondition")]
    public static partial nint CreateCondition();

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyCondition")]
    public static partial void DestroyCondition(nint cond);

    [LibraryImport(Lib, EntryPoint = "SDL_SignalCondition")]
    public static partial void SignalCondition(nint cond);

    [LibraryImport(Lib, EntryPoint = "SDL_BroadcastCondition")]
    public static partial void BroadcastCondition(nint cond);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitCondition")]
    public static partial void WaitCondition(nint cond, nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitConditionTimeout")]
    public static partial CBool WaitConditionTimeout(nint cond, nint mutex, int timeoutMS);

    [LibraryImport(Lib, EntryPoint = "SDL_ShouldInit")]
    public static partial CBool ShouldInit(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_ShouldQuit")]
    public static partial CBool ShouldQuit(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_SetInitialized")]
    public static partial void SetInitialized(nint state, CBool initialized);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromFile")]
    public static partial nint IOFromFile(nint file, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromMem")]
    public static partial nint IOFromMem(nint mem, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromConstMem")]
    public static partial nint IOFromConstMem(nint mem, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromDynamicMem")]
    public static partial nint IOFromDynamicMem();

    [LibraryImport(Lib, EntryPoint = "SDL_OpenIO")]
    public static partial nint OpenIO(nint iface, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseIO")]
    public static partial CBool CloseIO(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GetIOProperties")]
    public static partial uint GetIOProperties(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GetIOStatus")]
    public static partial SdlIoStatus GetIOStatus(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GetIOSize")]
    public static partial long GetIOSize(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_SeekIO")]
    public static partial long SeekIO(nint context, long offset, SdlIoWhence whence);

    [LibraryImport(Lib, EntryPoint = "SDL_TellIO")]
    public static partial long TellIO(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadIO")]
    public static partial nuint ReadIO(nint context, nint ptr, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteIO")]
    public static partial nuint WriteIO(nint context, nint ptr, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_IOprintf")]
    public static partial nuint IOprintf(nint context, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushIO")]
    public static partial CBool FlushIO(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadFile_IO")]
    public static partial nint LoadFileIO(nint src, nint datasize, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadFile")]
    public static partial nint LoadFile(nint file, nint datasize);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU8")]
    public static partial CBool ReadU8(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS8")]
    public static partial CBool ReadS8(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU16LE")]
    public static partial CBool ReadU16LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS16LE")]
    public static partial CBool ReadS16LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU16BE")]
    public static partial CBool ReadU16BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS16BE")]
    public static partial CBool ReadS16BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU32LE")]
    public static partial CBool ReadU32LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS32LE")]
    public static partial CBool ReadS32LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU32BE")]
    public static partial CBool ReadU32BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS32BE")]
    public static partial CBool ReadS32BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU64LE")]
    public static partial CBool ReadU64LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS64LE")]
    public static partial CBool ReadS64LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU64BE")]
    public static partial CBool ReadU64BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS64BE")]
    public static partial CBool ReadS64BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU8")]
    public static partial CBool WriteU8(nint dst, byte value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS8")]
    public static partial CBool WriteS8(nint dst, sbyte value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU16LE")]
    public static partial CBool WriteU16LE(nint dst, ushort value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS16LE")]
    public static partial CBool WriteS16LE(nint dst, short value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU16BE")]
    public static partial CBool WriteU16BE(nint dst, ushort value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS16BE")]
    public static partial CBool WriteS16BE(nint dst, short value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU32LE")]
    public static partial CBool WriteU32LE(nint dst, uint value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS32LE")]
    public static partial CBool WriteS32LE(nint dst, int value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU32BE")]
    public static partial CBool WriteU32BE(nint dst, uint value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS32BE")]
    public static partial CBool WriteS32BE(nint dst, int value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU64LE")]
    public static partial CBool WriteU64LE(nint dst, ulong value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS64LE")]
    public static partial CBool WriteS64LE(nint dst, long value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU64BE")]
    public static partial CBool WriteU64BE(nint dst, ulong value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS64BE")]
    public static partial CBool WriteS64BE(nint dst, long value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumAudioDrivers")]
    public static partial int GetNumAudioDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDriver")]
    public static partial nint GetAudioDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentAudioDriver")]
    public static partial nint GetCurrentAudioDriver();

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioPlaybackDevices")]
    public static partial nint GetAudioPlaybackDevices(out int count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioRecordingDevices")]
    public static partial nint GetAudioRecordingDevices(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceName")]
    public static partial nint GetAudioDeviceName(uint devid);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceFormat")]
    public static partial CBool GetAudioDeviceFormat(uint devid, nint spec, nint sample_frames);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceChannelMap")]
    public static partial nint GetAudioDeviceChannelMap(uint devid, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenAudioDevice")]
    public static partial uint OpenAudioDevice(uint devid, in SdlAudioSpec spec);

    [LibraryImport(Lib, EntryPoint = "SDL_PauseAudioDevice")]
    public static partial CBool PauseAudioDevice(uint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_ResumeAudioDevice")]
    public static partial CBool ResumeAudioDevice(uint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_AudioDevicePaused")]
    public static partial CBool AudioDevicePaused(uint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceGain")]
    public static partial float GetAudioDeviceGain(uint devid);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioDeviceGain")]
    public static partial CBool SetAudioDeviceGain(uint devid, float gain);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseAudioDevice")]
    public static partial void CloseAudioDevice(uint devid);

    [LibraryImport(Lib, EntryPoint = "SDL_BindAudioStreams")]
    public static partial CBool BindAudioStreams(uint devid, nint streams, int num_streams);

    [LibraryImport(Lib, EntryPoint = "SDL_BindAudioStream")]
    public static partial CBool BindAudioStream(uint devid, nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_UnbindAudioStreams")]
    public static partial void UnbindAudioStreams(nint streams, int num_streams);

    [LibraryImport(Lib, EntryPoint = "SDL_UnbindAudioStream")]
    public static partial void UnbindAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamDevice")]
    public static partial uint GetAudioStreamDevice(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateAudioStream")]
    public static partial nint CreateAudioStream(in SdlAudioSpec src_spec, in SdlAudioSpec dst_spec);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamProperties")]
    public static partial uint GetAudioStreamProperties(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamFormat")]
    public static partial CBool GetAudioStreamFormat(nint stream, nint src_spec, nint dst_spec);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamFormat")]
    public static partial CBool SetAudioStreamFormat(nint stream, nint src_spec, nint dst_spec);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamFrequencyRatio")]
    public static partial float GetAudioStreamFrequencyRatio(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamFrequencyRatio")]
    public static partial CBool SetAudioStreamFrequencyRatio(nint stream, float ratio);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamGain")]
    public static partial float GetAudioStreamGain(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamGain")]
    public static partial CBool SetAudioStreamGain(nint stream, float gain);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamInputChannelMap")]
    public static partial nint GetAudioStreamInputChannelMap(nint stream, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamOutputChannelMap")]
    public static partial nint GetAudioStreamOutputChannelMap(nint stream, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamInputChannelMap")]
    public static partial CBool SetAudioStreamInputChannelMap(nint stream, nint chmap, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamOutputChannelMap")]
    public static partial CBool SetAudioStreamOutputChannelMap(nint stream, nint chmap, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_PutAudioStreamData")]
    public static partial CBool PutAudioStreamData(nint stream, in byte buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_PutAudioStreamData")]
    public static partial CBool PutAudioStreamData(nint stream, in short buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_PutAudioStreamData")]
    public static partial CBool PutAudioStreamData(nint stream, in float buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamData")]
    public static partial int GetAudioStreamData(nint stream, out byte buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamData")]
    public static partial int GetAudioStreamData(nint stream, out short buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamData")]
    public static partial int GetAudioStreamData(nint stream, out float buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamAvailable")]
    public static partial int GetAudioStreamAvailable(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamQueued")]
    public static partial int GetAudioStreamQueued(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushAudioStream")]
    public static partial CBool FlushAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearAudioStream")]
    public static partial CBool ClearAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_PauseAudioStreamDevice")]
    public static partial CBool PauseAudioStreamDevice(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_ResumeAudioStreamDevice")]
    public static partial CBool ResumeAudioStreamDevice(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_LockAudioStream")]
    public static partial CBool LockAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockAudioStream")]
    public static partial CBool UnlockAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamGetCallback")]
    public static partial CBool SetAudioStreamGetCallback(nint stream, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamPutCallback")]
    public static partial CBool SetAudioStreamPutCallback(nint stream, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyAudioStream")]
    public static partial void DestroyAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenAudioDeviceStream")]
    public static partial nint OpenAudioDeviceStream(uint devid, nint spec, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioPostmixCallback")]
    public static partial CBool SetAudioPostmixCallback(uint devid, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadWAV_IO")]
    public static partial CBool LoadWAVIO(nint src, CBool closeio, nint spec, nint audio_buf, nint audio_len);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadWAV")]
    public static partial CBool LoadWAV(nint path, nint spec, nint audio_buf, nint audio_len);

    [LibraryImport(Lib, EntryPoint = "SDL_MixAudio")]
    public static partial CBool MixAudio(nint dst, nint src, SdlAudioFormat format, uint len, float volume);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertAudioSamples")]
    public static partial CBool ConvertAudioSamples(nint src_spec, nint src_data, int src_len, nint dst_spec, nint dst_data, nint dst_len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioFormatName")]
    public static partial nint GetAudioFormatName(SdlAudioFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSilenceValueForFormat")]
    public static partial int GetSilenceValueForFormat(SdlAudioFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_MostSignificantBitIndex32")]
    public static partial int MostSignificantBitIndex32(uint x);

    [LibraryImport(Lib, EntryPoint = "SDL_HasExactlyOneBitSet32")]
    public static partial CBool HasExactlyOneBitSet32(uint x);

    [LibraryImport(Lib, EntryPoint = "SDL_ComposeCustomBlendMode")]
    public static partial uint ComposeCustomBlendMode(SdlBlendFactor srcColorFactor, SdlBlendFactor dstColorFactor, SdlBlendOperation colorOperation, SdlBlendFactor srcAlphaFactor, SdlBlendFactor dstAlphaFactor, SdlBlendOperation alphaOperation);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPixelFormatName")]
    public static partial nint GetPixelFormatName(SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMasksForPixelFormat")]
    public static partial CBool GetMasksForPixelFormat(SdlPixelFormat format, nint bpp, nint Rmask, nint Gmask, nint Bmask, nint Amask);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPixelFormatForMasks")]
    public static partial SdlPixelFormat GetPixelFormatForMasks(int bpp, uint Rmask, uint Gmask, uint Bmask, uint Amask);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPixelFormatDetails")]
    public static partial nint GetPixelFormatDetails(SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_CreatePalette")]
    public static partial nint CreatePalette(int ncolors);

    [LibraryImport(Lib, EntryPoint = "SDL_SetPaletteColors")]
    public static partial CBool SetPaletteColors(nint palette, nint colors, int firstcolor, int ncolors);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyPalette")]
    public static partial void DestroyPalette(nint palette);

    [LibraryImport(Lib, EntryPoint = "SDL_MapRGB")]
    public static partial uint MapRGB(nint format, nint palette, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_MapRGBA")]
    public static partial uint MapRGBA(nint format, nint palette, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRGB")]
    public static partial void GetRGB(uint pixel, nint format, nint palette, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRGBA")]
    public static partial void GetRGBA(uint pixel, nint format, nint palette, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_RectToFRect")]
    public static partial void RectToFRect(nint rect, nint frect);

    [LibraryImport(Lib, EntryPoint = "SDL_PointInRect")]
    public static partial CBool PointInRect(nint p, nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectEmpty")]
    public static partial CBool RectEmpty(nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectsEqual")]
    public static partial CBool RectsEqual(nint a, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_HasRectIntersection")]
    public static partial CBool HasRectIntersection(nint A, nint B);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectIntersection")]
    public static partial CBool GetRectIntersection(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectUnion")]
    public static partial CBool GetRectUnion(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectEnclosingPoints")]
    public static partial CBool GetRectEnclosingPoints(nint points, int count, nint clip, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectAndLineIntersection")]
    public static partial CBool GetRectAndLineIntersection(nint rect, nint X1, nint Y1, nint X2, nint Y2);

    [LibraryImport(Lib, EntryPoint = "SDL_PointInRectFloat")]
    public static partial CBool PointInRectFloat(nint p, nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectEmptyFloat")]
    public static partial CBool RectEmptyFloat(nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectsEqualEpsilon")]
    public static partial CBool RectsEqualEpsilon(nint a, nint b, float epsilon);

    [LibraryImport(Lib, EntryPoint = "SDL_RectsEqualFloat")]
    public static partial CBool RectsEqualFloat(nint a, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_HasRectIntersectionFloat")]
    public static partial CBool HasRectIntersectionFloat(nint A, nint B);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectIntersectionFloat")]
    public static partial CBool GetRectIntersectionFloat(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectUnionFloat")]
    public static partial CBool GetRectUnionFloat(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectEnclosingPointsFloat")]
    public static partial CBool GetRectEnclosingPointsFloat(nint points, int count, nint clip, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectAndLineIntersectionFloat")]
    public static partial CBool GetRectAndLineIntersectionFloat(nint rect, nint X1, nint Y1, nint X2, nint Y2);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSurface")]
    public static partial nint CreateSurface(int width, int height, SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSurfaceFrom")]
    public static partial nint CreateSurfaceFrom(int width, int height, SdlPixelFormat format, nint pixels, int pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroySurface")]
    public static partial void DestroySurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceProperties")]
    public static partial uint GetSurfaceProperties(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceColorspace")]
    public static partial CBool SetSurfaceColorspace(nint surface, SdlColorspace colorspace);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceColorspace")]
    public static partial SdlColorspace GetSurfaceColorspace(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSurfacePalette")]
    public static partial nint CreateSurfacePalette(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfacePalette")]
    public static partial CBool SetSurfacePalette(nint surface, nint palette);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfacePalette")]
    public static partial nint GetSurfacePalette(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_AddSurfaceAlternateImage")]
    public static partial CBool AddSurfaceAlternateImage(nint surface, nint image);

    [LibraryImport(Lib, EntryPoint = "SDL_SurfaceHasAlternateImages")]
    public static partial CBool SurfaceHasAlternateImages(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceImages")]
    public static partial nint GetSurfaceImages(nint surface, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveSurfaceAlternateImages")]
    public static partial void RemoveSurfaceAlternateImages(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_LockSurface")]
    public static partial CBool LockSurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockSurface")]
    public static partial void UnlockSurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadBMP_IO")]
    public static partial nint LoadBMPIO(nint src, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadBMP")]
    public static partial nint LoadBMP(nint file);

    [LibraryImport(Lib, EntryPoint = "SDL_SaveBMP_IO")]
    public static partial CBool SaveBMPIO(nint surface, nint dst, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_SaveBMP")]
    public static partial CBool SaveBMP(nint surface, nint file);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceRLE")]
    public static partial CBool SetSurfaceRLE(nint surface, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_SurfaceHasRLE")]
    public static partial CBool SurfaceHasRLE(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceColorKey")]
    public static partial CBool SetSurfaceColorKey(nint surface, CBool enabled, uint key);

    [LibraryImport(Lib, EntryPoint = "SDL_SurfaceHasColorKey")]
    public static partial CBool SurfaceHasColorKey(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceColorKey")]
    public static partial CBool GetSurfaceColorKey(nint surface, nint key);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceColorMod")]
    public static partial CBool SetSurfaceColorMod(nint surface, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceColorMod")]
    public static partial CBool GetSurfaceColorMod(nint surface, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceAlphaMod")]
    public static partial CBool SetSurfaceAlphaMod(nint surface, byte alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceAlphaMod")]
    public static partial CBool GetSurfaceAlphaMod(nint surface, nint alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceBlendMode")]
    public static partial CBool SetSurfaceBlendMode(nint surface, uint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceBlendMode")]
    public static partial CBool GetSurfaceBlendMode(nint surface, nint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceClipRect")]
    public static partial CBool SetSurfaceClipRect(nint surface, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceClipRect")]
    public static partial CBool GetSurfaceClipRect(nint surface, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_FlipSurface")]
    public static partial CBool FlipSurface(nint surface, SdlFlipMode flip);

    [LibraryImport(Lib, EntryPoint = "SDL_DuplicateSurface")]
    public static partial nint DuplicateSurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_ScaleSurface")]
    public static partial nint ScaleSurface(nint surface, int width, int height, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertSurface")]
    public static partial nint ConvertSurface(nint surface, SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertSurfaceAndColorspace")]
    public static partial nint ConvertSurfaceAndColorspace(nint surface, SdlPixelFormat format, nint palette, SdlColorspace colorspace, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertPixels")]
    public static partial CBool ConvertPixels(int width, int height, SdlPixelFormat src_format, nint src, int src_pitch, SdlPixelFormat dst_format, nint dst, int dst_pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertPixelsAndColorspace")]
    public static partial CBool ConvertPixelsAndColorspace(int width, int height, SdlPixelFormat src_format, SdlColorspace src_colorspace, uint src_properties, nint src, int src_pitch, SdlPixelFormat dst_format, SdlColorspace dst_colorspace, uint dst_properties, nint dst, int dst_pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_PremultiplyAlpha")]
    public static partial CBool PremultiplyAlpha(int width, int height, SdlPixelFormat src_format, nint src, int src_pitch, SdlPixelFormat dst_format, nint dst, int dst_pitch, CBool linear);

    [LibraryImport(Lib, EntryPoint = "SDL_PremultiplySurfaceAlpha")]
    public static partial CBool PremultiplySurfaceAlpha(nint surface, CBool linear);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearSurface")]
    public static partial CBool ClearSurface(nint surface, float r, float g, float b, float a);

    [LibraryImport(Lib, EntryPoint = "SDL_FillSurfaceRect")]
    public static partial CBool FillSurfaceRect(nint dst, nint rect, uint color);

    [LibraryImport(Lib, EntryPoint = "SDL_FillSurfaceRects")]
    public static partial CBool FillSurfaceRects(nint dst, nint rects, int count, uint color);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurface")]
    public static partial CBool BlitSurface(nint src, nint srcrect, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceUnchecked")]
    public static partial CBool BlitSurfaceUnchecked(nint src, nint srcrect, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceScaled")]
    public static partial CBool BlitSurfaceScaled(nint src, nint srcrect, nint dst, nint dstrect, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceUncheckedScaled")]
    public static partial CBool BlitSurfaceUncheckedScaled(nint src, nint srcrect, nint dst, nint dstrect, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceTiled")]
    public static partial CBool BlitSurfaceTiled(nint src, nint srcrect, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceTiledWithScale")]
    public static partial CBool BlitSurfaceTiledWithScale(nint src, nint srcrect, float scale, SdlScaleMode scaleMode, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurface9Grid")]
    public static partial CBool BlitSurface9Grid(nint src, nint srcrect, int left_width, int right_width, int top_height, int bottom_height, float scale, SdlScaleMode scaleMode, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_MapSurfaceRGB")]
    public static partial uint MapSurfaceRGB(nint surface, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_MapSurfaceRGBA")]
    public static partial uint MapSurfaceRGBA(nint surface, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadSurfacePixel")]
    public static partial CBool ReadSurfacePixel(nint surface, int x, int y, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadSurfacePixelFloat")]
    public static partial CBool ReadSurfacePixelFloat(nint surface, int x, int y, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteSurfacePixel")]
    public static partial CBool WriteSurfacePixel(nint surface, int x, int y, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteSurfacePixelFloat")]
    public static partial CBool WriteSurfacePixelFloat(nint surface, int x, int y, float r, float g, float b, float a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumCameraDrivers")]
    public static partial int GetNumCameraDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraDriver")]
    public static partial nint GetCameraDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentCameraDriver")]
    public static partial nint GetCurrentCameraDriver();

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameras")]
    public static partial nint GetCameras(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraSupportedFormats")]
    public static partial nint GetCameraSupportedFormats(uint devid, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraName")]
    public static partial nint GetCameraName(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraPosition")]
    public static partial SdlCameraPosition GetCameraPosition(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenCamera")]
    public static partial nint OpenCamera(uint instance_id, nint spec);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraPermissionState")]
    public static partial int GetCameraPermissionState(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraID")]
    public static partial uint GetCameraID(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraProperties")]
    public static partial uint GetCameraProperties(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraFormat")]
    public static partial CBool GetCameraFormat(nint camera, nint spec);

    [LibraryImport(Lib, EntryPoint = "SDL_AcquireCameraFrame")]
    public static partial nint AcquireCameraFrame(nint camera, nint timestampNS);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseCameraFrame")]
    public static partial void ReleaseCameraFrame(nint camera, nint frame);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseCamera")]
    public static partial void CloseCamera(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_SetClipboardText")]
    public static partial CBool SetClipboardText(nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_GetClipboardText")]
    public static partial nint GetClipboardText();

    [LibraryImport(Lib, EntryPoint = "SDL_HasClipboardText")]
    public static partial CBool HasClipboardText();

    [LibraryImport(Lib, EntryPoint = "SDL_SetPrimarySelectionText")]
    public static partial CBool SetPrimarySelectionText(nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPrimarySelectionText")]
    public static partial nint GetPrimarySelectionText();

    [LibraryImport(Lib, EntryPoint = "SDL_HasPrimarySelectionText")]
    public static partial CBool HasPrimarySelectionText();

    [LibraryImport(Lib, EntryPoint = "SDL_SetClipboardData")]
    public static partial CBool SetClipboardData(nint callback, nint cleanup, nint userdata, nint mime_types, nuint num_mime_types);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearClipboardData")]
    public static partial CBool ClearClipboardData();

    [LibraryImport(Lib, EntryPoint = "SDL_GetClipboardData")]
    public static partial nint GetClipboardData(nint mime_type, nint size);

    [LibraryImport(Lib, EntryPoint = "SDL_HasClipboardData")]
    public static partial CBool HasClipboardData(nint mime_type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetClipboardMimeTypes")]
    public static partial nint GetClipboardMimeTypes(nint num_mime_types);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumLogicalCPUCores")]
    public static partial int GetNumLogicalCpuCores();

    [LibraryImport(Lib, EntryPoint = "SDL_GetCPUCacheLineSize")]
    public static partial int GetCpuCacheLineSize();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAltiVec")]
    public static partial CBool HasAltiVec();

    [LibraryImport(Lib, EntryPoint = "SDL_HasMMX")]
    public static partial CBool HasMMX();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE")]
    public static partial CBool HasSSE();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE2")]
    public static partial CBool HasSSE2();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE3")]
    public static partial CBool HasSSE3();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE41")]
    public static partial CBool HasSSE41();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE42")]
    public static partial CBool HasSSE42();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAVX")]
    public static partial CBool HasAVX();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAVX2")]
    public static partial CBool HasAVX2();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAVX512F")]
    public static partial CBool HasAVX512F();

    [LibraryImport(Lib, EntryPoint = "SDL_HasARMSIMD")]
    public static partial CBool HasARMSIMD();

    [LibraryImport(Lib, EntryPoint = "SDL_HasNEON")]
    public static partial CBool HasNEON();

    [LibraryImport(Lib, EntryPoint = "SDL_HasLSX")]
    public static partial CBool HasLSX();

    [LibraryImport(Lib, EntryPoint = "SDL_HasLASX")]
    public static partial CBool HasLASX();

    [LibraryImport(Lib, EntryPoint = "SDL_GetSystemRAM")]
    public static partial int GetSystemRAM();

    [LibraryImport(Lib, EntryPoint = "SDL_GetSIMDAlignment")]
    public static partial nuint GetSIMDAlignment();

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumVideoDrivers")]
    public static partial int GetNumVideoDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetVideoDriver")]
    public static partial nint GetVideoDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentVideoDriver")]
    public static partial nint GetCurrentVideoDriver();

    [LibraryImport(Lib, EntryPoint = "SDL_GetSystemTheme")]
    public static partial SdlSystemTheme GetSystemTheme();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplays")]
    public static partial nint GetDisplays(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPrimaryDisplay")]
    public static partial uint GetPrimaryDisplay();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayProperties")]
    public static partial uint GetDisplayProperties(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayName")]
    public static partial nint GetDisplayName(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayBounds")]
    public static partial CBool GetDisplayBounds(uint displayID, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayUsableBounds")]
    public static partial CBool GetDisplayUsableBounds(uint displayID, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNaturalDisplayOrientation")]
    public static partial SdlDisplayOrientation GetNaturalDisplayOrientation(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentDisplayOrientation")]
    public static partial SdlDisplayOrientation GetCurrentDisplayOrientation(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayContentScale")]
    public static partial float GetDisplayContentScale(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetFullscreenDisplayModes")]
    public static partial nint GetFullscreenDisplayModes(uint displayID, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetClosestFullscreenDisplayMode")]
    public static partial CBool GetClosestFullscreenDisplayMode(uint displayID, int w, int h, float refresh_rate, CBool include_high_density_modes, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDesktopDisplayMode")]
    public static partial nint GetDesktopDisplayMode(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentDisplayMode")]
    public static partial nint GetCurrentDisplayMode(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayForPoint")]
    public static partial uint GetDisplayForPoint(nint point);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayForRect")]
    public static partial uint GetDisplayForRect(nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayForWindow")]
    public static partial uint GetDisplayForWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowPixelDensity")]
    public static partial float GetWindowPixelDensity(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowDisplayScale")]
    public static partial float GetWindowDisplayScale(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowFullscreenMode")]
    public static partial CBool SetWindowFullscreenMode(nint window, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFullscreenMode")]
    public static partial nint GetWindowFullscreenMode(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowICCProfile")]
    public static partial nint GetWindowICCProfile(nint window, nint size);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowPixelFormat")]
    public static partial SdlPixelFormat GetWindowPixelFormat(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindows")]
    public static partial nint GetWindows(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateWindow", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint CreateWindow(string title, int w, int h, SdlWindowFlags flags);

    [LibraryImport(Lib, EntryPoint = "SDL_CreatePopupWindow")]
    public static partial nint CreatePopupWindow(nint parent, int offset_x, int offset_y, int w, int h, ulong flags);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateWindowWithProperties")]
    public static partial nint CreateWindowWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowID")]
    public static partial uint GetWindowID(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFromID")]
    public static partial nint GetWindowFromID(uint id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowParent")]
    public static partial nint GetWindowParent(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowProperties")]
    public static partial uint GetWindowProperties(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFlags")]
    public static partial ulong GetWindowFlags(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowTitle")]
    public static partial CBool SetWindowTitle(nint window, nint title);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowTitle")]
    public static partial nint GetWindowTitle(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowIcon")]
    public static partial CBool SetWindowIcon(nint window, nint icon);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowPosition")]
    public static partial CBool SetWindowPosition(nint window, int x, int y);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowPosition")]
    public static partial CBool GetWindowPosition(nint window, nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowSize")]
    public static partial CBool SetWindowSize(nint window, int w, int h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSize")]
    public static partial CBool GetWindowSize(nint window, out int w, out int h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSafeArea")]
    public static partial CBool GetWindowSafeArea(nint window, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowAspectRatio")]
    public static partial CBool SetWindowAspectRatio(nint window, float min_aspect, float max_aspect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowAspectRatio")]
    public static partial CBool GetWindowAspectRatio(nint window, nint min_aspect, nint max_aspect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowBordersSize")]
    public static partial CBool GetWindowBordersSize(nint window, nint top, nint left, nint bottom, nint right);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSizeInPixels")]
    public static partial CBool GetWindowSizeInPixels(nint window, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMinimumSize")]
    public static partial CBool SetWindowMinimumSize(nint window, int min_w, int min_h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMinimumSize")]
    public static partial CBool GetWindowMinimumSize(nint window, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMaximumSize")]
    public static partial CBool SetWindowMaximumSize(nint window, int max_w, int max_h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMaximumSize")]
    public static partial CBool GetWindowMaximumSize(nint window, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowBordered")]
    public static partial CBool SetWindowBordered(nint window, CBool bordered);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowResizable")]
    public static partial CBool SetWindowResizable(nint window, CBool resizable);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowAlwaysOnTop")]
    public static partial CBool SetWindowAlwaysOnTop(nint window, CBool on_top);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowWindow")]
    public static partial CBool ShowWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_HideWindow")]
    public static partial CBool HideWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_RaiseWindow")]
    public static partial CBool RaiseWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_MaximizeWindow")]
    public static partial CBool MaximizeWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_MinimizeWindow")]
    public static partial CBool MinimizeWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_RestoreWindow")]
    public static partial CBool RestoreWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowFullscreen")]
    public static partial CBool SetWindowFullscreen(nint window, CBool fullscreen);

    [LibraryImport(Lib, EntryPoint = "SDL_SyncWindow")]
    public static partial CBool SyncWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_WindowHasSurface")]
    public static partial CBool WindowHasSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSurface")]
    public static partial nint GetWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowSurfaceVSync")]
    public static partial CBool SetWindowSurfaceVSync(nint window, int vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSurfaceVSync")]
    public static partial CBool GetWindowSurfaceVSync(nint window, nint vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateWindowSurface")]
    public static partial CBool UpdateWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateWindowSurfaceRects")]
    public static partial CBool UpdateWindowSurfaceRects(nint window, nint rects, int numrects);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyWindowSurface")]
    public static partial CBool DestroyWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowKeyboardGrab")]
    public static partial CBool SetWindowKeyboardGrab(nint window, CBool grabbed);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMouseGrab")]
    public static partial CBool SetWindowMouseGrab(nint window, CBool grabbed);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowKeyboardGrab")]
    public static partial CBool GetWindowKeyboardGrab(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMouseGrab")]
    public static partial CBool GetWindowMouseGrab(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGrabbedWindow")]
    public static partial nint GetGrabbedWindow();

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMouseRect")]
    public static partial CBool SetWindowMouseRect(nint window, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMouseRect")]
    public static partial nint GetWindowMouseRect(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowOpacity")]
    public static partial CBool SetWindowOpacity(nint window, float opacity);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowOpacity")]
    public static partial float GetWindowOpacity(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowParent")]
    public static partial CBool SetWindowParent(nint window, nint parent);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowModal")]
    public static partial CBool SetWindowModal(nint window, CBool modal);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowFocusable")]
    public static partial CBool SetWindowFocusable(nint window, CBool focusable);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowWindowSystemMenu")]
    public static partial CBool ShowWindowSystemMenu(nint window, int x, int y);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowHitTest")]
    public static partial CBool SetWindowHitTest(nint window, nint callback, nint callback_data);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowShape")]
    public static partial CBool SetWindowShape(nint window, nint shape);

    [LibraryImport(Lib, EntryPoint = "SDL_FlashWindow")]
    public static partial CBool FlashWindow(nint window, SdlFlashOperation operation);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyWindow")]
    public static partial void DestroyWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_ScreenSaverEnabled")]
    public static partial CBool ScreenSaverEnabled();

    [LibraryImport(Lib, EntryPoint = "SDL_EnableScreenSaver")]
    public static partial CBool EnableScreenSaver();

    [LibraryImport(Lib, EntryPoint = "SDL_DisableScreenSaver")]
    public static partial CBool DisableScreenSaver();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_LoadLibrary")]
    public static partial CBool GlLoadLibrary(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetProcAddress", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint GlGetProcAddress(string proc);

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetProcAddress", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint EglGetProcAddress(string proc);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_UnloadLibrary")]
    public static partial void GlUnloadLibrary();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_ExtensionSupported")]
    public static partial CBool GlExtensionSupported(nint extension);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_ResetAttributes")]
    public static partial void GlResetAttributes();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_SetAttribute")]
    public static partial CBool GlSetAttribute(SdlGlAttr attr, int value);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetAttribute")]
    public static partial CBool GlGetAttribute(SdlGlAttr attr, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_CreateContext")]
    public static partial nint GlCreateContext(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_MakeCurrent")]
    public static partial CBool GlMakeCurrent(nint window, nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetCurrentWindow")]
    public static partial nint GlGetCurrentWindow();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetCurrentContext")]
    public static partial nint GlGetCurrentContext();

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetCurrentDisplay")]
    public static partial nint EglGetCurrentDisplay();

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetCurrentConfig")]
    public static partial nint EglGetCurrentConfig();

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetWindowSurface")]
    public static partial nint EglGetWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_SetAttributeCallbacks")]
    public static partial void EglSetAttributeCallbacks(nint platformAttribCallback, nint surfaceAttribCallback, nint contextAttribCallback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_SetSwapInterval")]
    public static partial CBool GlSetSwapInterval(int interval);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetSwapInterval")]
    public static partial CBool GlGetSwapInterval(nint interval);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_SwapWindow")]
    public static partial CBool GlSwapWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_DestroyContext")]
    public static partial CBool GlDestroyContext(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowOpenFileDialog")]
    public static partial void ShowOpenFileDialog(nint callback, nint userdata, nint window, nint filters, int nfilters, nint default_location, CBool allow_many);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowSaveFileDialog")]
    public static partial void ShowSaveFileDialog(nint callback, nint userdata, nint window, nint filters, int nfilters, nint default_location);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowOpenFolderDialog")]
    public static partial void ShowOpenFolderDialog(nint callback, nint userdata, nint window, nint default_location, CBool allow_many);

    [LibraryImport(Lib, EntryPoint = "SDL_GUIDToString")]
    public static partial void GUIDToString(nint guid, nint pszGUID, int cbGUID);

    [LibraryImport(Lib, EntryPoint = "SDL_StringToGUID")]
    public static partial nint StringToGUID(nint pchGUID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPowerInfo")]
    public static partial SdlPowerState GetPowerInfo(nint seconds, nint percent);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensors")]
    public static partial nint GetSensors(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorNameForID")]
    public static partial nint GetSensorNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorTypeForID")]
    public static partial SdlSensorType GetSensorTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorNonPortableTypeForID")]
    public static partial int GetSensorNonPortableTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenSensor")]
    public static partial nint OpenSensor(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorFromID")]
    public static partial nint GetSensorFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorProperties")]
    public static partial uint GetSensorProperties(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorName")]
    public static partial nint GetSensorName(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorType")]
    public static partial SdlSensorType GetSensorType(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorNonPortableType")]
    public static partial int GetSensorNonPortableType(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorID")]
    public static partial uint GetSensorID(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorData")]
    public static partial CBool GetSensorData(nint sensor, nint data, int num_values);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseSensor")]
    public static partial void CloseSensor(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateSensors")]
    public static partial void UpdateSensors();

    [LibraryImport(Lib, EntryPoint = "SDL_LockJoysticks")]
    public static partial void LockJoysticks();

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockJoysticks")]
    public static partial void UnlockJoysticks();

    [LibraryImport(Lib, EntryPoint = "SDL_HasJoystick")]
    public static partial CBool HasJoystick();

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoysticks")]
    public static partial nint GetJoysticks(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickNameForID")]
    public static partial nint GetJoystickNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPathForID")]
    public static partial nint GetJoystickPathForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPlayerIndexForID")]
    public static partial int GetJoystickPlayerIndexForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickGUIDForID")]
    public static partial nint GetJoystickGUIDForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickVendorForID")]
    public static partial ushort GetJoystickVendorForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProductForID")]
    public static partial ushort GetJoystickProductForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProductVersionForID")]
    public static partial ushort GetJoystickProductVersionForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickTypeForID")]
    public static partial SdlJoystickType GetJoystickTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenJoystick")]
    public static partial nint OpenJoystick(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickFromID")]
    public static partial nint GetJoystickFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickFromPlayerIndex")]
    public static partial nint GetJoystickFromPlayerIndex(int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_AttachVirtualJoystick")]
    public static partial uint AttachVirtualJoystick(nint desc);

    [LibraryImport(Lib, EntryPoint = "SDL_DetachVirtualJoystick")]
    public static partial CBool DetachVirtualJoystick(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_IsJoystickVirtual")]
    public static partial CBool IsJoystickVirtual(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualAxis")]
    public static partial CBool SetJoystickVirtualAxis(nint joystick, int axis, short value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualBall")]
    public static partial CBool SetJoystickVirtualBall(nint joystick, int ball, short xrel, short yrel);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualButton")]
    public static partial CBool SetJoystickVirtualButton(nint joystick, int button, CBool down);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualHat")]
    public static partial CBool SetJoystickVirtualHat(nint joystick, int hat, byte value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualTouchpad")]
    public static partial CBool SetJoystickVirtualTouchpad(nint joystick, int touchpad, int finger, CBool down, float x, float y, float pressure);

    [LibraryImport(Lib, EntryPoint = "SDL_SendJoystickVirtualSensorData")]
    public static partial CBool SendJoystickVirtualSensorData(nint joystick, SdlSensorType type, ulong sensor_timestamp, nint data, int num_values);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProperties")]
    public static partial uint GetJoystickProperties(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickName")]
    public static partial nint GetJoystickName(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPath")]
    public static partial nint GetJoystickPath(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPlayerIndex")]
    public static partial int GetJoystickPlayerIndex(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickPlayerIndex")]
    public static partial CBool SetJoystickPlayerIndex(nint joystick, int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickGUID")]
    public static partial nint GetJoystickGUID(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickVendor")]
    public static partial ushort GetJoystickVendor(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProduct")]
    public static partial ushort GetJoystickProduct(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProductVersion")]
    public static partial ushort GetJoystickProductVersion(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickFirmwareVersion")]
    public static partial ushort GetJoystickFirmwareVersion(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickSerial")]
    public static partial nint GetJoystickSerial(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickType")]
    public static partial SdlJoystickType GetJoystickType(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickGUIDInfo")]
    public static partial void GetJoystickGUIDInfo(nint guid, nint vendor, nint product, nint version, nint crc16);

    [LibraryImport(Lib, EntryPoint = "SDL_JoystickConnected")]
    public static partial CBool JoystickConnected(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickID")]
    public static partial uint GetJoystickID(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickAxes")]
    public static partial int GetNumJoystickAxes(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickBalls")]
    public static partial int GetNumJoystickBalls(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickHats")]
    public static partial int GetNumJoystickHats(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickButtons")]
    public static partial int GetNumJoystickButtons(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickEventsEnabled")]
    public static partial void SetJoystickEventsEnabled(CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_JoystickEventsEnabled")]
    public static partial CBool JoystickEventsEnabled();

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateJoysticks")]
    public static partial void UpdateJoysticks();

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickAxis")]
    public static partial short GetJoystickAxis(nint joystick, int axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickAxisInitialState")]
    public static partial CBool GetJoystickAxisInitialState(nint joystick, int axis, nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickBall")]
    public static partial CBool GetJoystickBall(nint joystick, int ball, nint dx, nint dy);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickHat")]
    public static partial byte GetJoystickHat(nint joystick, int hat);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickButton")]
    public static partial CBool GetJoystickButton(nint joystick, int button);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleJoystick")]
    public static partial CBool RumbleJoystick(nint joystick, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleJoystickTriggers")]
    public static partial CBool RumbleJoystickTriggers(nint joystick, ushort left_rumble, ushort right_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickLED")]
    public static partial CBool SetJoystickLED(nint joystick, byte red, byte green, byte blue);

    [LibraryImport(Lib, EntryPoint = "SDL_SendJoystickEffect")]
    public static partial CBool SendJoystickEffect(nint joystick, nint data, int size);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseJoystick")]
    public static partial void CloseJoystick(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickConnectionState")]
    public static partial SdlJoystickConnectionState GetJoystickConnectionState(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPowerInfo")]
    public static partial SdlPowerState GetJoystickPowerInfo(nint joystick, nint percent);

    [LibraryImport(Lib, EntryPoint = "SDL_AddGamepadMapping")]
    public static partial int AddGamepadMapping(nint mapping);

    [LibraryImport(Lib, EntryPoint = "SDL_AddGamepadMappingsFromIO")]
    public static partial int AddGamepadMappingsFromIO(nint src, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_AddGamepadMappingsFromFile")]
    public static partial int AddGamepadMappingsFromFile(nint file);

    [LibraryImport(Lib, EntryPoint = "SDL_ReloadGamepadMappings")]
    public static partial CBool ReloadGamepadMappings();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMappings")]
    public static partial nint GetGamepadMappings(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMappingForGUID")]
    public static partial nint GetGamepadMappingForGUID(nint guid);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMapping")]
    public static partial nint GetGamepadMapping(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadMapping")]
    public static partial CBool SetGamepadMapping(uint instance_id, nint mapping);

    [LibraryImport(Lib, EntryPoint = "SDL_HasGamepad")]
    public static partial CBool HasGamepad();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepads")]
    public static partial nint GetGamepads(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_IsGamepad")]
    public static partial CBool IsGamepad(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadNameForID")]
    public static partial nint GetGamepadNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPathForID")]
    public static partial nint GetGamepadPathForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPlayerIndexForID")]
    public static partial int GetGamepadPlayerIndexForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadGUIDForID")]
    public static partial nint GetGamepadGUIDForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadVendorForID")]
    public static partial ushort GetGamepadVendorForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProductForID")]
    public static partial ushort GetGamepadProductForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProductVersionForID")]
    public static partial ushort GetGamepadProductVersionForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadTypeForID")]
    public static partial SdlGamepadType GetGamepadTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRealGamepadTypeForID")]
    public static partial SdlGamepadType GetRealGamepadTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMappingForID")]
    public static partial nint GetGamepadMappingForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenGamepad")]
    public static partial nint OpenGamepad(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadFromID")]
    public static partial nint GetGamepadFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadFromPlayerIndex")]
    public static partial nint GetGamepadFromPlayerIndex(int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProperties")]
    public static partial uint GetGamepadProperties(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadID")]
    public static partial uint GetGamepadID(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadName")]
    public static partial nint GetGamepadName(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPath")]
    public static partial nint GetGamepadPath(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadType")]
    public static partial SdlGamepadType GetGamepadType(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRealGamepadType")]
    public static partial SdlGamepadType GetRealGamepadType(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPlayerIndex")]
    public static partial int GetGamepadPlayerIndex(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadPlayerIndex")]
    public static partial CBool SetGamepadPlayerIndex(nint gamepad, int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadVendor")]
    public static partial ushort GetGamepadVendor(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProduct")]
    public static partial ushort GetGamepadProduct(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProductVersion")]
    public static partial ushort GetGamepadProductVersion(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadFirmwareVersion")]
    public static partial ushort GetGamepadFirmwareVersion(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSerial")]
    public static partial nint GetGamepadSerial(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSteamHandle")]
    public static partial ulong GetGamepadSteamHandle(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadConnectionState")]
    public static partial SdlJoystickConnectionState GetGamepadConnectionState(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPowerInfo")]
    public static partial SdlPowerState GetGamepadPowerInfo(nint gamepad, nint percent);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadConnected")]
    public static partial CBool GamepadConnected(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadJoystick")]
    public static partial nint GetGamepadJoystick(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadEventsEnabled")]
    public static partial void SetGamepadEventsEnabled(CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadEventsEnabled")]
    public static partial CBool GamepadEventsEnabled();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadBindings")]
    public static partial nint GetGamepadBindings(nint gamepad, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateGamepads")]
    public static partial void UpdateGamepads();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadTypeFromString")]
    public static partial SdlGamepadType GetGamepadTypeFromString(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadStringForType")]
    public static partial nint GetGamepadStringForType(SdlGamepadType type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAxisFromString")]
    public static partial SdlGamepadAxis GetGamepadAxisFromString(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadStringForAxis")]
    public static partial nint GetGamepadStringForAxis(SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadHasAxis")]
    public static partial CBool GamepadHasAxis(nint gamepad, SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAxis")]
    public static partial short GetGamepadAxis(nint gamepad, SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButtonFromString")]
    public static partial SdlGamepadButton GetGamepadButtonFromString(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadStringForButton")]
    public static partial nint GetGamepadStringForButton(SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadHasButton")]
    public static partial CBool GamepadHasButton(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButton")]
    public static partial CBool GetGamepadButton(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButtonLabelForType")]
    public static partial SdlGamepadButtonLabel GetGamepadButtonLabelForType(SdlGamepadType type, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButtonLabel")]
    public static partial SdlGamepadButtonLabel GetGamepadButtonLabel(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumGamepadTouchpads")]
    public static partial int GetNumGamepadTouchpads(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumGamepadTouchpadFingers")]
    public static partial int GetNumGamepadTouchpadFingers(nint gamepad, int touchpad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadTouchpadFinger")]
    public static partial CBool GetGamepadTouchpadFinger(nint gamepad, int touchpad, int finger, nint down, nint x, nint y, nint pressure);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadHasSensor")]
    public static partial CBool GamepadHasSensor(nint gamepad, SdlSensorType type);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadSensorEnabled")]
    public static partial CBool SetGamepadSensorEnabled(nint gamepad, SdlSensorType type, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadSensorEnabled")]
    public static partial CBool GamepadSensorEnabled(nint gamepad, SdlSensorType type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSensorDataRate")]
    public static partial float GetGamepadSensorDataRate(nint gamepad, SdlSensorType type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSensorData")]
    public static partial CBool GetGamepadSensorData(nint gamepad, SdlSensorType type, nint data, int num_values);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleGamepad")]
    public static partial CBool RumbleGamepad(nint gamepad, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleGamepadTriggers")]
    public static partial CBool RumbleGamepadTriggers(nint gamepad, ushort left_rumble, ushort right_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadLED")]
    public static partial CBool SetGamepadLED(nint gamepad, byte red, byte green, byte blue);

    [LibraryImport(Lib, EntryPoint = "SDL_SendGamepadEffect")]
    public static partial CBool SendGamepadEffect(nint gamepad, nint data, int size);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseGamepad")]
    public static partial void CloseGamepad(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAppleSFSymbolsNameForButton")]
    public static partial nint GetGamepadAppleSFSymbolsNameForButton(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAppleSFSymbolsNameForAxis")]
    public static partial nint GetGamepadAppleSFSymbolsNameForAxis(nint gamepad, SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_HasKeyboard")]
    public static partial CBool HasKeyboard();

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboards")]
    public static partial nint GetKeyboards(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboardNameForID")]
    public static partial nint GetKeyboardNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboardFocus")]
    public static partial nint GetKeyboardFocus();

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboardState")]
    public static partial nint GetKeyboardState(nint numkeys);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetKeyboard")]
    public static partial void ResetKeyboard();

    [LibraryImport(Lib, EntryPoint = "SDL_GetModState")]
    public static partial ushort GetModState();

    [LibraryImport(Lib, EntryPoint = "SDL_SetModState")]
    public static partial void SetModState(ushort modstate);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyFromScancode")]
    public static partial uint GetKeyFromScancode(SdlScancode scancode, ushort modstate, CBool key_event);

    [LibraryImport(Lib, EntryPoint = "SDL_GetScancodeFromKey")]
    public static partial SdlScancode GetScancodeFromKey(uint key, nint modstate);

    [LibraryImport(Lib, EntryPoint = "SDL_SetScancodeName")]
    public static partial CBool SetScancodeName(SdlScancode scancode, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetScancodeName")]
    public static partial nint GetScancodeName(SdlScancode scancode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetScancodeFromName")]
    public static partial SdlScancode GetScancodeFromName(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyName")]
    public static partial nint GetKeyName(uint key);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyFromName")]
    public static partial uint GetKeyFromName(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_StartTextInput")]
    public static partial CBool StartTextInput(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_StartTextInputWithProperties")]
    public static partial CBool StartTextInputWithProperties(nint window, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_TextInputActive")]
    public static partial CBool TextInputActive(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_StopTextInput")]
    public static partial CBool StopTextInput(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearComposition")]
    public static partial CBool ClearComposition(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextInputArea")]
    public static partial CBool SetTextInputArea(nint window, nint rect, int cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextInputArea")]
    public static partial CBool GetTextInputArea(nint window, nint rect, nint cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_HasScreenKeyboardSupport")]
    public static partial CBool HasScreenKeyboardSupport();

    [LibraryImport(Lib, EntryPoint = "SDL_ScreenKeyboardShown")]
    public static partial CBool ScreenKeyboardShown(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_HasMouse")]
    public static partial CBool HasMouse();

    [LibraryImport(Lib, EntryPoint = "SDL_GetMice")]
    public static partial nint GetMice(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMouseNameForID")]
    public static partial nint GetMouseNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMouseFocus")]
    public static partial nint GetMouseFocus();

    [LibraryImport(Lib, EntryPoint = "SDL_GetMouseState")]
    public static partial uint GetMouseState(nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGlobalMouseState")]
    public static partial uint GetGlobalMouseState(nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRelativeMouseState")]
    public static partial uint GetRelativeMouseState(nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_WarpMouseInWindow")]
    public static partial void WarpMouseInWindow(nint window, float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_WarpMouseGlobal")]
    public static partial CBool WarpMouseGlobal(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowRelativeMouseMode")]
    public static partial CBool SetWindowRelativeMouseMode(nint window, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowRelativeMouseMode")]
    public static partial CBool GetWindowRelativeMouseMode(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_CaptureMouse")]
    public static partial CBool CaptureMouse(CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateCursor")]
    public static partial nint CreateCursor(nint data, nint mask, int w, int h, int hot_x, int hot_y);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateColorCursor")]
    public static partial nint CreateColorCursor(nint surface, int hot_x, int hot_y);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSystemCursor")]
    public static partial nint CreateSystemCursor(SdlSystemCursor id);

    [LibraryImport(Lib, EntryPoint = "SDL_SetCursor")]
    public static partial CBool SetCursor(nint cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCursor")]
    public static partial nint GetCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDefaultCursor")]
    public static partial nint GetDefaultCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyCursor")]
    public static partial void DestroyCursor(nint cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowCursor")]
    public static partial CBool ShowCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_HideCursor")]
    public static partial CBool HideCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_CursorVisible")]
    public static partial CBool CursorVisible();

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchDevices")]
    public static partial nint GetTouchDevices(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchDeviceName")]
    public static partial nint GetTouchDeviceName(ulong touchID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchDeviceType")]
    public static partial SdlTouchDeviceType GetTouchDeviceType(ulong touchID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchFingers")]
    public static partial nint GetTouchFingers(ulong touchID, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_PumpEvents")]
    public static partial void PumpEvents();

    [LibraryImport(Lib, EntryPoint = "SDL_PeepEvents")]
    public static partial int PeepEvents(nint events, int numevents, SdlEventAction action, uint minType, uint maxType);

    [LibraryImport(Lib, EntryPoint = "SDL_HasEvent")]
    public static partial CBool HasEvent(uint type);

    [LibraryImport(Lib, EntryPoint = "SDL_HasEvents")]
    public static partial CBool HasEvents(uint minType, uint maxType);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushEvent")]
    public static partial void FlushEvent(uint type);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushEvents")]
    public static partial void FlushEvents(uint minType, uint maxType);

    [LibraryImport(Lib, EntryPoint = "SDL_PollEvent")]
    public static partial CBool PollEvent(out SdlEvent @event);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitEvent")]
    public static partial CBool WaitEvent(nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitEventTimeout")]
    public static partial CBool WaitEventTimeout(nint @event, int timeoutMS);

    [LibraryImport(Lib, EntryPoint = "SDL_PushEvent")]
    public static partial CBool PushEvent(nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_SetEventFilter")]
    public static partial void SetEventFilter(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GetEventFilter")]
    public static partial CBool GetEventFilter(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_AddEventWatch")]
    public static partial CBool AddEventWatch(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveEventWatch")]
    public static partial void RemoveEventWatch(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_FilterEvents")]
    public static partial void FilterEvents(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetEventEnabled")]
    public static partial void SetEventEnabled(uint type, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_EventEnabled")]
    public static partial CBool EventEnabled(uint type);

    [LibraryImport(Lib, EntryPoint = "SDL_RegisterEvents")]
    public static partial uint RegisterEvents(int numevents);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFromEvent")]
    public static partial nint GetWindowFromEvent(nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_GetBasePath")]
    public static partial nint GetBasePath();

    [LibraryImport(Lib, EntryPoint = "SDL_GetPrefPath")]
    public static partial nint GetPrefPath(nint org, nint app);

    [LibraryImport(Lib, EntryPoint = "SDL_GetUserFolder")]
    public static partial nint GetUserFolder(SdlFolder folder);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateDirectory")]
    public static partial CBool CreateDirectory(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_EnumerateDirectory")]
    public static partial CBool EnumerateDirectory(nint path, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemovePath")]
    public static partial CBool RemovePath(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_RenamePath")]
    public static partial CBool RenamePath(nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyFile")]
    public static partial CBool CopyFile(nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPathInfo")]
    public static partial CBool GetPathInfo(nint path, nint info);

    [LibraryImport(Lib, EntryPoint = "SDL_GlobDirectory")]
    public static partial nint GlobDirectory(nint path, nint pattern, uint flags, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUSupportsShaderFormats")]
    public static partial CBool GpuSupportsShaderFormats(uint format_flags, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUSupportsProperties")]
    public static partial CBool GpuSupportsProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUDevice")]
    public static partial nint CreateGpuDevice(uint format_flags, CBool debug_mode, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUDeviceWithProperties")]
    public static partial nint CreateGpuDeviceWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyGPUDevice")]
    public static partial void DestroyGpuDevice(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumGPUDrivers")]
    public static partial int GetNumGpuDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUDriver")]
    public static partial nint GetGpuDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUDeviceDriver")]
    public static partial nint GetGpuDeviceDriver(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUShaderFormats")]
    public static partial uint GetGpuShaderFormats(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUComputePipeline")]
    public static partial nint CreateGpuComputePipeline(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUGraphicsPipeline")]
    public static partial nint CreateGpuGraphicsPipeline(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUSampler")]
    public static partial nint CreateGpuSampler(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUShader")]
    public static partial nint CreateGpuShader(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUTexture")]
    public static partial nint CreateGpuTexture(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUBuffer")]
    public static partial nint CreateGpuBuffer(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUTransferBuffer")]
    public static partial nint CreateGpuTransferBuffer(nint device, nint createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUBufferName")]
    public static partial void SetGpuBufferName(nint device, nint buffer, nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUTextureName")]
    public static partial void SetGpuTextureName(nint device, nint texture, nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_InsertGPUDebugLabel")]
    public static partial void InsertGpuDebugLabel(nint command_buffer, nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUDebugGroup")]
    public static partial void PushGpuDebugGroup(nint command_buffer, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_PopGPUDebugGroup")]
    public static partial void PopGpuDebugGroup(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUTexture")]
    public static partial void ReleaseGpuTexture(nint device, nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUSampler")]
    public static partial void ReleaseGpuSampler(nint device, nint sampler);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUBuffer")]
    public static partial void ReleaseGpuBuffer(nint device, nint buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUTransferBuffer")]
    public static partial void ReleaseGpuTransferBuffer(nint device, nint transfer_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUComputePipeline")]
    public static partial void ReleaseGpuComputePipeline(nint device, nint compute_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUShader")]
    public static partial void ReleaseGpuShader(nint device, nint shader);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUGraphicsPipeline")]
    public static partial void ReleaseGpuGraphicsPipeline(nint device, nint graphics_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_AcquireGPUCommandBuffer")]
    public static partial nint AcquireGpuCommandBuffer(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUVertexUniformData")]
    public static partial void PushGpuVertexUniformData(nint command_buffer, uint slot_index, nint data, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUFragmentUniformData")]
    public static partial void PushGpuFragmentUniformData(nint command_buffer, uint slot_index, nint data, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUComputeUniformData")]
    public static partial void PushGpuComputeUniformData(nint command_buffer, uint slot_index, nint data, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_BeginGPURenderPass")]
    public static partial nint BeginGpuRenderPass(nint command_buffer, nint color_target_infos, uint num_color_targets, nint depth_stencil_target_info);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUGraphicsPipeline")]
    public static partial void BindGpuGraphicsPipeline(nint render_pass, nint graphics_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUViewport")]
    public static partial void SetGpuViewport(nint render_pass, nint viewport);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUScissor")]
    public static partial void SetGpuScissor(nint render_pass, nint scissor);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUBlendConstants")]
    public static partial void SetGpuBlendConstants(nint render_pass, nint blend_constants);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUStencilReference")]
    public static partial void SetGpuStencilReference(nint render_pass, byte reference);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexBuffers")]
    public static partial void BindGpuVertexBuffers(nint render_pass, uint first_slot, nint bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUIndexBuffer")]
    public static partial void BindGpuIndexBuffer(nint render_pass, nint binding, SdlGpuIndexElementSize index_element_size);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexSamplers")]
    public static partial void BindGpuVertexSamplers(nint render_pass, uint first_slot, nint texture_sampler_bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexStorageTextures")]
    public static partial void BindGpuVertexStorageTextures(nint render_pass, uint first_slot, nint storage_textures, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexStorageBuffers")]
    public static partial void BindGpuVertexStorageBuffers(nint render_pass, uint first_slot, nint storage_buffers, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUFragmentSamplers")]
    public static partial void BindGpuFragmentSamplers(nint render_pass, uint first_slot, nint texture_sampler_bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUFragmentStorageTextures")]
    public static partial void BindGpuFragmentStorageTextures(nint render_pass, uint first_slot, nint storage_textures, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUFragmentStorageBuffers")]
    public static partial void BindGpuFragmentStorageBuffers(nint render_pass, uint first_slot, nint storage_buffers, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUIndexedPrimitives")]
    public static partial void DrawGpuIndexedPrimitives(nint render_pass, uint num_indices, uint num_instances, uint first_index, int vertex_offset, uint first_instance);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUPrimitives")]
    public static partial void DrawGpuPrimitives(nint render_pass, uint num_vertices, uint num_instances, uint first_vertex, uint first_instance);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUPrimitivesIndirect")]
    public static partial void DrawGpuPrimitivesIndirect(nint render_pass, nint buffer, uint offset, uint draw_count);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUIndexedPrimitivesIndirect")]
    public static partial void DrawGpuIndexedPrimitivesIndirect(nint render_pass, nint buffer, uint offset, uint draw_count);

    [LibraryImport(Lib, EntryPoint = "SDL_EndGPURenderPass")]
    public static partial void EndGpuRenderPass(nint render_pass);

    [LibraryImport(Lib, EntryPoint = "SDL_BeginGPUComputePass")]
    public static partial nint BeginGpuComputePass(nint command_buffer, nint storage_texture_bindings, uint num_storage_texture_bindings, nint storage_buffer_bindings, uint num_storage_buffer_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputePipeline")]
    public static partial void BindGpuComputePipeline(nint compute_pass, nint compute_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputeSamplers")]
    public static partial void BindGpuComputeSamplers(nint compute_pass, uint first_slot, nint texture_sampler_bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputeStorageTextures")]
    public static partial void BindGpuComputeStorageTextures(nint compute_pass, uint first_slot, nint storage_textures, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputeStorageBuffers")]
    public static partial void BindGpuComputeStorageBuffers(nint compute_pass, uint first_slot, nint storage_buffers, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_DispatchGPUCompute")]
    public static partial void DispatchGpuCompute(nint compute_pass, uint groupcount_x, uint groupcount_y, uint groupcount_z);

    [LibraryImport(Lib, EntryPoint = "SDL_DispatchGPUComputeIndirect")]
    public static partial void DispatchGpuComputeIndirect(nint compute_pass, nint buffer, uint offset);

    [LibraryImport(Lib, EntryPoint = "SDL_EndGPUComputePass")]
    public static partial void EndGpuComputePass(nint compute_pass);

    [LibraryImport(Lib, EntryPoint = "SDL_MapGPUTransferBuffer")]
    public static partial nint MapGpuTransferBuffer(nint device, nint transfer_buffer, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_UnmapGPUTransferBuffer")]
    public static partial void UnmapGpuTransferBuffer(nint device, nint transfer_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_BeginGPUCopyPass")]
    public static partial nint BeginGpuCopyPass(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_UploadToGPUTexture")]
    public static partial void UploadToGpuTexture(nint copy_pass, nint source, nint destination, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_UploadToGPUBuffer")]
    public static partial void UploadToGpuBuffer(nint copy_pass, nint source, nint destination, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyGPUTextureToTexture")]
    public static partial void CopyGpuTextureToTexture(nint copy_pass, nint source, nint destination, uint w, uint h, uint d, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyGPUBufferToBuffer")]
    public static partial void CopyGpuBufferToBuffer(nint copy_pass, nint source, nint destination, uint size, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_DownloadFromGPUTexture")]
    public static partial void DownloadFromGpuTexture(nint copy_pass, nint source, nint destination);

    [LibraryImport(Lib, EntryPoint = "SDL_DownloadFromGPUBuffer")]
    public static partial void DownloadFromGpuBuffer(nint copy_pass, nint source, nint destination);

    [LibraryImport(Lib, EntryPoint = "SDL_EndGPUCopyPass")]
    public static partial void EndGpuCopyPass(nint copy_pass);

    [LibraryImport(Lib, EntryPoint = "SDL_GenerateMipmapsForGPUTexture")]
    public static partial void GenerateMipmapsForGpuTexture(nint command_buffer, nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitGPUTexture")]
    public static partial void BlitGpuTexture(nint command_buffer, nint info);

    [LibraryImport(Lib, EntryPoint = "SDL_WindowSupportsGPUSwapchainComposition")]
    public static partial CBool WindowSupportsGpuSwapchainComposition(nint device, nint window, SdlGpuSwapchainComposition swapchain_composition);

    [LibraryImport(Lib, EntryPoint = "SDL_WindowSupportsGPUPresentMode")]
    public static partial CBool WindowSupportsGpuPresentMode(nint device, nint window, SdlGpuPresentMode present_mode);

    [LibraryImport(Lib, EntryPoint = "SDL_ClaimWindowForGPUDevice")]
    public static partial CBool ClaimWindowForGpuDevice(nint device, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseWindowFromGPUDevice")]
    public static partial void ReleaseWindowFromGpuDevice(nint device, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUSwapchainParameters")]
    public static partial CBool SetGpuSwapchainParameters(nint device, nint window, SdlGpuSwapchainComposition swapchain_composition, SdlGpuPresentMode present_mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUSwapchainTextureFormat")]
    public static partial SdlGpuTextureFormat GetGpuSwapchainTextureFormat(nint device, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_AcquireGPUSwapchainTexture")]
    public static partial CBool AcquireGpuSwapchainTexture(nint command_buffer, nint window, nint swapchain_texture, nint swapchain_texture_width, nint swapchain_texture_height);

    [LibraryImport(Lib, EntryPoint = "SDL_SubmitGPUCommandBuffer")]
    public static partial CBool SubmitGpuCommandBuffer(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_SubmitGPUCommandBufferAndAcquireFence")]
    public static partial nint SubmitGpuCommandBufferAndAcquireFence(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitForGPUIdle")]
    public static partial CBool WaitForGpuIdle(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitForGPUFences")]
    public static partial CBool WaitForGpuFences(nint device, CBool wait_all, nint fences, uint num_fences);

    [LibraryImport(Lib, EntryPoint = "SDL_QueryGPUFence")]
    public static partial CBool QueryGpuFence(nint device, nint fence);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUFence")]
    public static partial void ReleaseGpuFence(nint device, nint fence);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUTextureFormatTexelBlockSize")]
    public static partial uint GpuTextureFormatTexelBlockSize(SdlGpuTextureFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUTextureSupportsFormat")]
    public static partial CBool GpuTextureSupportsFormat(nint device, SdlGpuTextureFormat format, SdlGpuTextureType type, uint usage);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUTextureSupportsSampleCount")]
    public static partial CBool GpuTextureSupportsSampleCount(nint device, SdlGpuTextureFormat format, SdlGpuSampleCount sample_count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHaptics")]
    public static partial nint GetHaptics(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticNameForID")]
    public static partial nint GetHapticNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenHaptic")]
    public static partial nint OpenHaptic(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticFromID")]
    public static partial nint GetHapticFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticID")]
    public static partial uint GetHapticID(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticName")]
    public static partial nint GetHapticName(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_IsMouseHaptic")]
    public static partial CBool IsMouseHaptic();

    [LibraryImport(Lib, EntryPoint = "SDL_OpenHapticFromMouse")]
    public static partial nint OpenHapticFromMouse();

    [LibraryImport(Lib, EntryPoint = "SDL_IsJoystickHaptic")]
    public static partial CBool IsJoystickHaptic(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenHapticFromJoystick")]
    public static partial nint OpenHapticFromJoystick(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseHaptic")]
    public static partial void CloseHaptic(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMaxHapticEffects")]
    public static partial int GetMaxHapticEffects(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMaxHapticEffectsPlaying")]
    public static partial int GetMaxHapticEffectsPlaying(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticFeatures")]
    public static partial uint GetHapticFeatures(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumHapticAxes")]
    public static partial int GetNumHapticAxes(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_HapticEffectSupported")]
    public static partial CBool HapticEffectSupported(nint haptic, nint effect);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateHapticEffect")]
    public static partial int CreateHapticEffect(nint haptic, nint effect);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateHapticEffect")]
    public static partial CBool UpdateHapticEffect(nint haptic, int effect, nint data);

    [LibraryImport(Lib, EntryPoint = "SDL_RunHapticEffect")]
    public static partial CBool RunHapticEffect(nint haptic, int effect, uint iterations);

    [LibraryImport(Lib, EntryPoint = "SDL_StopHapticEffect")]
    public static partial CBool StopHapticEffect(nint haptic, int effect);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyHapticEffect")]
    public static partial void DestroyHapticEffect(nint haptic, int effect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticEffectStatus")]
    public static partial CBool GetHapticEffectStatus(nint haptic, int effect);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHapticGain")]
    public static partial CBool SetHapticGain(nint haptic, int gain);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHapticAutocenter")]
    public static partial CBool SetHapticAutocenter(nint haptic, int autocenter);

    [LibraryImport(Lib, EntryPoint = "SDL_PauseHaptic")]
    public static partial CBool PauseHaptic(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_ResumeHaptic")]
    public static partial CBool ResumeHaptic(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_StopHapticEffects")]
    public static partial CBool StopHapticEffects(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_HapticRumbleSupported")]
    public static partial CBool HapticRumbleSupported(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_InitHapticRumble")]
    public static partial CBool InitHapticRumble(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_PlayHapticRumble")]
    public static partial CBool PlayHapticRumble(nint haptic, float strength, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_StopHapticRumble")]
    public static partial CBool StopHapticRumble(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_init")]
    public static partial int HidInit();

    [LibraryImport(Lib, EntryPoint = "SDL_hid_exit")]
    public static partial int HidExit();

    [LibraryImport(Lib, EntryPoint = "SDL_hid_device_change_count")]
    public static partial uint HidDeviceChangeCount();

    [LibraryImport(Lib, EntryPoint = "SDL_hid_enumerate")]
    public static partial nint HidEnumerate(ushort vendor_id, ushort product_id);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_free_enumeration")]
    public static partial void HidFreeEnumeration(nint devs);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_open")]
    public static partial nint HidOpen(ushort vendor_id, ushort product_id, nint serial_number);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_open_path")]
    public static partial nint HidOpenPath(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_write")]
    public static partial int HidWrite(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_read_timeout")]
    public static partial int HidReadTimeout(nint dev, nint data, nuint length, int milliseconds);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_read")]
    public static partial int HidRead(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_set_nonblocking")]
    public static partial int HidSetNonblocking(nint dev, int nonblock);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_send_feature_report")]
    public static partial int HidSendFeatureReport(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_feature_report")]
    public static partial int HidGetFeatureReport(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_input_report")]
    public static partial int HidGetInputReport(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_close")]
    public static partial int HidClose(nint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_manufacturer_string")]
    public static partial int HidGetManufacturerString(nint dev, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_product_string")]
    public static partial int HidGetProductString(nint dev, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_serial_number_string")]
    public static partial int HidGetSerialNumberString(nint dev, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_indexed_string")]
    public static partial int HidGetIndexedString(nint dev, int string_index, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_device_info")]
    public static partial nint HidGetDeviceInfo(nint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_report_descriptor")]
    public static partial int HidGetReportDescriptor(nint dev, nint buf, nuint buf_size);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_ble_scan")]
    public static partial void HidBleScan(CBool active);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHintWithPriority")]
    public static partial CBool SetHintWithPriority(nint name, nint value, SdlHintPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHint")]
    public static partial CBool SetHint(nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetHint")]
    public static partial CBool ResetHint(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetHints")]
    public static partial void ResetHints();

    [LibraryImport(Lib, EntryPoint = "SDL_GetHint")]
    public static partial nint GetHint(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHintBoolean")]
    public static partial CBool GetHintBoolean(nint name, CBool default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_AddHintCallback")]
    public static partial CBool AddHintCallback(nint name, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveHintCallback")]
    public static partial void RemoveHintCallback(nint name, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_Init")]
    public static partial CBool Init(SdlInit flags);

    [LibraryImport(Lib, EntryPoint = "SDL_InitSubSystem")]
    public static partial CBool InitSubSystem(uint flags);

    [LibraryImport(Lib, EntryPoint = "SDL_QuitSubSystem")]
    public static partial void QuitSubSystem(uint flags);

    [LibraryImport(Lib, EntryPoint = "SDL_WasInit")]
    public static partial uint WasInit(uint flags);

    [LibraryImport(Lib, EntryPoint = "SDL_Quit")]
    public static partial void Quit();

    [LibraryImport(Lib, EntryPoint = "SDL_SetAppMetadata")]
    public static partial CBool SetAppMetadata(nint appname, nint appversion, nint appidentifier);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAppMetadataProperty")]
    public static partial CBool SetAppMetadataProperty(nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAppMetadataProperty")]
    public static partial nint GetAppMetadataProperty(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadObject")]
    public static partial nint LoadObject(nint sofile);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadFunction")]
    public static partial nint LoadFunction(nint handle, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_UnloadObject")]
    public static partial void UnloadObject(nint handle);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPreferredLocales")]
    public static partial nint GetPreferredLocales(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogPriorities")]
    public static partial void SetLogPriorities(SdlLogPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogPriority")]
    public static partial void SetLogPriority(int category, SdlLogPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_GetLogPriority")]
    public static partial SdlLogPriority GetLogPriority(int category);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetLogPriorities")]
    public static partial void ResetLogPriorities();

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogPriorityPrefix")]
    public static partial CBool SetLogPriorityPrefix(SdlLogPriority priority, nint prefix);

    [LibraryImport(Lib, EntryPoint = "SDL_Log")]
    public static partial void Log(nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogTrace")]
    public static partial void LogTrace(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogVerbose")]
    public static partial void LogVerbose(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogDebug")]
    public static partial void LogDebug(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogInfo")]
    public static partial void LogInfo(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogWarn")]
    public static partial void LogWarn(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogError")]
    public static partial void LogError(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogCritical")]
    public static partial void LogCritical(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogMessage")]
    public static partial void LogMessage(int category, SdlLogPriority priority, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_GetLogOutputFunction")]
    public static partial void GetLogOutputFunction(nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogOutputFunction")]
    public static partial void SetLogOutputFunction(nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowMessageBox")]
    public static partial CBool ShowMessageBox(nint messageboxdata, nint buttonid);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowSimpleMessageBox")]
    public static partial CBool ShowSimpleMessageBox(uint flags, nint title, nint message, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_Metal_CreateView")]
    public static partial nint MetalCreateView(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_Metal_DestroyView")]
    public static partial void MetalDestroyView(nint view);

    [LibraryImport(Lib, EntryPoint = "SDL_Metal_GetLayer")]
    public static partial nint MetalGetLayer(nint view);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenURL")]
    public static partial CBool OpenURL(nint url);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPlatform")]
    public static partial nint GetPlatform();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateProcess")]
    public static partial nint CreateProcess(nint args, CBool pipe_stdio);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateProcessWithProperties")]
    public static partial nint CreateProcessWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_GetProcessProperties")]
    public static partial uint GetProcessProperties(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadProcess")]
    public static partial nint ReadProcess(nint process, nint datasize, nint exitcode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetProcessInput")]
    public static partial nint GetProcessInput(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_GetProcessOutput")]
    public static partial nint GetProcessOutput(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_KillProcess")]
    public static partial CBool KillProcess(nint process, CBool force);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitProcess")]
    public static partial CBool WaitProcess(nint process, CBool block, nint exitcode);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyProcess")]
    public static partial void DestroyProcess(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumRenderDrivers")]
    public static partial int GetNumRenderDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDriver")]
    public static partial nint GetRenderDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateWindowAndRenderer")]
    public static partial CBool CreateWindowAndRenderer(nint title, int width, int height, ulong window_flags, nint window, nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateRenderer")]
    public static partial nint CreateRenderer(nint window, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateRendererWithProperties")]
    public static partial nint CreateRendererWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSoftwareRenderer")]
    public static partial nint CreateSoftwareRenderer(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderer")]
    public static partial nint GetRenderer(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderWindow")]
    public static partial nint GetRenderWindow(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRendererName")]
    public static partial nint GetRendererName(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRendererProperties")]
    public static partial uint GetRendererProperties(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderOutputSize")]
    public static partial CBool GetRenderOutputSize(nint renderer, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentRenderOutputSize")]
    public static partial CBool GetCurrentRenderOutputSize(nint renderer, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateTexture")]
    public static partial nint CreateTexture(nint renderer, SdlPixelFormat format, SdlTextureAccess access, int w, int h);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateTextureFromSurface")]
    public static partial nint CreateTextureFromSurface(nint renderer, nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateTextureWithProperties")]
    public static partial nint CreateTextureWithProperties(nint renderer, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureProperties")]
    public static partial uint GetTextureProperties(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRendererFromTexture")]
    public static partial nint GetRendererFromTexture(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureSize")]
    public static partial CBool GetTextureSize(nint texture, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureColorMod")]
    public static partial CBool SetTextureColorMod(nint texture, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureColorModFloat")]
    public static partial CBool SetTextureColorModFloat(nint texture, float r, float g, float b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureColorMod")]
    public static partial CBool GetTextureColorMod(nint texture, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureColorModFloat")]
    public static partial CBool GetTextureColorModFloat(nint texture, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureAlphaMod")]
    public static partial CBool SetTextureAlphaMod(nint texture, byte alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureAlphaModFloat")]
    public static partial CBool SetTextureAlphaModFloat(nint texture, float alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureAlphaMod")]
    public static partial CBool GetTextureAlphaMod(nint texture, nint alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureAlphaModFloat")]
    public static partial CBool GetTextureAlphaModFloat(nint texture, nint alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureBlendMode")]
    public static partial CBool SetTextureBlendMode(nint texture, uint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureBlendMode")]
    public static partial CBool GetTextureBlendMode(nint texture, nint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureScaleMode")]
    public static partial CBool SetTextureScaleMode(nint texture, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureScaleMode")]
    public static partial CBool GetTextureScaleMode(nint texture, nint scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateTexture")]
    public static partial CBool UpdateTexture(nint texture, nint rect, nint pixels, int pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateYUVTexture")]
    public static partial CBool UpdateYUVTexture(nint texture, nint rect, nint Yplane, int Ypitch, nint Uplane, int Upitch, nint Vplane, int Vpitch);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateNVTexture")]
    public static partial CBool UpdateNVTexture(nint texture, nint rect, nint Yplane, int Ypitch, nint UVplane, int UVpitch);

    [LibraryImport(Lib, EntryPoint = "SDL_LockTexture")]
    public static partial CBool LockTexture(nint texture, nint rect, nint pixels, nint pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_LockTextureToSurface")]
    public static partial CBool LockTextureToSurface(nint texture, nint rect, nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockTexture")]
    public static partial void UnlockTexture(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderTarget")]
    public static partial CBool SetRenderTarget(nint renderer, nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderTarget")]
    public static partial nint GetRenderTarget(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderLogicalPresentation")]
    public static partial CBool SetRenderLogicalPresentation(nint renderer, int w, int h, SdlRendererLogicalPresentation mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderLogicalPresentation")]
    public static partial CBool GetRenderLogicalPresentation(nint renderer, nint w, nint h, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderLogicalPresentationRect")]
    public static partial CBool GetRenderLogicalPresentationRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderCoordinatesFromWindow")]
    public static partial CBool RenderCoordinatesFromWindow(nint renderer, float window_x, float window_y, nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderCoordinatesToWindow")]
    public static partial CBool RenderCoordinatesToWindow(nint renderer, float x, float y, nint window_x, nint window_y);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertEventToRenderCoordinates")]
    public static partial CBool ConvertEventToRenderCoordinates(nint renderer, nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderViewport")]
    public static partial CBool SetRenderViewport(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderViewport")]
    public static partial CBool GetRenderViewport(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderViewportSet")]
    public static partial CBool RenderViewportSet(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderSafeArea")]
    public static partial CBool GetRenderSafeArea(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderClipRect")]
    public static partial CBool SetRenderClipRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderClipRect")]
    public static partial CBool GetRenderClipRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderClipEnabled")]
    public static partial CBool RenderClipEnabled(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderScale")]
    public static partial CBool SetRenderScale(nint renderer, float scaleX, float scaleY);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderScale")]
    public static partial CBool GetRenderScale(nint renderer, nint scaleX, nint scaleY);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderDrawColor")]
    public static partial CBool SetRenderDrawColor(nint renderer, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderDrawColorFloat")]
    public static partial CBool SetRenderDrawColorFloat(nint renderer, float r, float g, float b, float a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDrawColor")]
    public static partial CBool GetRenderDrawColor(nint renderer, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDrawColorFloat")]
    public static partial CBool GetRenderDrawColorFloat(nint renderer, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderColorScale")]
    public static partial CBool SetRenderColorScale(nint renderer, float scale);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderColorScale")]
    public static partial CBool GetRenderColorScale(nint renderer, nint scale);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderDrawBlendMode")]
    public static partial CBool SetRenderDrawBlendMode(nint renderer, uint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDrawBlendMode")]
    public static partial CBool GetRenderDrawBlendMode(nint renderer, nint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderClear")]
    public static partial CBool RenderClear(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderPoint")]
    public static partial CBool RenderPoint(nint renderer, float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderPoints")]
    public static partial CBool RenderPoints(nint renderer, nint points, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderLine")]
    public static partial CBool RenderLine(nint renderer, float x1, float y1, float x2, float y2);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderLines")]
    public static partial CBool RenderLines(nint renderer, nint points, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderRect")]
    public static partial CBool RenderRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderRects")]
    public static partial CBool RenderRects(nint renderer, nint rects, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderFillRect")]
    public static partial CBool RenderFillRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderFillRects")]
    public static partial CBool RenderFillRects(nint renderer, nint rects, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTexture")]
    public static partial CBool RenderTexture(nint renderer, nint texture, nint srcrect, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTextureRotated")]
    public static partial CBool RenderTextureRotated(nint renderer, nint texture, nint srcrect, nint dstrect, double angle, nint center, SdlFlipMode flip);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTextureTiled")]
    public static partial CBool RenderTextureTiled(nint renderer, nint texture, nint srcrect, float scale, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTexture9Grid")]
    public static partial CBool RenderTexture9Grid(nint renderer, nint texture, nint srcrect, float left_width, float right_width, float top_height, float bottom_height, float scale, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderGeometry")]
    public static partial CBool RenderGeometry(nint renderer, nint texture, nint vertices, int num_vertices, nint indices, int num_indices);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderGeometryRaw")]
    public static partial CBool RenderGeometryRaw(nint renderer, nint texture, nint xy, int xy_stride, nint color, int color_stride, nint uv, int uv_stride, int num_vertices, nint indices, int num_indices, int size_indices);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderReadPixels")]
    public static partial nint RenderReadPixels(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderPresent")]
    public static partial CBool RenderPresent(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyTexture")]
    public static partial void DestroyTexture(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyRenderer")]
    public static partial void DestroyRenderer(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushRenderer")]
    public static partial CBool FlushRenderer(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderMetalLayer")]
    public static partial nint GetRenderMetalLayer(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderMetalCommandEncoder")]
    public static partial nint GetRenderMetalCommandEncoder(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_AddVulkanRenderSemaphores")]
    public static partial CBool AddVulkanRenderSemaphores(nint renderer, uint wait_stage_mask, long wait_semaphore, long signal_semaphore);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderVSync")]
    public static partial CBool SetRenderVSync(nint renderer, int vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderVSync")]
    public static partial CBool GetRenderVSync(nint renderer, nint vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenTitleStorage")]
    public static partial nint OpenTitleStorage(nint @override, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenUserStorage")]
    public static partial nint OpenUserStorage(nint org, nint app, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenFileStorage")]
    public static partial nint OpenFileStorage(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenStorage")]
    public static partial nint OpenStorage(nint iface, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseStorage")]
    public static partial CBool CloseStorage(nint storage);

    [LibraryImport(Lib, EntryPoint = "SDL_StorageReady")]
    public static partial CBool StorageReady(nint storage);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStorageFileSize")]
    public static partial CBool GetStorageFileSize(nint storage, nint path, nint length);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadStorageFile")]
    public static partial CBool ReadStorageFile(nint storage, nint path, nint destination, ulong length);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteStorageFile")]
    public static partial CBool WriteStorageFile(nint storage, nint path, nint source, ulong length);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateStorageDirectory")]
    public static partial CBool CreateStorageDirectory(nint storage, nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_EnumerateStorageDirectory")]
    public static partial CBool EnumerateStorageDirectory(nint storage, nint path, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveStoragePath")]
    public static partial CBool RemoveStoragePath(nint storage, nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_RenameStoragePath")]
    public static partial CBool RenameStoragePath(nint storage, nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyStorageFile")]
    public static partial CBool CopyStorageFile(nint storage, nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStoragePathInfo")]
    public static partial CBool GetStoragePathInfo(nint storage, nint path, nint info);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStorageSpaceRemaining")]
    public static partial ulong GetStorageSpaceRemaining(nint storage);

    [LibraryImport(Lib, EntryPoint = "SDL_GlobStorageDirectory")]
    public static partial nint GlobStorageDirectory(nint storage, nint path, nint pattern, uint flags, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetX11EventHook")]
    public static partial void SetX11EventHook(nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLinuxThreadPriority")]
    public static partial CBool SetLinuxThreadPriority(long threadID, int priority);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLinuxThreadPriorityAndPolicy")]
    public static partial CBool SetLinuxThreadPriorityAndPolicy(long threadID, int sdlPriority, int schedPolicy);

    [LibraryImport(Lib, EntryPoint = "SDL_IsTablet")]
    public static partial CBool IsTablet();

    [LibraryImport(Lib, EntryPoint = "SDL_IsTV")]
    public static partial CBool IsTV();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationWillTerminate")]
    public static partial void OnApplicationWillTerminate();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationDidReceiveMemoryWarning")]
    public static partial void OnApplicationDidReceiveMemoryWarning();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationWillEnterBackground")]
    public static partial void OnApplicationWillEnterBackground();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationDidEnterBackground")]
    public static partial void OnApplicationDidEnterBackground();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationWillEnterForeground")]
    public static partial void OnApplicationWillEnterForeground();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationDidEnterForeground")]
    public static partial void OnApplicationDidEnterForeground();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDateTimeLocalePreferences")]
    public static partial CBool GetDateTimeLocalePreferences(nint dateFormat, nint timeFormat);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentTime")]
    public static partial CBool GetCurrentTime(nint ticks);

    [LibraryImport(Lib, EntryPoint = "SDL_TimeToDateTime")]
    public static partial CBool TimeToDateTime(long ticks, nint dt, CBool localTime);

    [LibraryImport(Lib, EntryPoint = "SDL_DateTimeToTime")]
    public static partial CBool DateTimeToTime(nint dt, nint ticks);

    [LibraryImport(Lib, EntryPoint = "SDL_TimeToWindows")]
    public static partial void TimeToWindows(long ticks, nint dwLowDateTime, nint dwHighDateTime);

    [LibraryImport(Lib, EntryPoint = "SDL_TimeFromWindows")]
    public static partial long TimeFromWindows(uint dwLowDateTime, uint dwHighDateTime);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDaysInMonth")]
    public static partial int GetDaysInMonth(int year, int month);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDayOfYear")]
    public static partial int GetDayOfYear(int year, int month, int day);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDayOfWeek")]
    public static partial int GetDayOfWeek(int year, int month, int day);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTicks")]
    public static partial ulong GetTicks();

    [LibraryImport(Lib, EntryPoint = "SDL_GetTicksNS")]
    public static partial ulong GetTicksNS();

    [LibraryImport(Lib, EntryPoint = "SDL_GetPerformanceCounter")]
    public static partial ulong GetPerformanceCounter();

    [LibraryImport(Lib, EntryPoint = "SDL_GetPerformanceFrequency")]
    public static partial ulong GetPerformanceFrequency();

    [LibraryImport(Lib, EntryPoint = "SDL_Delay")]
    public static partial void Delay(uint ms);

    [LibraryImport(Lib, EntryPoint = "SDL_DelayNS")]
    public static partial void DelayNS(ulong ns);

    [LibraryImport(Lib, EntryPoint = "SDL_AddTimer")]
    public static partial uint AddTimer(uint interval, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_AddTimerNS")]
    public static partial uint AddTimerNS(ulong interval, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveTimer")]
    public static partial CBool RemoveTimer(uint id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetVersion")]
    public static partial int GetVersion();

    [LibraryImport(Lib, EntryPoint = "SDL_GetRevision")]
    public static partial nint GetRevision();
}

#pragma warning restore CA1401
