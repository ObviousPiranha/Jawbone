using System.Runtime.InteropServices;

namespace Piranha.Jawbone.Net;

public static class JawboneNetworking
{
    private const string Library = "PiranhaNative.dll";

    [DllImport(Library, EntryPoint = "jawboneStartNetworking", CallingConvention = CallingConvention.Cdecl)]
    public static extern int StartNetworking();
    [DllImport(Library, EntryPoint = "jawboneStopNetworking", CallingConvention = CallingConvention.Cdecl)]
    public static extern void StopNetworking();
    [DllImport(Library, EntryPoint = "jawboneCreateAndBindUdpV4Socket", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CreateAndBindUdpV4Socket(
        uint address,
        ushort port,
        out long outSocket,
        out int outSocketError,
        out int outBindError);
    [DllImport(Library, EntryPoint = "jawboneGetV4SocketName", CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetV4SocketName(
        in long inSocket,
        out Address32 outAddress,
        out ushort outPort);
    [DllImport(Library, EntryPoint = "jawboneSendToV4", CallingConvention = CallingConvention.Cdecl)]
    public static extern int SendToV4(
        in long inSocket,
        in byte inBuffer,
        int bufferLength,
        Address32 address,
        ushort port);
    [DllImport(Library, EntryPoint = "jawboneReceiveFromV4", CallingConvention = CallingConvention.Cdecl)]
    public static extern int ReceiveFromV4(
        in long inSocket,
        out byte outBuffer,
        int bufferLength,
        out Address32 outAddress,
        out ushort outPort);
    [DllImport(Library, EntryPoint = "jawboneCloseSocket", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CloseSocket(in long closingSocket);
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
