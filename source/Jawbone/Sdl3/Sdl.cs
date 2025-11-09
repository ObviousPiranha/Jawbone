using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Jawbone.Sdl3;

#pragma warning disable CA1401

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void SdlAudioStreamCallback(
    nint userdata,
    nint stream,
    int additional_amount,
    int total_amount);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public unsafe delegate void SdlAudioPostmixCallback(
    nint userdata,
    in SdlAudioSpec spec,
    float* buffer,
    int buflen);

public static partial class Sdl
{
    private const string Lib = "SDL3";

    [LibraryImport(Lib, EntryPoint = "SDL_malloc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Malloc(nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_calloc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Calloc(nuint nmemb, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_realloc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Realloc(nint mem, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Free(nint mem);

    [LibraryImport(Lib, EntryPoint = "SDL_GetOriginalMemoryFunctions")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GetOriginalMemoryFunctions(nint malloc_func, nint calloc_func, nint realloc_func, nint free_func);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMemoryFunctions")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GetMemoryFunctions(nint malloc_func, nint calloc_func, nint realloc_func, nint free_func);

    [LibraryImport(Lib, EntryPoint = "SDL_SetMemoryFunctions")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetMemoryFunctions(nint malloc_func, nint calloc_func, nint realloc_func, nint free_func);

    [LibraryImport(Lib, EntryPoint = "SDL_aligned_alloc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint AlignedAlloc(nuint alignment, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_aligned_free")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void AlignedFree(nint mem);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumAllocations")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumAllocations();

    [LibraryImport(Lib, EntryPoint = "SDL_GetEnvironment")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetEnvironment();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateEnvironment")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateEnvironment(CBool populated);

    [LibraryImport(Lib, EntryPoint = "SDL_GetEnvironmentVariable")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetEnvironmentVariable(nint env, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetEnvironmentVariables")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetEnvironmentVariables(nint env);

    [LibraryImport(Lib, EntryPoint = "SDL_SetEnvironmentVariable")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetEnvironmentVariable(nint env, nint name, nint value, CBool overwrite);

    [LibraryImport(Lib, EntryPoint = "SDL_UnsetEnvironmentVariable")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UnsetEnvironmentVariable(nint env, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyEnvironment")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyEnvironment(nint env);

    [LibraryImport(Lib, EntryPoint = "SDL_getenv")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Getenv(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_getenv_unsafe")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetenvUnsafe(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_setenv_unsafe")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SetenvUnsafe(nint name, nint value, int overwrite);

    [LibraryImport(Lib, EntryPoint = "SDL_unsetenv_unsafe")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int UnsetenvUnsafe(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_qsort")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Qsort(nint @base, nuint nmemb, nuint size, nint compare);

    [LibraryImport(Lib, EntryPoint = "SDL_bsearch")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Bsearch(nint key, nint @base, nuint nmemb, nuint size, nint compare);

    [LibraryImport(Lib, EntryPoint = "SDL_qsort_r")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void QsortR(nint @base, nuint nmemb, nuint size, nint compare, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_bsearch_r")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint BsearchR(nint key, nint @base, nuint nmemb, nuint size, nint compare, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_abs")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Abs(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isalpha")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isalpha(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isalnum")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isalnum(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isblank")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isblank(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_iscntrl")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Iscntrl(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isdigit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isdigit(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isxdigit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isxdigit(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_ispunct")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Ispunct(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isspace")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isspace(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isupper")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isupper(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_islower")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Islower(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isprint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isprint(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_isgraph")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isgraph(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_toupper")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Toupper(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_tolower")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Tolower(int x);

    [LibraryImport(Lib, EntryPoint = "SDL_crc16")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort Crc16(ushort crc, nint data, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_crc32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint Crc32(uint crc, nint data, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_murmur3_32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint Murmur332(nint data, nuint len, uint seed);

    [LibraryImport(Lib, EntryPoint = "SDL_memcpy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Memcpy(nint dst, nint src, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_memmove")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Memmove(nint dst, nint src, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_memset")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Memset(nint dst, int c, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_memset4")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Memset4(nint dst, uint val, nuint dwords);

    [LibraryImport(Lib, EntryPoint = "SDL_memcmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Memcmp(nint s1, nint s2, nuint len);

    [LibraryImport(Lib, EntryPoint = "SDL_wcslen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Wcslen(nint wstr);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsnlen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Wcsnlen(nint wstr, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcslcpy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Wcslcpy(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcslcat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Wcslcat(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsdup")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Wcsdup(nint wstr);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsstr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Wcsstr(nint haystack, nint needle);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsnstr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Wcsnstr(nint haystack, nint needle, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcscmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Wcscmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsncmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Wcsncmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcscasecmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Wcscasecmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_wcsncasecmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Wcsncasecmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_wcstol")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long Wcstol(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strlen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Strlen(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strnlen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Strnlen(nint str, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strlcpy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Strlcpy(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_utf8strlcpy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Utf8Strlcpy(nint dst, nint src, nuint dst_bytes);

    [LibraryImport(Lib, EntryPoint = "SDL_strlcat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Strlcat(nint dst, nint src, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strdup")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strdup(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strndup")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strndup(nint str, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strrev")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strrev(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strupr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strupr(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strlwr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strlwr(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strchr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strchr(nint str, int c);

    [LibraryImport(Lib, EntryPoint = "SDL_strrchr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strrchr(nint str, int c);

    [LibraryImport(Lib, EntryPoint = "SDL_strstr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strstr(nint haystack, nint needle);

    [LibraryImport(Lib, EntryPoint = "SDL_strnstr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strnstr(nint haystack, nint needle, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strcasestr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strcasestr(nint haystack, nint needle);

    [LibraryImport(Lib, EntryPoint = "SDL_strtok_r")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint StrtokR(nint s1, nint s2, nint saveptr);

    [LibraryImport(Lib, EntryPoint = "SDL_utf8strlen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Utf8Strlen(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_utf8strnlen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Utf8Strnlen(nint str, nuint bytes);

    [LibraryImport(Lib, EntryPoint = "SDL_itoa")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Itoa(int value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_uitoa")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Uitoa(uint value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_ltoa")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Ltoa(long value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_ultoa")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Ultoa(ulong value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_lltoa")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Lltoa(long value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_ulltoa")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Ulltoa(ulong value, nint str, int radix);

    [LibraryImport(Lib, EntryPoint = "SDL_atoi")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Atoi(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_atof")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Atof(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_strtol")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long Strtol(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtoul")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong Strtoul(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtoll")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long Strtoll(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtoull")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong Strtoull(nint str, nint endp, int @base);

    [LibraryImport(Lib, EntryPoint = "SDL_strtod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Strtod(nint str, nint endp);

    [LibraryImport(Lib, EntryPoint = "SDL_strcmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Strcmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_strncmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Strncmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strcasecmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Strcasecmp(nint str1, nint str2);

    [LibraryImport(Lib, EntryPoint = "SDL_strncasecmp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Strncasecmp(nint str1, nint str2, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_strpbrk")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint Strpbrk(nint str, nint breakset);

    [LibraryImport(Lib, EntryPoint = "SDL_StepUTF8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint StepUTF8(nint pstr, nint pslen);

    [LibraryImport(Lib, EntryPoint = "SDL_UCS4ToUTF8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint UCS4ToUTF8(uint codepoint, nint dst);

    [LibraryImport(Lib, EntryPoint = "SDL_sscanf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Sscanf(nint text, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_snprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Snprintf(nint text, nuint maxlen, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_swprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Swprintf(nint text, nuint maxlen, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_asprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Asprintf(nint strp, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_srand")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Srand(ulong seed);

    [LibraryImport(Lib, EntryPoint = "SDL_rand")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Rand(int n);

    [LibraryImport(Lib, EntryPoint = "SDL_randf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Randf();

    [LibraryImport(Lib, EntryPoint = "SDL_rand_bits")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint RandBits();

    [LibraryImport(Lib, EntryPoint = "SDL_rand_r")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int RandR(nint state, int n);

    [LibraryImport(Lib, EntryPoint = "SDL_randf_r")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float RandfR(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_rand_bits_r")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint RandBitsR(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_acos")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Acos(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_acosf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Acosf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_asin")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Asin(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_asinf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Asinf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_atan")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Atan(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_atanf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Atanf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_atan2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Atan2(double y, double x);

    [LibraryImport(Lib, EntryPoint = "SDL_atan2f")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Atan2F(float y, float x);

    [LibraryImport(Lib, EntryPoint = "SDL_ceil")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Ceil(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_ceilf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Ceilf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_copysign")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Copysign(double x, double y);

    [LibraryImport(Lib, EntryPoint = "SDL_copysignf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Copysignf(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_cos")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Cos(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_cosf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Cosf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_exp")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Exp(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_expf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Expf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_fabs")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Fabs(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_fabsf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Fabsf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_floor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Floor(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_floorf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Floorf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_trunc")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Trunc(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_truncf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Truncf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_fmod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Fmod(double x, double y);

    [LibraryImport(Lib, EntryPoint = "SDL_fmodf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Fmodf(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_isinf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isinf(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_isinff")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isinff(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_isnan")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isnan(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_isnanf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int Isnanf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Log(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_logf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Logf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_log10")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Log10(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_log10f")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Log10F(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_modf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Modf(double x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_modff")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Modff(float x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_pow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Pow(double x, double y);

    [LibraryImport(Lib, EntryPoint = "SDL_powf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Powf(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_round")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Round(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_roundf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Roundf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_lround")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long Lround(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_lroundf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long Lroundf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_scalbn")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Scalbn(double x, int n);

    [LibraryImport(Lib, EntryPoint = "SDL_scalbnf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Scalbnf(float x, int n);

    [LibraryImport(Lib, EntryPoint = "SDL_sin")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Sin(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_sinf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Sinf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_sqrt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Sqrt(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_sqrtf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Sqrtf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_tan")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial double Tan(double x);

    [LibraryImport(Lib, EntryPoint = "SDL_tanf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float Tanf(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv_open")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint IconvOpen(nint tocode, nint fromcode);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv_close")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int IconvClose(nint cd);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint Iconv(nint cd, nint inbuf, nint inbytesleft, nint outbuf, nint outbytesleft);

    [LibraryImport(Lib, EntryPoint = "SDL_iconv_string")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint IconvString(nint tocode, nint fromcode, nint inbuf, nuint inbytesleft);

    [LibraryImport(Lib, EntryPoint = "SDL_size_mul_check_overflow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SizeMulCheckOverflow(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_size_mul_check_overflow_builtin")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SizeMulCheckOverflowBuiltin(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_size_add_check_overflow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SizeAddCheckOverflow(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_size_add_check_overflow_builtin")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SizeAddCheckOverflowBuiltin(nuint a, nuint b, nint ret);

    [LibraryImport(Lib, EntryPoint = "SDL_ReportAssertion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlAssertState ReportAssertion(nint data, nint func, nint file, int line);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAssertionHandler")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetAssertionHandler(nint handler, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDefaultAssertionHandler")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetDefaultAssertionHandler();

    [LibraryImport(Lib, EntryPoint = "SDL_GetAssertionHandler")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAssertionHandler(nint puserdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAssertionReport")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAssertionReport();

    [LibraryImport(Lib, EntryPoint = "SDL_ResetAssertionReport")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ResetAssertionReport();

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockSpinlock")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TryLockSpinlock(nint @lock);

    [LibraryImport(Lib, EntryPoint = "SDL_LockSpinlock")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LockSpinlock(nint @lock);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockSpinlock")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockSpinlock(nint @lock);

    [LibraryImport(Lib, EntryPoint = "SDL_MemoryBarrierReleaseFunction")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void MemoryBarrierReleaseFunction();

    [LibraryImport(Lib, EntryPoint = "SDL_MemoryBarrierAcquireFunction")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void MemoryBarrierAcquireFunction();

    [LibraryImport(Lib, EntryPoint = "SDL_CompareAndSwapAtomicInt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CompareAndSwapAtomicInt(nint a, int oldval, int newval);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAtomicInt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SetAtomicInt(nint a, int v);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAtomicInt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAtomicInt(nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_AddAtomicInt")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int AddAtomicInt(nint a, int v);

    [LibraryImport(Lib, EntryPoint = "SDL_CompareAndSwapAtomicU32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CompareAndSwapAtomicU32(nint a, uint oldval, uint newval);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAtomicU32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint SetAtomicU32(nint a, uint v);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAtomicU32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetAtomicU32(nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_CompareAndSwapAtomicPointer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CompareAndSwapAtomicPointer(nint a, nint oldval, nint newval);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAtomicPointer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint SetAtomicPointer(nint a, nint v);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAtomicPointer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAtomicPointer(nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_SwapFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float SwapFloat(float x);

    [LibraryImport(Lib, EntryPoint = "SDL_SetError")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetError(nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_OutOfMemory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool OutOfMemory();

    [LibraryImport(Lib, EntryPoint = "SDL_GetError")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetError();

    [LibraryImport(Lib, EntryPoint = "SDL_ClearError")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClearError();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGlobalProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetGlobalProperties();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint CreateProperties();

    [LibraryImport(Lib, EntryPoint = "SDL_CopyProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CopyProperties(uint src, uint dst);

    [LibraryImport(Lib, EntryPoint = "SDL_LockProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LockProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_SetPointerPropertyWithCleanup")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetPointerPropertyWithCleanup(uint props, nint name, nint value, nint cleanup, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetPointerProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetPointerProperty(uint props, nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetStringProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetStringProperty(uint props, nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetNumberProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetNumberProperty(uint props, nint name, long value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetFloatProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetFloatProperty(uint props, nint name, float value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetBooleanProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetBooleanProperty(uint props, in byte name, CBool value);

    [LibraryImport(Lib, EntryPoint = "SDL_HasProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasProperty(uint props, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPropertyType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlPropertyType GetPropertyType(uint props, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPointerProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPointerProperty(uint props, nint name, nint default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStringProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetStringProperty(uint props, nint name, nint default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumberProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long GetNumberProperty(uint props, nint name, long default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetFloatProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetFloatProperty(uint props, nint name, float default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetBooleanProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetBooleanProperty(uint props, nint name, CBool default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClearProperty(uint props, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_EnumerateProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool EnumerateProperties(uint props, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateThreadRuntime")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateThreadRuntime(nint fn, nint name, nint data, nint pfnBeginThread, nint pfnEndThread);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateThreadWithPropertiesRuntime")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateThreadWithPropertiesRuntime(uint props, nint pfnBeginThread, nint pfnEndThread);

    [LibraryImport(Lib, EntryPoint = "SDL_GetThreadName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetThreadName(nint thread);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentThreadID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetCurrentThreadID();

    [LibraryImport(Lib, EntryPoint = "SDL_GetThreadID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetThreadID(nint thread);

    [LibraryImport(Lib, EntryPoint = "SDL_SetCurrentThreadPriority")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetCurrentThreadPriority(SdlThreadPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitThread")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WaitThread(nint thread, nint status);

    [LibraryImport(Lib, EntryPoint = "SDL_DetachThread")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DetachThread(nint thread);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTLS")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetTLS(nint id);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTLS")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTLS(nint id, nint value, nint destructor);

    [LibraryImport(Lib, EntryPoint = "SDL_CleanupTLS")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CleanupTLS();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateMutex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateMutex();

    [LibraryImport(Lib, EntryPoint = "SDL_LockMutex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LockMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockMutex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TryLockMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockMutex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyMutex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyMutex(nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateRWLock")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateRWLock();

    [LibraryImport(Lib, EntryPoint = "SDL_LockRWLockForReading")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LockRWLockForReading(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_LockRWLockForWriting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LockRWLockForWriting(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockRWLockForReading")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TryLockRWLockForReading(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_TryLockRWLockForWriting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TryLockRWLockForWriting(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockRWLock")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockRWLock(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyRWLock")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyRWLock(nint rwlock);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSemaphore")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateSemaphore(uint initial_value);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroySemaphore")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroySemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitSemaphore")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WaitSemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_TryWaitSemaphore")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TryWaitSemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitSemaphoreTimeout")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitSemaphoreTimeout(nint sem, int timeoutMS);

    [LibraryImport(Lib, EntryPoint = "SDL_SignalSemaphore")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SignalSemaphore(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSemaphoreValue")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetSemaphoreValue(nint sem);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateCondition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateCondition();

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyCondition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyCondition(nint cond);

    [LibraryImport(Lib, EntryPoint = "SDL_SignalCondition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SignalCondition(nint cond);

    [LibraryImport(Lib, EntryPoint = "SDL_BroadcastCondition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BroadcastCondition(nint cond);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitCondition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WaitCondition(nint cond, nint mutex);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitConditionTimeout")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitConditionTimeout(nint cond, nint mutex, int timeoutMS);

    [LibraryImport(Lib, EntryPoint = "SDL_ShouldInit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShouldInit(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_ShouldQuit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShouldQuit(nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_SetInitialized")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetInitialized(nint state, CBool initialized);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint IOFromFile(nint file, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromMem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint IOFromMem(nint mem, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromConstMem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint IOFromConstMem(nint mem, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_IOFromDynamicMem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint IOFromDynamicMem();

    [LibraryImport(Lib, EntryPoint = "SDL_OpenIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenIO(nint iface, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CloseIO(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GetIOProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetIOProperties(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GetIOStatus")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlIoStatus GetIOStatus(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GetIOSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long GetIOSize(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_SeekIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SeekIO(nint context, long offset, SdlIoWhence whence);

    [LibraryImport(Lib, EntryPoint = "SDL_TellIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long TellIO(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint ReadIO(nint context, nint ptr, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint WriteIO(nint context, nint ptr, nuint size);

    [LibraryImport(Lib, EntryPoint = "SDL_IOprintf")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint IOprintf(nint context, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FlushIO(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadFile_IO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadFileIO(nint src, nint datasize, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadFile(nint file, nint datasize);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU8(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS8(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU16LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS16LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU16BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS16BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU32LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS32LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU32BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS32BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU64LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS64LE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadU64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadU64BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadS64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadS64BE(nint src, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU8(nint dst, byte value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS8")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS8(nint dst, sbyte value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU16LE(nint dst, ushort value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS16LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS16LE(nint dst, short value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU16BE(nint dst, ushort value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS16BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS16BE(nint dst, short value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU32LE(nint dst, uint value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS32LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS32LE(nint dst, int value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU32BE(nint dst, uint value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS32BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS32BE(nint dst, int value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU64LE(nint dst, ulong value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS64LE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS64LE(nint dst, long value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteU64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteU64BE(nint dst, ulong value);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteS64BE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteS64BE(nint dst, long value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumAudioDrivers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumAudioDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentAudioDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCurrentAudioDriver();

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioPlaybackDevices")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioPlaybackDevices(out int count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioRecordingDevices")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioRecordingDevices(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetAudioDeviceName(uint devid);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetAudioDeviceFormat(uint devid, nint spec, nint sample_frames);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceChannelMap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioDeviceChannelMap(uint devid, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenAudioDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint OpenAudioDevice(uint devid, in SdlAudioSpec spec);

    [LibraryImport(Lib, EntryPoint = "SDL_PauseAudioDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PauseAudioDevice(uint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_ResumeAudioDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ResumeAudioDevice(uint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_AudioDevicePaused")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool AudioDevicePaused(uint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioDeviceGain")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetAudioDeviceGain(uint devid);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioDeviceGain")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioDeviceGain(uint devid, float gain);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseAudioDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CloseAudioDevice(uint devid);

    [LibraryImport(Lib, EntryPoint = "SDL_BindAudioStreams")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BindAudioStreams(uint devid, nint streams, int num_streams);

    [LibraryImport(Lib, EntryPoint = "SDL_BindAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BindAudioStream(uint devid, nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_UnbindAudioStreams")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnbindAudioStreams(nint streams, int num_streams);

    [LibraryImport(Lib, EntryPoint = "SDL_UnbindAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnbindAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetAudioStreamDevice(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateAudioStream(in SdlAudioSpec src_spec, in SdlAudioSpec dst_spec);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetAudioStreamProperties(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetAudioStreamFormat(nint stream, nint src_spec, nint dst_spec);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamFormat(nint stream, nint src_spec, nint dst_spec);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamFrequencyRatio")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetAudioStreamFrequencyRatio(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamFrequencyRatio")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamFrequencyRatio(nint stream, float ratio);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamGain")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetAudioStreamGain(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamGain")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamGain(nint stream, float gain);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamInputChannelMap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioStreamInputChannelMap(nint stream, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamOutputChannelMap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioStreamOutputChannelMap(nint stream, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamInputChannelMap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamInputChannelMap(nint stream, nint chmap, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamOutputChannelMap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamOutputChannelMap(nint stream, nint chmap, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_PutAudioStreamData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PutAudioStreamData(nint stream, in byte buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_PutAudioStreamData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PutAudioStreamData(nint stream, in short buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_PutAudioStreamData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PutAudioStreamData(nint stream, in float buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAudioStreamData(nint stream, out byte buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAudioStreamData(nint stream, out short buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAudioStreamData(nint stream, out float buf, int len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamAvailable")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAudioStreamAvailable(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioStreamQueued")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetAudioStreamQueued(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FlushAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClearAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_PauseAudioStreamDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PauseAudioStreamDevice(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_ResumeAudioStreamDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ResumeAudioStreamDevice(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_LockAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LockAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UnlockAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamGetCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamGetCallback(nint stream, SdlAudioStreamCallback? callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioStreamPutCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioStreamPutCallback(nint stream, SdlAudioStreamCallback? callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyAudioStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyAudioStream(nint stream);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenAudioDeviceStream")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenAudioDeviceStream(uint devid, nint spec, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAudioPostmixCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetAudioPostmixCallback(uint devid, SdlAudioPostmixCallback? callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadWAV_IO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LoadWAVIO(nint src, CBool closeio, nint spec, nint audio_buf, nint audio_len);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadWAV")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LoadWAV(nint path, nint spec, nint audio_buf, nint audio_len);

    [LibraryImport(Lib, EntryPoint = "SDL_MixAudio")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool MixAudio(nint dst, nint src, SdlAudioFormat format, uint len, float volume);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertAudioSamples")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ConvertAudioSamples(nint src_spec, nint src_data, int src_len, nint dst_spec, nint dst_data, nint dst_len);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAudioFormatName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAudioFormatName(SdlAudioFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSilenceValueForFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetSilenceValueForFormat(SdlAudioFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_MostSignificantBitIndex32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int MostSignificantBitIndex32(uint x);

    [LibraryImport(Lib, EntryPoint = "SDL_HasExactlyOneBitSet32")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasExactlyOneBitSet32(uint x);

    [LibraryImport(Lib, EntryPoint = "SDL_ComposeCustomBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint ComposeCustomBlendMode(SdlBlendFactor srcColorFactor, SdlBlendFactor dstColorFactor, SdlBlendOperation colorOperation, SdlBlendFactor srcAlphaFactor, SdlBlendFactor dstAlphaFactor, SdlBlendOperation alphaOperation);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPixelFormatName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPixelFormatName(SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMasksForPixelFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetMasksForPixelFormat(SdlPixelFormat format, nint bpp, nint Rmask, nint Gmask, nint Bmask, nint Amask);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPixelFormatForMasks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlPixelFormat GetPixelFormatForMasks(int bpp, uint Rmask, uint Gmask, uint Bmask, uint Amask);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPixelFormatDetails")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPixelFormatDetails(SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_CreatePalette")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreatePalette(int ncolors);

    [LibraryImport(Lib, EntryPoint = "SDL_SetPaletteColors")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetPaletteColors(nint palette, nint colors, int firstcolor, int ncolors);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyPalette")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyPalette(nint palette);

    [LibraryImport(Lib, EntryPoint = "SDL_MapRGB")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint MapRGB(nint format, nint palette, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_MapRGBA")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint MapRGBA(nint format, nint palette, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRGB")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GetRGB(uint pixel, nint format, nint palette, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRGBA")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GetRGBA(uint pixel, nint format, nint palette, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_RectToFRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void RectToFRect(nint rect, nint frect);

    [LibraryImport(Lib, EntryPoint = "SDL_PointInRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PointInRect(nint p, nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectEmpty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RectEmpty(nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectsEqual")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RectsEqual(nint a, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_HasRectIntersection")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasRectIntersection(nint A, nint B);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectIntersection")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectIntersection(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectUnion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectUnion(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectEnclosingPoints")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectEnclosingPoints(nint points, int count, nint clip, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectAndLineIntersection")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectAndLineIntersection(nint rect, nint X1, nint Y1, nint X2, nint Y2);

    [LibraryImport(Lib, EntryPoint = "SDL_PointInRectFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PointInRectFloat(nint p, nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectEmptyFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RectEmptyFloat(nint r);

    [LibraryImport(Lib, EntryPoint = "SDL_RectsEqualEpsilon")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RectsEqualEpsilon(nint a, nint b, float epsilon);

    [LibraryImport(Lib, EntryPoint = "SDL_RectsEqualFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RectsEqualFloat(nint a, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_HasRectIntersectionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasRectIntersectionFloat(nint A, nint B);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectIntersectionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectIntersectionFloat(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectUnionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectUnionFloat(nint A, nint B, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectEnclosingPointsFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectEnclosingPointsFloat(nint points, int count, nint clip, nint result);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRectAndLineIntersectionFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRectAndLineIntersectionFloat(nint rect, nint X1, nint Y1, nint X2, nint Y2);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateSurface(int width, int height, SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSurfaceFrom")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateSurfaceFrom(int width, int height, SdlPixelFormat format, nint pixels, int pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroySurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroySurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetSurfaceProperties(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceColorspace")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceColorspace(nint surface, SdlColorspace colorspace);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceColorspace")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlColorspace GetSurfaceColorspace(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSurfacePalette")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateSurfacePalette(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfacePalette")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfacePalette(nint surface, nint palette);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfacePalette")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetSurfacePalette(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_AddSurfaceAlternateImage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool AddSurfaceAlternateImage(nint surface, nint image);

    [LibraryImport(Lib, EntryPoint = "SDL_SurfaceHasAlternateImages")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SurfaceHasAlternateImages(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceImages")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetSurfaceImages(nint surface, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveSurfaceAlternateImages")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void RemoveSurfaceAlternateImages(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_LockSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LockSurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockSurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadBMP_IO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadBMPIO(nint src, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadBMP")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadBMP(nint file);

    [LibraryImport(Lib, EntryPoint = "SDL_SaveBMP_IO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SaveBMPIO(nint surface, nint dst, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_SaveBMP")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SaveBMP(nint surface, nint file);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceRLE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceRLE(nint surface, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_SurfaceHasRLE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SurfaceHasRLE(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceColorKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceColorKey(nint surface, CBool enabled, uint key);

    [LibraryImport(Lib, EntryPoint = "SDL_SurfaceHasColorKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SurfaceHasColorKey(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceColorKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetSurfaceColorKey(nint surface, nint key);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceColorMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceColorMod(nint surface, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceColorMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetSurfaceColorMod(nint surface, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceAlphaMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceAlphaMod(nint surface, byte alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceAlphaMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetSurfaceAlphaMod(nint surface, nint alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceBlendMode(nint surface, uint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetSurfaceBlendMode(nint surface, nint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_SetSurfaceClipRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetSurfaceClipRect(nint surface, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSurfaceClipRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetSurfaceClipRect(nint surface, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_FlipSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FlipSurface(nint surface, SdlFlipMode flip);

    [LibraryImport(Lib, EntryPoint = "SDL_DuplicateSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint DuplicateSurface(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_ScaleSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint ScaleSurface(nint surface, int width, int height, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint ConvertSurface(nint surface, SdlPixelFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertSurfaceAndColorspace")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint ConvertSurfaceAndColorspace(nint surface, SdlPixelFormat format, nint palette, SdlColorspace colorspace, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertPixels")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ConvertPixels(int width, int height, SdlPixelFormat src_format, nint src, int src_pitch, SdlPixelFormat dst_format, nint dst, int dst_pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertPixelsAndColorspace")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ConvertPixelsAndColorspace(int width, int height, SdlPixelFormat src_format, SdlColorspace src_colorspace, uint src_properties, nint src, int src_pitch, SdlPixelFormat dst_format, SdlColorspace dst_colorspace, uint dst_properties, nint dst, int dst_pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_PremultiplyAlpha")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PremultiplyAlpha(int width, int height, SdlPixelFormat src_format, nint src, int src_pitch, SdlPixelFormat dst_format, nint dst, int dst_pitch, CBool linear);

    [LibraryImport(Lib, EntryPoint = "SDL_PremultiplySurfaceAlpha")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PremultiplySurfaceAlpha(nint surface, CBool linear);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClearSurface(nint surface, float r, float g, float b, float a);

    [LibraryImport(Lib, EntryPoint = "SDL_FillSurfaceRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FillSurfaceRect(nint dst, nint rect, uint color);

    [LibraryImport(Lib, EntryPoint = "SDL_FillSurfaceRects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FillSurfaceRects(nint dst, nint rects, int count, uint color);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurface(nint src, in SdlRect srcrect, nint dst, in SdlRect dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceUnchecked")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurfaceUnchecked(nint src, nint srcrect, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceScaled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurfaceScaled(nint src, nint srcrect, nint dst, nint dstrect, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceUncheckedScaled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurfaceUncheckedScaled(nint src, nint srcrect, nint dst, nint dstrect, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceTiled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurfaceTiled(nint src, nint srcrect, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurfaceTiledWithScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurfaceTiledWithScale(nint src, nint srcrect, float scale, SdlScaleMode scaleMode, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitSurface9Grid")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool BlitSurface9Grid(nint src, nint srcrect, int left_width, int right_width, int top_height, int bottom_height, float scale, SdlScaleMode scaleMode, nint dst, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_MapSurfaceRGB")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint MapSurfaceRGB(nint surface, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_MapSurfaceRGBA")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint MapSurfaceRGBA(nint surface, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadSurfacePixel")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadSurfacePixel(nint surface, int x, int y, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadSurfacePixelFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadSurfacePixelFloat(nint surface, int x, int y, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteSurfacePixel")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteSurfacePixel(nint surface, int x, int y, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteSurfacePixelFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteSurfacePixelFloat(nint surface, int x, int y, float r, float g, float b, float a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumCameraDrivers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumCameraDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCameraDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentCameraDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCurrentCameraDriver();

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameras")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCameras(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraSupportedFormats")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCameraSupportedFormats(uint devid, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetCameraName(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraPosition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlCameraPosition GetCameraPosition(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenCamera")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenCamera(uint instance_id, nint spec);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraPermissionState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetCameraPermissionState(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetCameraID(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetCameraProperties(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCameraFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetCameraFormat(nint camera, nint spec);

    [LibraryImport(Lib, EntryPoint = "SDL_AcquireCameraFrame")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint AcquireCameraFrame(nint camera, nint timestampNS);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseCameraFrame")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseCameraFrame(nint camera, nint frame);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseCamera")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CloseCamera(nint camera);

    [LibraryImport(Lib, EntryPoint = "SDL_SetClipboardText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetClipboardText(nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_GetClipboardText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetClipboardText();

    [LibraryImport(Lib, EntryPoint = "SDL_HasClipboardText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasClipboardText();

    [LibraryImport(Lib, EntryPoint = "SDL_SetPrimarySelectionText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetPrimarySelectionText(nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPrimarySelectionText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPrimarySelectionText();

    [LibraryImport(Lib, EntryPoint = "SDL_HasPrimarySelectionText")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasPrimarySelectionText();

    [LibraryImport(Lib, EntryPoint = "SDL_SetClipboardData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetClipboardData(nint callback, nint cleanup, nint userdata, nint mime_types, nuint num_mime_types);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearClipboardData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClearClipboardData();

    [LibraryImport(Lib, EntryPoint = "SDL_GetClipboardData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetClipboardData(nint mime_type, nint size);

    [LibraryImport(Lib, EntryPoint = "SDL_HasClipboardData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasClipboardData(nint mime_type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetClipboardMimeTypes")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetClipboardMimeTypes(nint num_mime_types);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumLogicalCPUCores")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumLogicalCpuCores();

    [LibraryImport(Lib, EntryPoint = "SDL_GetCPUCacheLineSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetCpuCacheLineSize();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAltiVec")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasAltiVec();

    [LibraryImport(Lib, EntryPoint = "SDL_HasMMX")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasMMX();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasSSE();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasSSE2();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE3")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasSSE3();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE41")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasSSE41();

    [LibraryImport(Lib, EntryPoint = "SDL_HasSSE42")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasSSE42();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAVX")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasAVX();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAVX2")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasAVX2();

    [LibraryImport(Lib, EntryPoint = "SDL_HasAVX512F")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasAVX512F();

    [LibraryImport(Lib, EntryPoint = "SDL_HasARMSIMD")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasARMSIMD();

    [LibraryImport(Lib, EntryPoint = "SDL_HasNEON")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasNEON();

    [LibraryImport(Lib, EntryPoint = "SDL_HasLSX")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasLSX();

    [LibraryImport(Lib, EntryPoint = "SDL_HasLASX")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasLASX();

    [LibraryImport(Lib, EntryPoint = "SDL_GetSystemRAM")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetSystemRAM();

    [LibraryImport(Lib, EntryPoint = "SDL_GetSIMDAlignment")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint GetSIMDAlignment();

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumVideoDrivers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumVideoDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetVideoDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetVideoDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentVideoDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetCurrentVideoDriver();

    [LibraryImport(Lib, EntryPoint = "SDL_GetSystemTheme")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlSystemTheme GetSystemTheme();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplays")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetDisplays(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPrimaryDisplay")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetPrimaryDisplay();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetDisplayProperties(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetDisplayName(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayBounds")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetDisplayBounds(uint displayID, out SdlRect rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayUsableBounds")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetDisplayUsableBounds(uint displayID, out SdlRect rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNaturalDisplayOrientation")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlDisplayOrientation GetNaturalDisplayOrientation(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentDisplayOrientation")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlDisplayOrientation GetCurrentDisplayOrientation(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayContentScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetDisplayContentScale(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetFullscreenDisplayModes")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetFullscreenDisplayModes(uint displayID, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetClosestFullscreenDisplayMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetClosestFullscreenDisplayMode(uint displayID, int w, int h, float refresh_rate, CBool include_high_density_modes, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDesktopDisplayMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetDesktopDisplayMode(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentDisplayMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCurrentDisplayMode(uint displayID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayForPoint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetDisplayForPoint(nint point);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayForRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetDisplayForRect(nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDisplayForWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetDisplayForWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowPixelDensity")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetWindowPixelDensity(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowDisplayScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetWindowDisplayScale(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowFullscreenMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowFullscreenMode(nint window, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFullscreenMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowFullscreenMode(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowICCProfile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowICCProfile(nint window, nint size);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowPixelFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlPixelFormat GetWindowPixelFormat(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindows")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindows(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateWindow", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint CreateWindow(string title, int w, int h, SdlWindowFlags flags);

    [LibraryImport(Lib, EntryPoint = "SDL_CreatePopupWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreatePopupWindow(nint parent, int offset_x, int offset_y, int w, int h, ulong flags);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateWindowWithProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateWindowWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetWindowID(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFromID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowFromID(uint id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowParent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowParent(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetWindowProperties(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFlags")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetWindowFlags(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowTitle", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowTitle(nint window, string title);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowTitle")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowTitle(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowIcon")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowIcon(nint window, nint icon);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowPosition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowPosition(nint window, int x, int y);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowPosition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowPosition(nint window, nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowSize(nint window, int w, int h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowSize(nint window, out int w, out int h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSafeArea")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowSafeArea(nint window, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowAspectRatio")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowAspectRatio(nint window, float min_aspect, float max_aspect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowAspectRatio")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowAspectRatio(nint window, nint min_aspect, nint max_aspect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowBordersSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowBordersSize(nint window, nint top, nint left, nint bottom, nint right);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSizeInPixels")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowSizeInPixels(nint window, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMinimumSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowMinimumSize(nint window, int min_w, int min_h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMinimumSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowMinimumSize(nint window, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMaximumSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowMaximumSize(nint window, int max_w, int max_h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMaximumSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowMaximumSize(nint window, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowBordered")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowBordered(nint window, CBool bordered);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowResizable")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowResizable(nint window, CBool resizable);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowAlwaysOnTop")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowAlwaysOnTop(nint window, CBool on_top);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShowWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_HideWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HideWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_RaiseWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RaiseWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_MaximizeWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool MaximizeWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_MinimizeWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool MinimizeWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_RestoreWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RestoreWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowFullscreen")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowFullscreen(nint window, CBool fullscreen);

    [LibraryImport(Lib, EntryPoint = "SDL_SyncWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SyncWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_WindowHasSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WindowHasSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowSurfaceVSync")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowSurfaceVSync(nint window, int vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowSurfaceVSync")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowSurfaceVSync(nint window, nint vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateWindowSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UpdateWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateWindowSurfaceRects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UpdateWindowSurfaceRects(nint window, nint rects, int numrects);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyWindowSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool DestroyWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowKeyboardGrab")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowKeyboardGrab(nint window, CBool grabbed);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMouseGrab")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowMouseGrab(nint window, CBool grabbed);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowKeyboardGrab")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowKeyboardGrab(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMouseGrab")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowMouseGrab(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGrabbedWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGrabbedWindow();

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowMouseRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowMouseRect(nint window, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowMouseRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowMouseRect(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowOpacity")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowOpacity(nint window, float opacity);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowOpacity")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetWindowOpacity(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowParent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowParent(nint window, nint parent);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowModal")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowModal(nint window, CBool modal);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowFocusable")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowFocusable(nint window, CBool focusable);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowWindowSystemMenu")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShowWindowSystemMenu(nint window, int x, int y);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowHitTest")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowHitTest(nint window, nint callback, nint callback_data);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowShape")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowShape(nint window, nint shape);

    [LibraryImport(Lib, EntryPoint = "SDL_FlashWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FlashWindow(nint window, SdlFlashOperation operation);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_ScreenSaverEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ScreenSaverEnabled();

    [LibraryImport(Lib, EntryPoint = "SDL_EnableScreenSaver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool EnableScreenSaver();

    [LibraryImport(Lib, EntryPoint = "SDL_DisableScreenSaver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool DisableScreenSaver();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_LoadLibrary")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlLoadLibrary(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetProcAddress", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint GlGetProcAddress(string proc);

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetProcAddress", StringMarshalling = StringMarshalling.Utf8)]
    public static partial nint EglGetProcAddress(string proc);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_UnloadLibrary")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GlUnloadLibrary();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_ExtensionSupported")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlExtensionSupported(nint extension);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_ResetAttributes")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GlResetAttributes();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_SetAttribute")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlSetAttribute(SdlGlAttr attr, int value);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetAttribute")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlGetAttribute(SdlGlAttr attr, out int value);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_CreateContext")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GlCreateContext(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_MakeCurrent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlMakeCurrent(nint window, nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetCurrentWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GlGetCurrentWindow();

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetCurrentContext")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GlGetCurrentContext();

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetCurrentDisplay")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint EglGetCurrentDisplay();

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetCurrentConfig")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint EglGetCurrentConfig();

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_GetWindowSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint EglGetWindowSurface(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_EGL_SetAttributeCallbacks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EglSetAttributeCallbacks(nint platformAttribCallback, nint surfaceAttribCallback, nint contextAttribCallback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_SetSwapInterval")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlSetSwapInterval(int interval);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_GetSwapInterval")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlGetSwapInterval(nint interval);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_SwapWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlSwapWindow(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GL_DestroyContext")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GlDestroyContext(nint context);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowOpenFileDialog")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ShowOpenFileDialog(nint callback, nint userdata, nint window, nint filters, int nfilters, nint default_location, CBool allow_many);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowSaveFileDialog")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ShowSaveFileDialog(nint callback, nint userdata, nint window, nint filters, int nfilters, nint default_location);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowOpenFolderDialog")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ShowOpenFolderDialog(nint callback, nint userdata, nint window, nint default_location, CBool allow_many);

    [LibraryImport(Lib, EntryPoint = "SDL_GUIDToString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GUIDToString(nint guid, nint pszGUID, int cbGUID);

    [LibraryImport(Lib, EntryPoint = "SDL_StringToGUID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint StringToGUID(nint pchGUID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPowerInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlPowerState GetPowerInfo(nint seconds, nint percent);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensors")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetSensors(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorNameForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetSensorNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorTypeForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlSensorType GetSensorTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorNonPortableTypeForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetSensorNonPortableTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenSensor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenSensor(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorFromID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetSensorFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetSensorProperties(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetSensorName(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlSensorType GetSensorType(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorNonPortableType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetSensorNonPortableType(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetSensorID(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetSensorData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetSensorData(nint sensor, nint data, int num_values);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseSensor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CloseSensor(nint sensor);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateSensors")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UpdateSensors();

    [LibraryImport(Lib, EntryPoint = "SDL_LockJoysticks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LockJoysticks();

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockJoysticks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockJoysticks();

    [LibraryImport(Lib, EntryPoint = "SDL_HasJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasJoystick();

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoysticks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoysticks(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickNameForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPathForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickPathForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPlayerIndexForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetJoystickPlayerIndexForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickGUIDForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickGUIDForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickVendorForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickVendorForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProductForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickProductForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProductVersionForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickProductVersionForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickTypeForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlJoystickType GetJoystickTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenJoystick(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickFromID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickFromPlayerIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickFromPlayerIndex(int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_AttachVirtualJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint AttachVirtualJoystick(nint desc);

    [LibraryImport(Lib, EntryPoint = "SDL_DetachVirtualJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool DetachVirtualJoystick(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_IsJoystickVirtual")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool IsJoystickVirtual(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualAxis")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickVirtualAxis(nint joystick, int axis, short value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualBall")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickVirtualBall(nint joystick, int ball, short xrel, short yrel);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualButton")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickVirtualButton(nint joystick, int button, CBool down);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualHat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickVirtualHat(nint joystick, int hat, byte value);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickVirtualTouchpad")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickVirtualTouchpad(nint joystick, int touchpad, int finger, CBool down, float x, float y, float pressure);

    [LibraryImport(Lib, EntryPoint = "SDL_SendJoystickVirtualSensorData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SendJoystickVirtualSensorData(nint joystick, SdlSensorType type, ulong sensor_timestamp, nint data, int num_values);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetJoystickProperties(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickName(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickPath(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPlayerIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetJoystickPlayerIndex(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickPlayerIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickPlayerIndex(nint joystick, int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickGUID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickGUID(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickVendor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickVendor(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProduct")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickProduct(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickProductVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickProductVersion(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickFirmwareVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetJoystickFirmwareVersion(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickSerial")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetJoystickSerial(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlJoystickType GetJoystickType(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickGUIDInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GetJoystickGUIDInfo(nint guid, nint vendor, nint product, nint version, nint crc16);

    [LibraryImport(Lib, EntryPoint = "SDL_JoystickConnected")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool JoystickConnected(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetJoystickID(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickAxes")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumJoystickAxes(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickBalls")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumJoystickBalls(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickHats")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumJoystickHats(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumJoystickButtons")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumJoystickButtons(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickEventsEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetJoystickEventsEnabled(CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_JoystickEventsEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool JoystickEventsEnabled();

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateJoysticks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UpdateJoysticks();

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickAxis")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial short GetJoystickAxis(nint joystick, int axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickAxisInitialState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetJoystickAxisInitialState(nint joystick, int axis, nint state);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickBall")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetJoystickBall(nint joystick, int ball, nint dx, nint dy);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickHat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial byte GetJoystickHat(nint joystick, int hat);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickButton")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetJoystickButton(nint joystick, int button);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RumbleJoystick(nint joystick, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleJoystickTriggers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RumbleJoystickTriggers(nint joystick, ushort left_rumble, ushort right_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_SetJoystickLED")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetJoystickLED(nint joystick, byte red, byte green, byte blue);

    [LibraryImport(Lib, EntryPoint = "SDL_SendJoystickEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SendJoystickEffect(nint joystick, nint data, int size);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CloseJoystick(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickConnectionState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlJoystickConnectionState GetJoystickConnectionState(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_GetJoystickPowerInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlPowerState GetJoystickPowerInfo(nint joystick, nint percent);

    [LibraryImport(Lib, EntryPoint = "SDL_AddGamepadMapping")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int AddGamepadMapping(nint mapping);

    [LibraryImport(Lib, EntryPoint = "SDL_AddGamepadMappingsFromIO")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int AddGamepadMappingsFromIO(nint src, CBool closeio);

    [LibraryImport(Lib, EntryPoint = "SDL_AddGamepadMappingsFromFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int AddGamepadMappingsFromFile(nint file);

    [LibraryImport(Lib, EntryPoint = "SDL_ReloadGamepadMappings")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReloadGamepadMappings();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMappings")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadMappings(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMappingForGUID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadMappingForGUID(nint guid);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMapping")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadMapping(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadMapping")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetGamepadMapping(uint instance_id, nint mapping);

    [LibraryImport(Lib, EntryPoint = "SDL_HasGamepad")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasGamepad();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepads")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepads(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_IsGamepad")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool IsGamepad(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadNameForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetGamepadNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPathForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadPathForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPlayerIndexForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetGamepadPlayerIndexForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadGUIDForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadGUIDForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadVendorForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadVendorForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProductForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadProductForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProductVersionForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadProductVersionForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadTypeForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadType GetGamepadTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRealGamepadTypeForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadType GetRealGamepadTypeForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadMappingForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadMappingForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenGamepad")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenGamepad(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadFromID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadFromPlayerIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadFromPlayerIndex(int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetGamepadProperties(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetGamepadID(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetGamepadName(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadPath(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadType GetGamepadType(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRealGamepadType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadType GetRealGamepadType(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPlayerIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetGamepadPlayerIndex(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadPlayerIndex")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetGamepadPlayerIndex(nint gamepad, int player_index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadVendor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadVendor(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProduct")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadProduct(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadProductVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadProductVersion(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadFirmwareVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetGamepadFirmwareVersion(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSerial")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadSerial(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSteamHandle")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetGamepadSteamHandle(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadConnectionState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlJoystickConnectionState GetGamepadConnectionState(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadPowerInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlPowerState GetGamepadPowerInfo(nint gamepad, nint percent);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadConnected")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GamepadConnected(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadJoystick(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadEventsEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGamepadEventsEnabled(CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadEventsEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GamepadEventsEnabled();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadBindings")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadBindings(nint gamepad, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateGamepads")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UpdateGamepads();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadTypeFromString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadType GetGamepadTypeFromString(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadStringForType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadStringForType(SdlGamepadType type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAxisFromString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadAxis GetGamepadAxisFromString(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadStringForAxis")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadStringForAxis(SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadHasAxis")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GamepadHasAxis(nint gamepad, SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAxis")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial short GetGamepadAxis(nint gamepad, SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButtonFromString")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadButton GetGamepadButtonFromString(nint str);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadStringForButton")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadStringForButton(SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadHasButton")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GamepadHasButton(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButton")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetGamepadButton(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButtonLabelForType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadButtonLabel GetGamepadButtonLabelForType(SdlGamepadType type, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadButtonLabel")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGamepadButtonLabel GetGamepadButtonLabel(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumGamepadTouchpads")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumGamepadTouchpads(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumGamepadTouchpadFingers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumGamepadTouchpadFingers(nint gamepad, int touchpad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadTouchpadFinger")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetGamepadTouchpadFinger(nint gamepad, int touchpad, int finger, nint down, nint x, nint y, nint pressure);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadHasSensor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GamepadHasSensor(nint gamepad, SdlSensorType type);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadSensorEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetGamepadSensorEnabled(nint gamepad, SdlSensorType type, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_GamepadSensorEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GamepadSensorEnabled(nint gamepad, SdlSensorType type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSensorDataRate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial float GetGamepadSensorDataRate(nint gamepad, SdlSensorType type);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadSensorData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetGamepadSensorData(nint gamepad, SdlSensorType type, nint data, int num_values);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleGamepad")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RumbleGamepad(nint gamepad, ushort low_frequency_rumble, ushort high_frequency_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_RumbleGamepadTriggers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RumbleGamepadTriggers(nint gamepad, ushort left_rumble, ushort right_rumble, uint duration_ms);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGamepadLED")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetGamepadLED(nint gamepad, byte red, byte green, byte blue);

    [LibraryImport(Lib, EntryPoint = "SDL_SendGamepadEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SendGamepadEffect(nint gamepad, nint data, int size);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseGamepad")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CloseGamepad(nint gamepad);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAppleSFSymbolsNameForButton")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadAppleSFSymbolsNameForButton(nint gamepad, SdlGamepadButton button);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGamepadAppleSFSymbolsNameForAxis")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetGamepadAppleSFSymbolsNameForAxis(nint gamepad, SdlGamepadAxis axis);

    [LibraryImport(Lib, EntryPoint = "SDL_HasKeyboard")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasKeyboard();

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboards")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetKeyboards(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboardNameForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetKeyboardNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboardFocus")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetKeyboardFocus();

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyboardState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetKeyboardState(nint numkeys);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetKeyboard")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ResetKeyboard();

    [LibraryImport(Lib, EntryPoint = "SDL_GetModState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ushort GetModState();

    [LibraryImport(Lib, EntryPoint = "SDL_SetModState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetModState(ushort modstate);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyFromScancode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetKeyFromScancode(SdlScancode scancode, ushort modstate, CBool key_event);

    [LibraryImport(Lib, EntryPoint = "SDL_GetScancodeFromKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlScancode GetScancodeFromKey(uint key, nint modstate);

    [LibraryImport(Lib, EntryPoint = "SDL_SetScancodeName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetScancodeName(SdlScancode scancode, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetScancodeName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetScancodeName(SdlScancode scancode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetScancodeFromName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlScancode GetScancodeFromName(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetKeyName(uint key);

    [LibraryImport(Lib, EntryPoint = "SDL_GetKeyFromName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetKeyFromName(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_StartTextInput")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StartTextInput(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_StartTextInputWithProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StartTextInputWithProperties(nint window, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_TextInputActive")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TextInputActive(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_StopTextInput")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StopTextInput(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_ClearComposition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClearComposition(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextInputArea")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextInputArea(nint window, nint rect, int cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextInputArea")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextInputArea(nint window, nint rect, nint cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_HasScreenKeyboardSupport")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasScreenKeyboardSupport();

    [LibraryImport(Lib, EntryPoint = "SDL_ScreenKeyboardShown")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ScreenKeyboardShown(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_HasMouse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasMouse();

    [LibraryImport(Lib, EntryPoint = "SDL_GetMice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetMice(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMouseNameForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetMouseNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMouseFocus")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetMouseFocus();

    [LibraryImport(Lib, EntryPoint = "SDL_GetMouseState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetMouseState(nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGlobalMouseState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetGlobalMouseState(nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRelativeMouseState")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetRelativeMouseState(nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_WarpMouseInWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void WarpMouseInWindow(nint window, float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_WarpMouseGlobal")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WarpMouseGlobal(float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_SetWindowRelativeMouseMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetWindowRelativeMouseMode(nint window, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowRelativeMouseMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetWindowRelativeMouseMode(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_CaptureMouse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CaptureMouse(CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateCursor(nint data, nint mask, int w, int h, int hot_x, int hot_y);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateColorCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateColorCursor(nint surface, int hot_x, int hot_y);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSystemCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateSystemCursor(SdlSystemCursor id);

    [LibraryImport(Lib, EntryPoint = "SDL_SetCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetCursor(nint cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDefaultCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetDefaultCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyCursor(nint cursor);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShowCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_HideCursor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HideCursor();

    [LibraryImport(Lib, EntryPoint = "SDL_CursorVisible")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CursorVisible();

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchDevices")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetTouchDevices(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchDeviceName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetTouchDeviceName(ulong touchID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchDeviceType")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlTouchDeviceType GetTouchDeviceType(ulong touchID);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTouchFingers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetTouchFingers(ulong touchID, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_PumpEvents")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PumpEvents();

    [LibraryImport(Lib, EntryPoint = "SDL_PeepEvents")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int PeepEvents(nint events, int numevents, SdlEventAction action, uint minType, uint maxType);

    [LibraryImport(Lib, EntryPoint = "SDL_HasEvent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasEvent(uint type);

    [LibraryImport(Lib, EntryPoint = "SDL_HasEvents")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HasEvents(uint minType, uint maxType);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushEvent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void FlushEvent(uint type);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushEvents")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void FlushEvents(uint minType, uint maxType);

    [LibraryImport(Lib, EntryPoint = "SDL_PollEvent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PollEvent(out SdlEvent @event);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitEvent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitEvent(nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitEventTimeout")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitEventTimeout(nint @event, int timeoutMS);

    [LibraryImport(Lib, EntryPoint = "SDL_PushEvent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PushEvent(nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_SetEventFilter")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetEventFilter(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_GetEventFilter")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetEventFilter(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_AddEventWatch")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool AddEventWatch(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveEventWatch")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void RemoveEventWatch(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_FilterEvents")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void FilterEvents(nint filter, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetEventEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetEventEnabled(uint type, CBool enabled);

    [LibraryImport(Lib, EntryPoint = "SDL_EventEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool EventEnabled(uint type);

    [LibraryImport(Lib, EntryPoint = "SDL_RegisterEvents")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint RegisterEvents(int numevents);

    [LibraryImport(Lib, EntryPoint = "SDL_GetWindowFromEvent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetWindowFromEvent(in SdlEvent @event);

    [LibraryImport(Lib, EntryPoint = "SDL_GetBasePath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetBasePath();

    [LibraryImport(Lib, EntryPoint = "SDL_GetPrefPath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPrefPath(nint org, nint app);

    [LibraryImport(Lib, EntryPoint = "SDL_GetUserFolder")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetUserFolder(SdlFolder folder);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateDirectory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CreateDirectory(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_EnumerateDirectory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool EnumerateDirectory(nint path, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemovePath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RemovePath(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_RenamePath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenamePath(nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CopyFile(nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPathInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetPathInfo(nint path, nint info);

    [LibraryImport(Lib, EntryPoint = "SDL_GlobDirectory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GlobDirectory(nint path, nint pattern, uint flags, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUSupportsShaderFormats")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GpuSupportsShaderFormats(uint format_flags, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUSupportsProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GpuSupportsProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuDevice(uint format_flags, CBool debug_mode, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUDeviceWithProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuDeviceWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyGPUDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyGpuDevice(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumGPUDrivers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumGpuDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetGpuDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUDeviceDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CString GetGpuDeviceDriver(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUShaderFormats")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetGpuShaderFormats(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUComputePipeline")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuComputePipeline(nint device, in SdlGpuComputePipelineCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUGraphicsPipeline")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuGraphicsPipeline(nint device, in SdlGpuGraphicsPipelineCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUSampler")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuSampler(nint device, in SdlGpuSamplerCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUShader")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuShader(nint device, in SdlGpuShaderCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuTexture(nint device, in SdlGpuTextureCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuBuffer(nint device, in SdlGpuBufferCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateGPUTransferBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateGpuTransferBuffer(nint device, in SdlGpuTransferBufferCreateInfo createinfo);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUBufferName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGpuBufferName(nint device, nint buffer, nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUTextureName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGpuTextureName(nint device, nint texture, nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_InsertGPUDebugLabel")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void InsertGpuDebugLabel(nint command_buffer, nint text);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUDebugGroup")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PushGpuDebugGroup(nint command_buffer, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_PopGPUDebugGroup")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PopGpuDebugGroup(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuTexture(nint device, nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUSampler")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuSampler(nint device, nint sampler);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuBuffer(nint device, nint buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUTransferBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuTransferBuffer(nint device, nint transfer_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUComputePipeline")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuComputePipeline(nint device, nint compute_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUShader")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuShader(nint device, nint shader);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUGraphicsPipeline")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuGraphicsPipeline(nint device, nint graphics_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_AcquireGPUCommandBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint AcquireGpuCommandBuffer(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUVertexUniformData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PushGpuVertexUniformData(nint command_buffer, uint slot_index, nint data, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUFragmentUniformData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PushGpuFragmentUniformData(nint command_buffer, uint slot_index, nint data, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_PushGPUComputeUniformData")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void PushGpuComputeUniformData(nint command_buffer, uint slot_index, nint data, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_BeginGPURenderPass")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint BeginGpuRenderPass(nint command_buffer, in SdlGpuColorTargetInfo color_target_infos, uint num_color_targets, nint depth_stencil_target_info);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUGraphicsPipeline")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuGraphicsPipeline(nint render_pass, nint graphics_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUViewport")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGpuViewport(nint render_pass, nint viewport);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUScissor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGpuScissor(nint render_pass, nint scissor);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUBlendConstants")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGpuBlendConstants(nint render_pass, nint blend_constants);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUStencilReference")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetGpuStencilReference(nint render_pass, byte reference);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexBuffers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuVertexBuffers(nint render_pass, uint first_slot, in SdlGpuBufferBinding bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUIndexBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuIndexBuffer(nint render_pass, in SdlGpuBufferBinding binding, SdlGpuIndexElementSize index_element_size);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexSamplers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuVertexSamplers(nint render_pass, uint first_slot, nint texture_sampler_bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexStorageTextures")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuVertexStorageTextures(nint render_pass, uint first_slot, nint storage_textures, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUVertexStorageBuffers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuVertexStorageBuffers(nint render_pass, uint first_slot, nint storage_buffers, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUFragmentSamplers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuFragmentSamplers(nint render_pass, uint first_slot, in SdlGpuTextureSamplerBinding texture_sampler_bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUFragmentStorageTextures")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuFragmentStorageTextures(nint render_pass, uint first_slot, nint storage_textures, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUFragmentStorageBuffers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuFragmentStorageBuffers(nint render_pass, uint first_slot, nint storage_buffers, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUIndexedPrimitives")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DrawGpuIndexedPrimitives(nint render_pass, uint num_indices, uint num_instances, uint first_index, int vertex_offset, uint first_instance);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUPrimitives")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DrawGpuPrimitives(nint render_pass, uint num_vertices, uint num_instances, uint first_vertex, uint first_instance);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUPrimitivesIndirect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DrawGpuPrimitivesIndirect(nint render_pass, nint buffer, uint offset, uint draw_count);

    [LibraryImport(Lib, EntryPoint = "SDL_DrawGPUIndexedPrimitivesIndirect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DrawGpuIndexedPrimitivesIndirect(nint render_pass, nint buffer, uint offset, uint draw_count);

    [LibraryImport(Lib, EntryPoint = "SDL_EndGPURenderPass")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EndGpuRenderPass(nint render_pass);

    [LibraryImport(Lib, EntryPoint = "SDL_BeginGPUComputePass")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint BeginGpuComputePass(nint command_buffer, nint storage_texture_bindings, uint num_storage_texture_bindings, nint storage_buffer_bindings, uint num_storage_buffer_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputePipeline")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuComputePipeline(nint compute_pass, nint compute_pipeline);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputeSamplers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuComputeSamplers(nint compute_pass, uint first_slot, nint texture_sampler_bindings, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputeStorageTextures")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuComputeStorageTextures(nint compute_pass, uint first_slot, nint storage_textures, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_BindGPUComputeStorageBuffers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BindGpuComputeStorageBuffers(nint compute_pass, uint first_slot, nint storage_buffers, uint num_bindings);

    [LibraryImport(Lib, EntryPoint = "SDL_DispatchGPUCompute")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DispatchGpuCompute(nint compute_pass, uint groupcount_x, uint groupcount_y, uint groupcount_z);

    [LibraryImport(Lib, EntryPoint = "SDL_DispatchGPUComputeIndirect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DispatchGpuComputeIndirect(nint compute_pass, nint buffer, uint offset);

    [LibraryImport(Lib, EntryPoint = "SDL_EndGPUComputePass")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EndGpuComputePass(nint compute_pass);

    [LibraryImport(Lib, EntryPoint = "SDL_MapGPUTransferBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint MapGpuTransferBuffer(nint device, nint transfer_buffer, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_UnmapGPUTransferBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnmapGpuTransferBuffer(nint device, nint transfer_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_BeginGPUCopyPass")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint BeginGpuCopyPass(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_UploadToGPUTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UploadToGpuTexture(nint copy_pass, in SdlGpuTextureTransferInfo source, in SdlGpuTextureRegion destination, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_UploadToGPUBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UploadToGpuBuffer(nint copy_pass, in SdlGpuTransferBufferLocation source, in SdlGpuBufferRegion destination, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyGPUTextureToTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CopyGpuTextureToTexture(nint copy_pass, nint source, nint destination, uint w, uint h, uint d, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyGPUBufferToBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CopyGpuBufferToBuffer(nint copy_pass, nint source, nint destination, uint size, CBool cycle);

    [LibraryImport(Lib, EntryPoint = "SDL_DownloadFromGPUTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DownloadFromGpuTexture(nint copy_pass, nint source, nint destination);

    [LibraryImport(Lib, EntryPoint = "SDL_DownloadFromGPUBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DownloadFromGpuBuffer(nint copy_pass, nint source, nint destination);

    [LibraryImport(Lib, EntryPoint = "SDL_EndGPUCopyPass")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void EndGpuCopyPass(nint copy_pass);

    [LibraryImport(Lib, EntryPoint = "SDL_GenerateMipmapsForGPUTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GenerateMipmapsForGpuTexture(nint command_buffer, nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_BlitGPUTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void BlitGpuTexture(nint command_buffer, nint info);

    [LibraryImport(Lib, EntryPoint = "SDL_WindowSupportsGPUSwapchainComposition")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WindowSupportsGpuSwapchainComposition(nint device, nint window, SdlGpuSwapchainComposition swapchain_composition);

    [LibraryImport(Lib, EntryPoint = "SDL_WindowSupportsGPUPresentMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WindowSupportsGpuPresentMode(nint device, nint window, SdlGpuPresentMode present_mode);

    [LibraryImport(Lib, EntryPoint = "SDL_ClaimWindowForGPUDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ClaimWindowForGpuDevice(nint device, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseWindowFromGPUDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseWindowFromGpuDevice(nint device, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_SetGPUSwapchainParameters")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetGpuSwapchainParameters(nint device, nint window, SdlGpuSwapchainComposition swapchain_composition, SdlGpuPresentMode present_mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetGPUSwapchainTextureFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlGpuTextureFormat GetGpuSwapchainTextureFormat(nint device, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_AcquireGPUSwapchainTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool AcquireGpuSwapchainTexture(nint command_buffer, nint window, nint swapchain_texture, nint swapchain_texture_width, nint swapchain_texture_height);

    [LibraryImport(Lib, EntryPoint = "SDL_SubmitGPUCommandBuffer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SubmitGpuCommandBuffer(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_SubmitGPUCommandBufferAndAcquireFence")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint SubmitGpuCommandBufferAndAcquireFence(nint command_buffer);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitForGPUIdle")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitForGpuIdle(nint device);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitForGPUFences")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitForGpuFences(nint device, CBool wait_all, nint fences, uint num_fences);

    [LibraryImport(Lib, EntryPoint = "SDL_QueryGPUFence")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool QueryGpuFence(nint device, nint fence);

    [LibraryImport(Lib, EntryPoint = "SDL_ReleaseGPUFence")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ReleaseGpuFence(nint device, nint fence);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUTextureFormatTexelBlockSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GpuTextureFormatTexelBlockSize(SdlGpuTextureFormat format);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUTextureSupportsFormat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GpuTextureSupportsFormat(nint device, SdlGpuTextureFormat format, SdlGpuTextureType type, uint usage);

    [LibraryImport(Lib, EntryPoint = "SDL_GPUTextureSupportsSampleCount")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GpuTextureSupportsSampleCount(nint device, SdlGpuTextureFormat format, SdlGpuSampleCount sample_count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHaptics")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetHaptics(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticNameForID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetHapticNameForID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenHaptic")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenHaptic(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticFromID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetHapticFromID(uint instance_id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticID")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetHapticID(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetHapticName(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_IsMouseHaptic")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool IsMouseHaptic();

    [LibraryImport(Lib, EntryPoint = "SDL_OpenHapticFromMouse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenHapticFromMouse();

    [LibraryImport(Lib, EntryPoint = "SDL_IsJoystickHaptic")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool IsJoystickHaptic(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenHapticFromJoystick")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenHapticFromJoystick(nint joystick);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseHaptic")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void CloseHaptic(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMaxHapticEffects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetMaxHapticEffects(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetMaxHapticEffectsPlaying")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetMaxHapticEffectsPlaying(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticFeatures")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetHapticFeatures(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumHapticAxes")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumHapticAxes(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_HapticEffectSupported")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HapticEffectSupported(nint haptic, nint effect);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateHapticEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int CreateHapticEffect(nint haptic, nint effect);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateHapticEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UpdateHapticEffect(nint haptic, int effect, nint data);

    [LibraryImport(Lib, EntryPoint = "SDL_RunHapticEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RunHapticEffect(nint haptic, int effect, uint iterations);

    [LibraryImport(Lib, EntryPoint = "SDL_StopHapticEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StopHapticEffect(nint haptic, int effect);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyHapticEffect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyHapticEffect(nint haptic, int effect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHapticEffectStatus")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetHapticEffectStatus(nint haptic, int effect);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHapticGain")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetHapticGain(nint haptic, int gain);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHapticAutocenter")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetHapticAutocenter(nint haptic, int autocenter);

    [LibraryImport(Lib, EntryPoint = "SDL_PauseHaptic")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PauseHaptic(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_ResumeHaptic")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ResumeHaptic(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_StopHapticEffects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StopHapticEffects(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_HapticRumbleSupported")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool HapticRumbleSupported(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_InitHapticRumble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool InitHapticRumble(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_PlayHapticRumble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool PlayHapticRumble(nint haptic, float strength, uint length);

    [LibraryImport(Lib, EntryPoint = "SDL_StopHapticRumble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StopHapticRumble(nint haptic);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_init")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidInit();

    [LibraryImport(Lib, EntryPoint = "SDL_hid_exit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidExit();

    [LibraryImport(Lib, EntryPoint = "SDL_hid_device_change_count")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint HidDeviceChangeCount();

    [LibraryImport(Lib, EntryPoint = "SDL_hid_enumerate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint HidEnumerate(ushort vendor_id, ushort product_id);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_free_enumeration")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void HidFreeEnumeration(nint devs);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_open")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint HidOpen(ushort vendor_id, ushort product_id, nint serial_number);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_open_path")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint HidOpenPath(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_write")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidWrite(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_read_timeout")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidReadTimeout(nint dev, nint data, nuint length, int milliseconds);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_read")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidRead(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_set_nonblocking")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidSetNonblocking(nint dev, int nonblock);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_send_feature_report")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidSendFeatureReport(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_feature_report")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetFeatureReport(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_input_report")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetInputReport(nint dev, nint data, nuint length);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_close")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidClose(nint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_manufacturer_string")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetManufacturerString(nint dev, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_product_string")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetProductString(nint dev, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_serial_number_string")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetSerialNumberString(nint dev, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_indexed_string")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetIndexedString(nint dev, int string_index, nint @string, nuint maxlen);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_device_info")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint HidGetDeviceInfo(nint dev);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_get_report_descriptor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int HidGetReportDescriptor(nint dev, nint buf, nuint buf_size);

    [LibraryImport(Lib, EntryPoint = "SDL_hid_ble_scan")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void HidBleScan(CBool active);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHintWithPriority")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetHintWithPriority(nint name, nint value, SdlHintPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_SetHint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetHint(nint name, nint value);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetHint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ResetHint(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetHints")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ResetHints();

    [LibraryImport(Lib, EntryPoint = "SDL_GetHint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetHint(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_GetHintBoolean")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetHintBoolean(nint name, CBool default_value);

    [LibraryImport(Lib, EntryPoint = "SDL_AddHintCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool AddHintCallback(nint name, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveHintCallback")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void RemoveHintCallback(nint name, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_Init")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool Init(SdlInit flags);

    [LibraryImport(Lib, EntryPoint = "SDL_InitSubSystem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool InitSubSystem(uint flags);

    [LibraryImport(Lib, EntryPoint = "SDL_QuitSubSystem")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void QuitSubSystem(uint flags);

    [LibraryImport(Lib, EntryPoint = "SDL_WasInit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint WasInit(uint flags);

    [LibraryImport(Lib, EntryPoint = "SDL_Quit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Quit();

    [LibraryImport(Lib, EntryPoint = "SDL_SetAppMetadata", StringMarshalling = StringMarshalling.Utf8)]
    public static partial CBool SetAppMetadata(string? appname = null, string? appversion = null, string? appidentifier = null);

    [LibraryImport(Lib, EntryPoint = "SDL_SetAppMetadataProperty", StringMarshalling = StringMarshalling.Utf8)]
    public static partial CBool SetAppMetadataProperty(string? name, string? value);

    [LibraryImport(Lib, EntryPoint = "SDL_GetAppMetadataProperty")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetAppMetadataProperty(nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadObject")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadObject(nint sofile);

    [LibraryImport(Lib, EntryPoint = "SDL_LoadFunction")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint LoadFunction(nint handle, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_UnloadObject")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnloadObject(nint handle);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPreferredLocales")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPreferredLocales(nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogPriorities")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLogPriorities(SdlLogPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogPriority")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLogPriority(int category, SdlLogPriority priority);

    [LibraryImport(Lib, EntryPoint = "SDL_GetLogPriority")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SdlLogPriority GetLogPriority(int category);

    [LibraryImport(Lib, EntryPoint = "SDL_ResetLogPriorities")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void ResetLogPriorities();

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogPriorityPrefix")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetLogPriorityPrefix(SdlLogPriority priority, nint prefix);

    [LibraryImport(Lib, EntryPoint = "SDL_Log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Log(nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogTrace")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogTrace(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogVerbose")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogVerbose(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogDebug")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogDebug(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogInfo(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogWarn")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogWarn(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogError")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogError(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogCritical")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogCritical(int category, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_LogMessage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void LogMessage(int category, SdlLogPriority priority, nint fmt);

    [LibraryImport(Lib, EntryPoint = "SDL_GetLogOutputFunction")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void GetLogOutputFunction(nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLogOutputFunction")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetLogOutputFunction(nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowMessageBox")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShowMessageBox(nint messageboxdata, nint buttonid);

    [LibraryImport(Lib, EntryPoint = "SDL_ShowSimpleMessageBox")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ShowSimpleMessageBox(uint flags, nint title, nint message, nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_Metal_CreateView")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint MetalCreateView(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_Metal_DestroyView")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void MetalDestroyView(nint view);

    [LibraryImport(Lib, EntryPoint = "SDL_Metal_GetLayer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint MetalGetLayer(nint view);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenURL")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool OpenURL(nint url);

    [LibraryImport(Lib, EntryPoint = "SDL_GetPlatform")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetPlatform();

    [LibraryImport(Lib, EntryPoint = "SDL_CreateProcess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateProcess(nint args, CBool pipe_stdio);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateProcessWithProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateProcessWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_GetProcessProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetProcessProperties(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadProcess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint ReadProcess(nint process, nint datasize, nint exitcode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetProcessInput")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetProcessInput(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_GetProcessOutput")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetProcessOutput(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_KillProcess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool KillProcess(nint process, CBool force);

    [LibraryImport(Lib, EntryPoint = "SDL_WaitProcess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitProcess(nint process, CBool block, nint exitcode);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyProcess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyProcess(nint process);

    [LibraryImport(Lib, EntryPoint = "SDL_GetNumRenderDrivers")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetNumRenderDrivers();

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDriver")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRenderDriver(int index);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateWindowAndRenderer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CreateWindowAndRenderer(nint title, int width, int height, ulong window_flags, nint window, nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateRenderer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateRenderer(nint window, nint name);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateRendererWithProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateRendererWithProperties(uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateSoftwareRenderer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateSoftwareRenderer(nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRenderer(nint window);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRenderWindow(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRendererName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRendererName(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRendererProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetRendererProperties(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderOutputSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderOutputSize(nint renderer, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentRenderOutputSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetCurrentRenderOutputSize(nint renderer, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateTexture(nint renderer, SdlPixelFormat format, SdlTextureAccess access, int w, int h);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateTextureFromSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateTextureFromSurface(nint renderer, nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateTextureWithProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint CreateTextureWithProperties(nint renderer, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureProperties")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint GetTextureProperties(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRendererFromTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRendererFromTexture(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureSize(nint texture, nint w, nint h);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureColorMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextureColorMod(nint texture, byte r, byte g, byte b);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureColorModFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextureColorModFloat(nint texture, float r, float g, float b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureColorMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureColorMod(nint texture, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureColorModFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureColorModFloat(nint texture, nint r, nint g, nint b);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureAlphaMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextureAlphaMod(nint texture, byte alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureAlphaModFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextureAlphaModFloat(nint texture, float alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureAlphaMod")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureAlphaMod(nint texture, nint alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureAlphaModFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureAlphaModFloat(nint texture, nint alpha);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextureBlendMode(nint texture, uint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureBlendMode(nint texture, nint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_SetTextureScaleMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetTextureScaleMode(nint texture, SdlScaleMode scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTextureScaleMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetTextureScaleMode(nint texture, nint scaleMode);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UpdateTexture(nint texture, nint rect, nint pixels, int pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateYUVTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UpdateYUVTexture(nint texture, nint rect, nint Yplane, int Ypitch, nint Uplane, int Upitch, nint Vplane, int Vpitch);

    [LibraryImport(Lib, EntryPoint = "SDL_UpdateNVTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool UpdateNVTexture(nint texture, nint rect, nint Yplane, int Ypitch, nint UVplane, int UVpitch);

    [LibraryImport(Lib, EntryPoint = "SDL_LockTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LockTexture(nint texture, nint rect, nint pixels, nint pitch);

    [LibraryImport(Lib, EntryPoint = "SDL_LockTextureToSurface")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool LockTextureToSurface(nint texture, nint rect, nint surface);

    [LibraryImport(Lib, EntryPoint = "SDL_UnlockTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void UnlockTexture(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderTarget")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderTarget(nint renderer, nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderTarget")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRenderTarget(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderLogicalPresentation")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderLogicalPresentation(nint renderer, int w, int h, SdlRendererLogicalPresentation mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderLogicalPresentation")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderLogicalPresentation(nint renderer, nint w, nint h, nint mode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderLogicalPresentationRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderLogicalPresentationRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderCoordinatesFromWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderCoordinatesFromWindow(nint renderer, float window_x, float window_y, nint x, nint y);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderCoordinatesToWindow")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderCoordinatesToWindow(nint renderer, float x, float y, nint window_x, nint window_y);

    [LibraryImport(Lib, EntryPoint = "SDL_ConvertEventToRenderCoordinates")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ConvertEventToRenderCoordinates(nint renderer, nint @event);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderViewport")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderViewport(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderViewport")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderViewport(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderViewportSet")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderViewportSet(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderSafeArea")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderSafeArea(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderClipRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderClipRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderClipRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderClipRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderClipEnabled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderClipEnabled(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderScale(nint renderer, float scaleX, float scaleY);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderScale(nint renderer, nint scaleX, nint scaleY);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderDrawColor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderDrawColor(nint renderer, byte r, byte g, byte b, byte a);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderDrawColorFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderDrawColorFloat(nint renderer, float r, float g, float b, float a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDrawColor")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderDrawColor(nint renderer, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDrawColorFloat")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderDrawColorFloat(nint renderer, nint r, nint g, nint b, nint a);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderColorScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderColorScale(nint renderer, float scale);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderColorScale")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderColorScale(nint renderer, nint scale);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderDrawBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderDrawBlendMode(nint renderer, uint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderDrawBlendMode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderDrawBlendMode(nint renderer, nint blendMode);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderClear")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderClear(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderPoint")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderPoint(nint renderer, float x, float y);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderPoints")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderPoints(nint renderer, nint points, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderLine")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderLine(nint renderer, float x1, float y1, float x2, float y2);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderLines")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderLines(nint renderer, nint points, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderRects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderRects(nint renderer, nint rects, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderFillRect")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderFillRect(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderFillRects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderFillRects(nint renderer, nint rects, int count);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderTexture(nint renderer, nint texture, nint srcrect, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTextureRotated")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderTextureRotated(nint renderer, nint texture, nint srcrect, nint dstrect, double angle, nint center, SdlFlipMode flip);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTextureTiled")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderTextureTiled(nint renderer, nint texture, nint srcrect, float scale, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderTexture9Grid")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderTexture9Grid(nint renderer, nint texture, nint srcrect, float left_width, float right_width, float top_height, float bottom_height, float scale, nint dstrect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderGeometry")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderGeometry(nint renderer, nint texture, nint vertices, int num_vertices, nint indices, int num_indices);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderGeometryRaw")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderGeometryRaw(nint renderer, nint texture, nint xy, int xy_stride, nint color, int color_stride, nint uv, int uv_stride, int num_vertices, nint indices, int num_indices, int size_indices);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderReadPixels")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint RenderReadPixels(nint renderer, nint rect);

    [LibraryImport(Lib, EntryPoint = "SDL_RenderPresent")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenderPresent(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyTexture(nint texture);

    [LibraryImport(Lib, EntryPoint = "SDL_DestroyRenderer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DestroyRenderer(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_FlushRenderer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool FlushRenderer(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderMetalLayer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRenderMetalLayer(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderMetalCommandEncoder")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRenderMetalCommandEncoder(nint renderer);

    [LibraryImport(Lib, EntryPoint = "SDL_AddVulkanRenderSemaphores")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool AddVulkanRenderSemaphores(nint renderer, uint wait_stage_mask, long wait_semaphore, long signal_semaphore);

    [LibraryImport(Lib, EntryPoint = "SDL_SetRenderVSync")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetRenderVSync(nint renderer, int vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_GetRenderVSync")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetRenderVSync(nint renderer, nint vsync);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenTitleStorage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenTitleStorage(nint @override, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenUserStorage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenUserStorage(nint org, nint app, uint props);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenFileStorage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenFileStorage(nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_OpenStorage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint OpenStorage(nint iface, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_CloseStorage")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CloseStorage(nint storage);

    [LibraryImport(Lib, EntryPoint = "SDL_StorageReady")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool StorageReady(nint storage);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStorageFileSize")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetStorageFileSize(nint storage, nint path, nint length);

    [LibraryImport(Lib, EntryPoint = "SDL_ReadStorageFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool ReadStorageFile(nint storage, nint path, nint destination, ulong length);

    [LibraryImport(Lib, EntryPoint = "SDL_WriteStorageFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WriteStorageFile(nint storage, nint path, nint source, ulong length);

    [LibraryImport(Lib, EntryPoint = "SDL_CreateStorageDirectory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CreateStorageDirectory(nint storage, nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_EnumerateStorageDirectory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool EnumerateStorageDirectory(nint storage, nint path, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveStoragePath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RemoveStoragePath(nint storage, nint path);

    [LibraryImport(Lib, EntryPoint = "SDL_RenameStoragePath")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RenameStoragePath(nint storage, nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_CopyStorageFile")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool CopyStorageFile(nint storage, nint oldpath, nint newpath);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStoragePathInfo")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetStoragePathInfo(nint storage, nint path, nint info);

    [LibraryImport(Lib, EntryPoint = "SDL_GetStorageSpaceRemaining")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetStorageSpaceRemaining(nint storage);

    [LibraryImport(Lib, EntryPoint = "SDL_GlobStorageDirectory")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GlobStorageDirectory(nint storage, nint path, nint pattern, uint flags, nint count);

    [LibraryImport(Lib, EntryPoint = "SDL_SetX11EventHook")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SetX11EventHook(nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLinuxThreadPriority")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetLinuxThreadPriority(long threadID, int priority);

    [LibraryImport(Lib, EntryPoint = "SDL_SetLinuxThreadPriorityAndPolicy")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool SetLinuxThreadPriorityAndPolicy(long threadID, int sdlPriority, int schedPolicy);

    [LibraryImport(Lib, EntryPoint = "SDL_IsTablet")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool IsTablet();

    [LibraryImport(Lib, EntryPoint = "SDL_IsTV")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool IsTV();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationWillTerminate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void OnApplicationWillTerminate();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationDidReceiveMemoryWarning")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void OnApplicationDidReceiveMemoryWarning();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationWillEnterBackground")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void OnApplicationWillEnterBackground();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationDidEnterBackground")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void OnApplicationDidEnterBackground();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationWillEnterForeground")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void OnApplicationWillEnterForeground();

    [LibraryImport(Lib, EntryPoint = "SDL_OnApplicationDidEnterForeground")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void OnApplicationDidEnterForeground();

    [LibraryImport(Lib, EntryPoint = "SDL_GetDateTimeLocalePreferences")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetDateTimeLocalePreferences(nint dateFormat, nint timeFormat);

    [LibraryImport(Lib, EntryPoint = "SDL_GetCurrentTime")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool GetCurrentTime(nint ticks);

    [LibraryImport(Lib, EntryPoint = "SDL_TimeToDateTime")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool TimeToDateTime(long ticks, nint dt, CBool localTime);

    [LibraryImport(Lib, EntryPoint = "SDL_DateTimeToTime")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool DateTimeToTime(nint dt, nint ticks);

    [LibraryImport(Lib, EntryPoint = "SDL_TimeToWindows")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void TimeToWindows(long ticks, nint dwLowDateTime, nint dwHighDateTime);

    [LibraryImport(Lib, EntryPoint = "SDL_TimeFromWindows")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long TimeFromWindows(uint dwLowDateTime, uint dwHighDateTime);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDaysInMonth")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetDaysInMonth(int year, int month);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDayOfYear")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetDayOfYear(int year, int month, int day);

    [LibraryImport(Lib, EntryPoint = "SDL_GetDayOfWeek")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetDayOfWeek(int year, int month, int day);

    [LibraryImport(Lib, EntryPoint = "SDL_GetTicks")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetTicks();

    [LibraryImport(Lib, EntryPoint = "SDL_GetTicksNS")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetTicksNS();

    [LibraryImport(Lib, EntryPoint = "SDL_GetPerformanceCounter")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetPerformanceCounter();

    [LibraryImport(Lib, EntryPoint = "SDL_GetPerformanceFrequency")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial ulong GetPerformanceFrequency();

    [LibraryImport(Lib, EntryPoint = "SDL_Delay")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Delay(uint ms);

    [LibraryImport(Lib, EntryPoint = "SDL_DelayNS")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void DelayNS(ulong ns);

    [LibraryImport(Lib, EntryPoint = "SDL_AddTimer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint AddTimer(uint interval, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_AddTimerNS")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint AddTimerNS(ulong interval, nint callback, nint userdata);

    [LibraryImport(Lib, EntryPoint = "SDL_RemoveTimer")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool RemoveTimer(uint id);

    [LibraryImport(Lib, EntryPoint = "SDL_GetVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int GetVersion();

    [LibraryImport(Lib, EntryPoint = "SDL_GetRevision")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint GetRevision();

    [LibraryImport(Lib, EntryPoint = "SDL_WaitAndAcquireGPUSwapchainTexture")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CBool WaitAndAcquireGpuSwapchainTexture(nint commandBuffer, nint window, out nint swapchain_texture, out uint swapchain_texture_width, out uint swapchain_texture_height);
}

#pragma warning restore CA1401
