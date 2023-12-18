using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocketV4 : IUdpSocket<AddressV4>
{
    private readonly long _handle;

    private UdpSocketV4(long handle)
    {
        _handle = handle;
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

    public int Send(ReadOnlySpan<byte> message, Endpoint<AddressV4> destination)
    {
        var result = JawboneNetworking.SendToV4(
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
        out Endpoint<AddressV4> origin,
        TimeSpan timeout)
    {
        var milliseconds = (int)(timeout.Ticks / TimeSpan.TicksPerMillisecond);
        var result = JawboneNetworking.ReceiveFromV4(
            _handle,
            out buffer[0],
            buffer.Length,
            out var address,
            out var networkOrderPort,
            out var errorCode,
            milliseconds);

        SocketException.ThrowOnError(errorCode, "Error on receive.");

        origin = new Endpoint<AddressV4>
        {
            Address = address,
            Port = new NetworkPort { NetworkValue = networkOrderPort }
        };

        return result;
    }

    public Endpoint<AddressV4> GetEndpoint()
    {
        var result = JawboneNetworking.GetV4SocketName(
            _handle,
            out var address,
            out var networkOrderPort);

        SocketException.ThrowOnError(result, "Unable to get socket name.");

        // https://learn.microsoft.com/en-us/windows/win32/api/winsock2/nf-winsock2-sendto
        // If the socket is not connected, the getsockname function can be used to
        // determine the local port number associated with the socket but the IP address
        // returned is set to the wildcard address for the given protocol (for example,
        // INADDR_ANY or "0.0.0.0" for IPv4 and IN6ADDR_ANY_INIT or "::" for IPv6).
        return new Endpoint<AddressV4>
        {
            Address = address.IsDefault && networkOrderPort != 0 ? AddressV4.Local : address,
            Port = new NetworkPort { NetworkValue = networkOrderPort }
        };
    }

    public static UdpSocketV4 BindAnyIp(int port) => BindAnyIp((NetworkPort)port);
    public static UdpSocketV4 BindAnyIp(NetworkPort port) => Bind(new(default, port));
    public static UdpSocketV4 BindAnyIp() => Bind(default);
    public static UdpSocketV4 BindLocalIp(int port) => Bind(new(AddressV4.Local, (NetworkPort)port));
    public static UdpSocketV4 BindLocalIp(NetworkPort port) => Bind(new(AddressV4.Local, port));
    public static UdpSocketV4 BindLocalIp() => Bind(new(AddressV4.Local, default));
    public static UdpSocketV4 Bind(Endpoint<AddressV4> endpoint)
    {
        JawboneNetworking.CreateAndBindUdpV4Socket(
            endpoint.Address,
            endpoint.Port.NetworkValue,
            UdpSocket.Bind,
            out var handle,
            out var socketError,
            out var setSocketOptionError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(setSocketOptionError, "Unable to change socket option.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");

        return new(handle);
    }

    public static UdpSocketV4 CreateWithoutBinding()
    {
        // https://stackoverflow.com/a/17922652
        JawboneNetworking.CreateAndBindUdpV4Socket(
            default,
            default,
            0,
            out var handle,
            out var socketError,
            out var setSocketOptionError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(setSocketOptionError, "Unable to change socket option.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");

        return new(handle);
    }
}
