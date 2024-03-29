using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net;

public static partial class JawboneNetworking
{
    private const int ExpectedVersion = 3;
    private const string Library = "PiranhaNative.dll";

    static JawboneNetworking()
    {
        var version = GetVersion();
        if (version != ExpectedVersion)
            throw new System.Exception($"Expected version {ExpectedVersion}; found version {version}.");
        _ = StartNetworking();
    }

    [LibraryImport(Library, EntryPoint = "jawboneGetVersion")]
    private static partial int GetVersion();
    [LibraryImport(Library, EntryPoint = "jawboneStartNetworking")]
    private static partial int StartNetworking();
    [LibraryImport(Library, EntryPoint = "jawboneStopNetworking")]
    public static partial void StopNetworking();
    [LibraryImport(Library, EntryPoint = "jawboneCreateAndBindUdpV4Socket")]
    public static partial void CreateAndBindUdpV4Socket(
        AddressV4 address,
        ushort port,
        int flags,
        out long outSocket,
        out int outSocketError,
        out int outSetSocketOptionError,
        out int outBindError);
    [LibraryImport(Library, EntryPoint = "jawboneCreateAndBindUdpV6Socket")]
    public static partial void CreateAndBindUdpV6Socket(
        in AddressV6 address,
        ushort port,
        int flags,
        out long outSocket,
        out int outSocketError,
        out int outSetSocketOptionError,
        out int outBindError);
    [LibraryImport(Library, EntryPoint = "jawboneGetV4SocketName")]
    public static partial int GetV4SocketName(
        in long inSocket,
        out AddressV4 outAddress,
        out ushort outPort);
    [LibraryImport(Library, EntryPoint = "jawboneGetV6SocketName")]
    public static partial int GetV6SocketName(
        in long inSocket,
        out AddressV6 outAddress,
        out ushort outPort);
    [LibraryImport(Library, EntryPoint = "jawboneSendToV4")]
    public static partial int SendToV4(
        in long inSocket,
        in byte inBuffer,
        int bufferLength,
        AddressV4 address,
        ushort port,
        out int errorCode);
    [LibraryImport(Library, EntryPoint = "jawboneSendToV6")]
    public static partial int SendToV6(
        in long inSocket,
        in byte inBuffer,
        int bufferLength,
        in AddressV6 address,
        ushort port,
        out int errorCode);
    [LibraryImport(Library, EntryPoint = "jawboneReceiveFromV4")]
    public static partial int ReceiveFromV4(
        in long inSocket,
        out byte outBuffer,
        int bufferLength,
        out AddressV4 outAddress,
        out ushort outPort,
        out int errorCode,
        int milliseconds);
    [LibraryImport(Library, EntryPoint = "jawboneReceiveFromV6")]
    public static partial int ReceiveFromV6(
        in long inSocket,
        out byte outBuffer,
        int bufferLength,
        out AddressV6 outAddress,
        out ushort outPort,
        out int errorCode,
        int milliseconds);
    [LibraryImport(Library, EntryPoint = "jawboneShutdownSocket")]
    public static partial int ShutdownSocket(in long targetSocket);
    [LibraryImport(Library, EntryPoint = "jawboneCloseSocket")]
    public static partial int CloseSocket(in long closingSocket);
    [LibraryImport(Library, EntryPoint = "jawboneGetAddressInfo", StringMarshalling = StringMarshalling.Utf8)]
    public static partial int GetAddressInfo(
        string? node,
        string? service,
        out Endpoint<AddressV4> resultsV4,
        int sizeV4,
        int capacityV4,
        out int countV4,
        out Endpoint<AddressV6> resultsV6,
        int sizeV6,
        int capacityV6,
        out int countV6);
}
