using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocket128 : IUdpSocket<Address128>
{
    private readonly long _handle;

    public bool AllowV4 { get; }

    public UdpSocket128(Endpoint<Address128> endpoint, bool allowV4)
    {
        JawboneNetworking.CreateAndBindUdpV6Socket(
            endpoint.Address,
            endpoint.NetworkOrderPort,
            Convert.ToInt32(allowV4),
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
        out Endpoint<Address128> origin)
    {
        return Receive(buffer, out origin, TimeSpan.Zero);
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
            out var rawPort,
            out var errorCode,
            milliseconds);

        SocketException.ThrowOnError(errorCode, "Error on receive.");

        origin = new Endpoint<Address128>
        {
            Address = address,
            NetworkOrderPort = rawPort
        };

        return result;
    }

    public Endpoint<Address128> GetEndpoint()
    {
        var result = JawboneNetworking.GetV6SocketName(
            _handle,
            out var address,
            out var rawPort);

        SocketException.ThrowOnError(result, "Unable to get socket name.");

        return new Endpoint<Address128>
        {
            Address = address,
            NetworkOrderPort = rawPort
        };
    }
}
