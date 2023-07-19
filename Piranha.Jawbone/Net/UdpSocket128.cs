using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocket128 : IUdpSocket<Address128>
{
    private readonly long _handle;

    public bool AllowV4 { get; }

    public UdpSocket128(Endpoint<Address128> endpoint, bool allowV4)
    {
        var flags = UdpSocket.Bind | (allowV4 ? 0 : UdpSocket.IPv6Only);
        JawboneNetworking.CreateAndBindUdpV6Socket(
            endpoint.Address,
            endpoint.NetworkOrderPort,
            flags,
            out _handle,
            out var socketError,
            out var setSocketOptionError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(setSocketOptionError, "Unable to change socket option.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");

        AllowV4 = allowV4;
    }

    public UdpSocket128(bool allowV4)
    {
        var flags = allowV4 ? 0 : UdpSocket.IPv6Only;
        JawboneNetworking.CreateAndBindUdpV6Socket(
            default,
            default,
            flags,
            out _handle,
            out var socketError,
            out var setSocketOptionError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(setSocketOptionError, "Unable to change socket option.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");

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

    public int Send(ReadOnlySpan<byte> message, Endpoint<Address128> destination)
    {
        var result = JawboneNetworking.SendToV6(
            _handle,
            message[0],
            message.Length,
            destination.Address,
            destination.NetworkOrderPort,
            out var errorCode);

        SocketException.ThrowOnError(errorCode, "Unable to send data.");

        return result;
    }

    public int Receive(
        Span<byte> buffer,
        out Endpoint<Address128> origin,
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

        origin = new Endpoint<Address128>
        {
            Address = address,
            NetworkOrderPort = networkOrderPort
        };

        return result;
    }

    public Endpoint<Address128> GetEndpoint()
    {
        var result = JawboneNetworking.GetV6SocketName(
            _handle,
            out var address,
            out var networkOrderPort);

        SocketException.ThrowOnError(result, "Unable to get socket name.");

        return new Endpoint<Address128>
        {
            Address = address.IsDefault && networkOrderPort != 0 ? Address128.Local : address,
            NetworkOrderPort = networkOrderPort
        };
    }
}
