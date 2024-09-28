using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net;

public static unsafe partial class Linux
{
    public static class Af
    {
        public const int INet = 2;
    }

    public static class Sock
    {
        public const int DGram = 2;
    }

    public static class IpProto
    {
        public const int Udp = 17;
    }

    public struct SockAddr
    {
        [InlineArray(Length)]
        public struct Data
        {
            public const int Length = 14;
            private byte _e0;
        }

        public ushort SaFamily;
        public Data SaData;
    }

    public struct SockAddrIn
    {
        [InlineArray(Length)]
        public struct Zero
        {
            public const int Length = 8;
            private byte _e0;
        }

        public ushort SinFamily;
        public ushort SinPort;
        public uint SinAddr;
        public Zero SinZero;
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct In6Addr
    {
        [FieldOffset(0)]
        public AddressV6.ArrayU8 U6Addr8;
        [FieldOffset(0)]
        public AddressV6.ArrayU16 U6Addr16;
        [FieldOffset(0)]
        public AddressV6.ArrayU32 U6Addr32;
    }

    public struct SockAddrIn6
    {
        public ushort Sin6Family;
        public ushort Sin6Port;
        public uint Sin6FlowInfo;
        public In6Addr Sin6Addr;
        public uint Sin6ScopeId;
    }

    public struct PollFd
    {
        public int Fd;
        public short Events;
        public short REvents;
    }

    public static class EventTypes
    {
        public const short PollIn = 1 << 0;
        public const short PollPri = 1 << 1;
        public const short PollOut = 1 << 2;
    }

    public const string Lib = "libc";

    [LibraryImport(Lib, EntryPoint = "socket")]
    public static partial int* ErrNoLocation();

    [LibraryImport(Lib, EntryPoint = "socket")]
    public static partial int Socket(int domain, int type, int protocol);

    [LibraryImport(Lib, EntryPoint = "bind")]
    public static partial int Bind(int sockfd, in SockAddrIn addr, uint addrlen);

    [LibraryImport(Lib, EntryPoint = "sendto")]
    public static partial nint SendTo(
        int sockfd,
        in byte buf,
        nuint len,
        int flags,
        in SockAddrIn destAddr,
        uint addrlen);

    [LibraryImport(Lib, EntryPoint = "recvfrom")]
    public static partial nint RecvFrom(
        int socket,
        out byte buffer,
        nuint length,
        int flags,
        out SockAddrIn address,
        ref uint addressLen);

    [LibraryImport(Lib, EntryPoint = "poll")]
    public static partial int Poll(ref PollFd fds, nuint nfds, int timeout);

    [LibraryImport(Lib, EntryPoint = "close")]
    public static partial int Close(int fd);

    public static unsafe int ErrNo() => *ErrNoLocation();

    public static readonly ImmutableArray<ErrorCode> ErrorCodes =
    [
        ErrorCode.None,
        new(1, "EPERM", "Operation not permitted"),
        new(2, "ENOENT", "No such file or directory"),
        new(3, "ESRCH", "No such process"),
        new(4, "EINTR", "Interrupted system call"),
        new(5, "EIO", "I/O error"),
        new(6, "ENXIO", "No such device or address"),
        new(7, "E2BIG", "Argument list too long"),
        new(8, "ENOEXEC", "Exec format error"),
        new(9, "EBADF", "Bad file number"),
        new(10, "ECHILD", "No child processes"),
        new(11, "EAGAIN", "Try again"),
        new(12, "ENOMEM", "Out of memory"),
        new(13, "EACCES", "Permission denied"),
        new(14, "EFAULT", "Bad address"),
        new(15, "ENOTBLK", "Block device required"),
        new(16, "EBUSY", "Device or resource busy"),
        new(17, "EEXIST", "File exists"),
        new(18, "EXDEV", "Cross-device link"),
        new(19, "ENODEV", "No such device"),
        new(20, "ENOTDIR", "Not a directory"),
        new(21, "EISDIR", "Is a directory"),
        new(22, "EINVAL", "Invalid argument"),
        new(23, "ENFILE", "File table overflow"),
        new(24, "EMFILE", "Too many open files"),
        new(25, "ENOTTY", "Not a typewriter"),
        new(26, "ETXTBSY", "Text file busy"),
        new(27, "EFBIG", "File too large"),
        new(28, "ENOSPC", "No space left on device"),
        new(29, "ESPIPE", "Illegal seek"),
        new(30, "EROFS", "Read-only file system"),
        new(31, "EMLINK", "Too many links"),
        new(32, "EPIPE", "Broken pipe"),
        new(33, "EDOM", "Math argument out of domain of func"),
        new(34, "ERANGE", "Math result not representable"),
        new(35, "EDEADLK", "Resource deadlock would occur"),
        new(36, "ENAMETOOLONG", "File name too long"),
        new(37, "ENOLCK", "No record locks available"),
        new(38, "ENOSYS", "Invalid system call number"),
        new(39, "ENOTEMPTY", "Directory not empty"),
        new(40, "ELOOP", "Too many symbolic links encountered"),
        new(41, "EWOULDBLOCK", "Operation would block"),
        new(42, "ENOMSG", "No message of desired type"),
        new(43, "EIDRM", "Identifier removed"),
        new(44, "ECHRNG", "Channel number out of range"),
        new(45, "EL2NSYNC", "Level 2 not synchronized"),
        new(46, "EL3HLT", "Level 3 halted"),
        new(47, "EL3RST", "Level 3 reset"),
        new(48, "ELNRNG", "Link number out of range"),
        new(49, "EUNATCH", "Protocol driver not attached"),
        new(50, "ENOCSI", "No CSI structure available"),
        new(51, "EL2HLT", "Level 2 halted"),
        new(52, "EBADE", "Invalid exchange"),
        new(53, "EBADR", "Invalid request descriptor"),
        new(54, "EXFULL", "Exchange full"),
        new(55, "ENOANO", "No anode"),
        new(56, "EBADRQC", "Invalid request code"),
        new(57, "EBADSLT", "Invalid slot"),
        new(58, "EDEADLOCK", "Resource deadlock would occur"),
        new(59, "EBFONT", "Bad font file format"),
        new(60, "ENOSTR", "Device not a stream"),
        new(61, "ENODATA", "No data available"),
        new(62, "ETIME", "Timer expired"),
        new(63, "ENOSR", "Out of streams resources"),
        new(64, "ENONET", "Machine is not on the network"),
        new(65, "ENOPKG", "Package not installed"),
        new(66, "EREMOTE", "Object is remote"),
        new(67, "ENOLINK", "Link has been severed"),
        new(68, "EADV", "Advertise error"),
        new(69, "ESRMNT", "Srmount error"),
        new(70, "ECOMM", "Communication error on send"),
        new(71, "EPROTO", "Protocol error"),
        new(72, "EMULTIHOP", "Multihop attempted"),
        new(73, "EDOTDOT", "RFS specific error"),
        new(74, "EBADMSG", "Not a data message"),
        new(75, "EOVERFLOW", "Value too large for defined data type"),
        new(76, "ENOTUNIQ", "Name not unique on network"),
        new(77, "EBADFD", "File descriptor in bad state"),
        new(78, "EREMCHG", "Remote address changed"),
        new(79, "ELIBACC", "Can not access a needed shared library"),
        new(80, "ELIBBAD", "Accessing a corrupted shared library"),
        new(81, "ELIBSCN", ".lib section in a.out corrupted"),
        new(82, "ELIBMAX", "Attempting to link in too many shared libraries"),
        new(83, "ELIBEXEC", "Cannot exec a shared library directly"),
        new(84, "EILSEQ", "Illegal byte sequence"),
        new(85, "ERESTART", "Interrupted system call should be restarted"),
        new(86, "ESTRPIPE", "Streams pipe error"),
        new(87, "EUSERS", "Too many users"),
        new(88, "ENOTSOCK", "Socket operation on non-socket"),
        new(89, "EDESTADDRREQ", "Destination address required"),
        new(90, "EMSGSIZE", "Message too long"),
        new(91, "EPROTOTYPE", "Protocol wrong type for socket"),
        new(92, "ENOPROTOOPT", "Protocol not available"),
        new(93, "EPROTONOSUPPORT", "Protocol not supported"),
        new(94, "ESOCKTNOSUPPORT", "Socket type not supported"),
        new(95, "EOPNOTSUPP", "Operation not supported on transport endpoint"),
        new(96, "EPFNOSUPPORT", "Protocol family not supported"),
        new(97, "EAFNOSUPPORT", "Address family not supported by protocol"),
        new(98, "EADDRINUSE", "Address already in use"),
        new(99, "EADDRNOTAVAIL", "Cannot assign requested address"),
        new(100, "ENETDOWN", "Network is down"),
        new(101, "ENETUNREACH", "Network is unreachable"),
        new(102, "ENETRESET", "Network dropped connection because of reset"),
        new(103, "ECONNABORTED", "Software caused connection abort"),
        new(104, "ECONNRESET", "Connection reset by peer"),
        new(105, "ENOBUFS", "No buffer space available"),
        new(106, "EISCONN", "Transport endpoint is already connected"),
        new(107, "ENOTCONN", "Transport endpoint is not connected"),
        new(108, "ESHUTDOWN", "Cannot send after transport endpoint shutdown"),
        new(109, "ETOOMANYREFS", "Too many references: cannot splice"),
        new(110, "ETIMEDOUT", "Connection timed out"),
        new(111, "ECONNREFUSED", "Connection refused"),
        new(112, "EHOSTDOWN", "Host is down"),
        new(113, "EHOSTUNREACH", "No route to host"),
        new(114, "EALREADY", "Operation already in progress"),
        new(115, "EINPROGRESS", "Operation now in progress"),
        new(116, "ESTALE", "Stale file handle"),
        new(117, "EUCLEAN", "Structure needs cleaning"),
        new(118, "ENOTNAM", "Not a XENIX named type file"),
        new(119, "ENAVAIL", "No XENIX semaphores available"),
        new(120, "EISNAM", "Is a named type file"),
        new(121, "EREMOTEIO", "Remote I/O error"),
        new(122, "EDQUOT", "Quota exceeded"),
        new(123, "ENOMEDIUM", "No medium found"),
        new(124, "EMEDIUMTYPE", "Wrong medium type"),
        new(125, "ECANCELED", "Operation Canceled"),
        new(126, "ENOKEY", "Required key not available"),
        new(127, "EKEYEXPIRED", "Key has expired"),
        new(128, "EKEYREVOKED", "Key has been revoked"),
        new(129, "EKEYREJECTED", "Key was rejected by service"),
        new(130, "EOWNERDEAD", "Owner died"),
        new(131, "ENOTRECOVERABLE", "State not recoverable")
    ];
}
