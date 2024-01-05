using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocketV6 : IUdpSocket<AddressV6>
{
    private readonly long _handle;

    public bool AllowV4 { get; }

    private UdpSocketV6(long handle, bool allowV4)
    {
        _handle = handle;
        AllowV4 = allowV4;
    }

    public void Shutdown()
    {
        var error = JawboneNetworking.ShutdownSocket(_handle);
        SocketException.ThrowOnError(error, "Unable to shutdown socket.");
    }

    public void Dispose()
    {
        JawboneNetworking.CloseSocket(_handle);
    }

    public int Send(ReadOnlySpan<byte> message, Endpoint<AddressV6> destination)
    {
        var result = JawboneNetworking.SendToV6(
            _handle,
            message[0],
            message.Length,
            destination.Address,
            destination.Port.NetworkValue,
            out var errorCode);

        SocketException.ThrowOnError(errorCode, "Unable to send data.");

        return result;
    }

    public int Receive(
        Span<byte> buffer,
        out Endpoint<AddressV6> origin,
        TimeSpan timeout)
    {
        var milliseconds = (int)(timeout.Ticks / TimeSpan.TicksPerMillisecond);
        var result = JawboneNetworking.ReceiveFromV6(
            _handle,
            out buffer[0],
            buffer.Length,
            out var address,
            out var networkOrderPort,
            out var errorCode,
            milliseconds);

        SocketException.ThrowOnError(errorCode, "Error on receive.");

        origin = new Endpoint<AddressV6>
        {
            Address = address,
            Port = new NetworkPort { NetworkValue = networkOrderPort }
        };

        return result;
    }

    public Endpoint<AddressV6> GetEndpoint()
    {
        var result = JawboneNetworking.GetV6SocketName(
            _handle,
            out var address,
            out var networkOrderPort);

        SocketException.ThrowOnError(result, "Unable to get socket name.");

        return new Endpoint<AddressV6>
        {
            Address = address.IsDefault && networkOrderPort != 0 ? AddressV6.Local : address,
            Port = new NetworkPort { NetworkValue = networkOrderPort }
        };
    }

    public static UdpSocketV6 BindAnyIp(int port, bool allowV4 = false) => BindAnyIp((NetworkPort)port, allowV4);
    public static UdpSocketV6 BindAnyIp(NetworkPort port, bool allowV4 = false) => Bind(new(default, port), allowV4);
    public static UdpSocketV6 BindAnyIp(bool allowV4 = false) => Bind(default, allowV4);
    public static UdpSocketV6 BindLocalIp(int port, bool allowV4 = false) => Bind(new(AddressV6.Local, (NetworkPort)port), allowV4);
    public static UdpSocketV6 BindLocalIp(NetworkPort port, bool allowV4 = false) => Bind(new(AddressV6.Local, port), allowV4);
    public static UdpSocketV6 BindLocalIp(bool allowV4 = false) => Bind(new(AddressV6.Local, default(NetworkPort)), allowV4);
    public static UdpSocketV6 Bind(Endpoint<AddressV6> endpoint, bool allowV4 = false)
    {
        var flags = UdpSocket.Bind | (allowV4 ? 0 : UdpSocket.IPv6Only);
        JawboneNetworking.CreateAndBindUdpV6Socket(
            endpoint.Address,
            endpoint.Port.NetworkValue,
            flags,
            out var handle,
            out var socketError,
            out var setSocketOptionError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(setSocketOptionError, "Unable to change socket option.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");

        return new(handle, allowV4);
    }

    public static UdpSocketV6 CreateWithoutBinding(bool allowV4 = false)
    {
        // https://stackoverflow.com/a/17922652
        var flags = allowV4 ? 0 : UdpSocket.IPv6Only;
        JawboneNetworking.CreateAndBindUdpV6Socket(
            default,
            default,
            flags,
            out var handle,
            out var socketError,
            out var setSocketOptionError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(setSocketOptionError, "Unable to change socket option.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");

        return new(handle, allowV4);
    }
}
