using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net.Mac;

static unsafe partial class Sys
{
    public const string Lib = "c";

    [LibraryImport(Lib, EntryPoint = "__error")]
    public static partial int* ErrorLocation();

    // https://man7.org/linux/man-pages/man2/socket.2.html
    // https://man7.org/linux/man-pages/man7/ipv6.7.html
    [LibraryImport(Lib, EntryPoint = "socket")]
    public static partial int Socket(int domain, int type, int protocol);

    // https://man7.org/linux/man-pages/man2/bind.2.html
    // https://man7.org/linux/man-pages/man3/bind.3p.html
    [LibraryImport(Lib, EntryPoint = "bind")]
    public static partial int BindV4(int sockfd, in SockAddrIn addr, uint addrlen);

    [LibraryImport(Lib, EntryPoint = "bind")]
    public static partial int BindV6(int sockfd, in SockAddrIn6 addr, uint addrlen);

    // https://man7.org/linux/man-pages/man2/sendto.2.html
    [LibraryImport(Lib, EntryPoint = "sendto")]
    public static partial nint SendToV4(
        int sockfd,
        in byte buf,
        nuint len,
        int flags,
        in SockAddrIn destAddr,
        uint addrlen);

    [LibraryImport(Lib, EntryPoint = "sendto")]
    public static partial nint SendToV6(
    int sockfd,
    in byte buf,
    nuint len,
    int flags,
    in SockAddrIn6 destAddr,
    uint addrlen);

    // https://man7.org/linux/man-pages/man3/recvfrom.3p.html
    [LibraryImport(Lib, EntryPoint = "recvfrom")]
    public static partial nint RecvFromV4(
        int socket,
        out byte buffer,
        nuint length,
        int flags,
        out SockAddrIn address,
        ref uint addressLen);

    [LibraryImport(Lib, EntryPoint = "recvfrom")]
    public static partial nint RecvFromV6(
        int socket,
        out byte buffer,
        nuint length,
        int flags,
        out SockAddrStorage address,
        ref uint addressLen);

    // https://man7.org/linux/man-pages/man2/recvmsg.2.html
    // https://man7.org/linux/man-pages/man2/read.2.html
    [LibraryImport(Lib, EntryPoint = "read")]
    public static partial nint Read(int fd, out byte buf, nuint length);

    // https://man7.org/linux/man-pages/man2/write.2.html
    [LibraryImport(Lib, EntryPoint = "write")]
    public static partial nint Write(int fd, in byte buf, nuint count);

    // https://man7.org/linux/man-pages/man2/getsockname.2.html
    [LibraryImport(Lib, EntryPoint = "getsockname")]
    public static partial int GetSockNameV4(int sockfd, out SockAddrIn addr, ref uint addrlen);

    [LibraryImport(Lib, EntryPoint = "getsockname")]
    public static partial int GetSockNameV6(int sockfd, out SockAddrStorage addr, ref uint addrlen);

    // https://man7.org/linux/man-pages/man2/connect.2.html
    [LibraryImport(Lib, EntryPoint = "connect")]
    public static partial int ConnectV4(int sockfd, in SockAddrIn addr, uint addrlen);

    [LibraryImport(Lib, EntryPoint = "connect")]
    public static partial int ConnectV6(int sockfd, in SockAddrIn6 addr, uint addrlen);

    // https://man7.org/linux/man-pages/man2/listen.2.html
    [LibraryImport(Lib, EntryPoint = "listen")]
    public static partial int Listen(int sockfd, int backlog);

    // https://man7.org/linux/man-pages/man2/accept.2.html
    [LibraryImport(Lib, EntryPoint = "accept")]
    public static partial int AcceptV4(int sockfd, out SockAddrIn addr, ref uint addrLen);

    [LibraryImport(Lib, EntryPoint = "accept")]
    public static partial int AcceptV6(int sockfd, out SockAddrStorage addr, ref uint addrLen);

    // https://man7.org/linux/man-pages/man2/poll.2.html
    [LibraryImport(Lib, EntryPoint = "poll")]
    public static partial int Poll(ref PollFd fds, nuint nfds, int timeout);

    // https://man7.org/linux/man-pages/man3/gai_strerror.3.html
    [LibraryImport(Lib, EntryPoint = "getaddrinfo", StringMarshalling = StringMarshalling.Utf8)]
    public static partial int GetAddrInfo(string? node, string? service, in AddrInfo hints, out AddrInfo* res);

    [LibraryImport(Lib, EntryPoint = "freeaddrinfo")]
    public static partial void FreeAddrInfo(AddrInfo* res);

    // https://man7.org/linux/man-pages/man2/setsockopt.2.html
    [LibraryImport(Lib, EntryPoint = "setsockopt")]
    public static partial int SetSockOpt(
        int socket,
        int level,
        int optionName,
        in int optionValue,
        uint optionLen);

    // https://man7.org/linux/man-pages/man2/close.2.html
    [LibraryImport(Lib, EntryPoint = "close")]
    public static partial int Close(int fd);

    public static int ErrNo() => *ErrorLocation();

    public static uint SockLen<T>() => (uint)Unsafe.SizeOf<T>();

    [DoesNotReturn]
    public static void Throw(string message)
    {
        var error = ErrNo();
        var errorCode = SocketException.GetErrorCode(error);
        var exception = new SocketException(message + " " + errorCode.ToString())
        {
            Error = error
        };

        throw exception;
    }
}
