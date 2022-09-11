using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net;

public static class JawboneNetworking
{
    private const string Library = "PiranhaNative.dll";

    static JawboneNetworking()
    {
        StartNetworking();
    }

    [DllImport(Library, EntryPoint = "jawboneStartNetworking", CallingConvention = CallingConvention.Cdecl)]
    private static extern int StartNetworking();
    [DllImport(Library, EntryPoint = "jawboneStopNetworking", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StopNetworking();
    [DllImport(Library, EntryPoint = "jawboneCreateAndBindUdpV4Socket", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CreateAndBindUdpV4Socket(
        Address32 address,
        ushort port,
        out long outSocket,
        out int outSocketError,
        out int outBindError);
    [DllImport(Library, EntryPoint = "jawboneCreateAndBindUdpV6Socket", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CreateAndBindUdpV6Socket(
        in Address128 address,
        ushort port,
        out long outSocket,
        out int outSocketError,
        out int outBindError);
    [DllImport(Library, EntryPoint = "jawboneGetV4SocketName", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetV4SocketName(
        in long inSocket,
        out Address32 outAddress,
        out ushort outPort);
    [DllImport(Library, EntryPoint = "jawboneGetV6SocketName", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetV6SocketName(
        in long inSocket,
        out Address128 outAddress,
        out ushort outPort);
    [DllImport(Library, EntryPoint = "jawboneSendToV4", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SendToV4(
        in long inSocket,
        in byte inBuffer,
        int bufferLength,
        Address32 address,
        ushort port,
        out int errorCode);
    [DllImport(Library, EntryPoint = "jawboneSendToV6", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SendToV6(
        in long inSocket,
        in byte inBuffer,
        int bufferLength,
        in Address128 address,
        ushort port,
        out int errorCode);
    [DllImport(Library, EntryPoint = "jawboneReceiveFromV4", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReceiveFromV4(
        in long inSocket,
        out byte outBuffer,
        int bufferLength,
        out Address32 outAddress,
        out ushort outPort,
        out int errorCode,
        int milliseconds);
    [DllImport(Library, EntryPoint = "jawboneReceiveFromV6", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReceiveFromV6(
        in long inSocket,
        out byte outBuffer,
        int bufferLength,
        out Address128 outAddress,
        out ushort outPort,
        out int errorCode,
        int milliseconds);
    [DllImport(Library, EntryPoint = "jawboneShutdownSocket", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ShutdownSocket(in long targetSocket);
    [DllImport(Library, EntryPoint = "jawboneCloseSocket", CallingConvention = CallingConvention.Cdecl)]
    public static extern int CloseSocket(in long closingSocket);
    [DllImport(Library, EntryPoint = "jawboneGetAddressInfo", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetAddressInfo(
        string? node,
        string? service,
        out Endpoint<Address32> resultsV4,
        int sizeV4,
        int capacityV4,
        out int countV4,
        out Endpoint<Address128> resultsV6,
        int sizeV6,
        int capacityV6,
        out int countV6);
}
