using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocket32 : IUdpSocket<Address32>
{
    private readonly long _handle;

    public UdpSocket32(Endpoint<Address32> endpoint)
    {
        JawboneNetworking.CreateAndBindUdpV4Socket(
            endpoint.Address,
            endpoint.RawPort,
            out _handle,
            out var socketError,
            out var bindError);

        SocketException.ThrowOnError(socketError, "Unable to create socket.");
        SocketException.ThrowOnError(bindError, "Unable to bind socket.");
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

    public int Send(ReadOnlySpan<byte> message, Endpoint<Address32> destination)
    {
        var result = JawboneNetworking.SendToV4(
            _handle,
            message[0],
            message.Length,
            destination.Address,
            destination.RawPort,
            out var errorCode);

        SocketException.ThrowOnError(errorCode, "Unable to send data.");

        return result;
    }

    public int Receive(
        Span<byte> buffer,
        out Endpoint<Address32> origin)
    {
        return Receive(buffer, out origin, TimeSpan.Zero);
    }

    public int Receive(
        Span<byte> buffer,
        out Endpoint<Address32> origin,
        TimeSpan timeout)
    {
        var milliseconds = (int)(timeout.Ticks / TimeSpan.TicksPerMillisecond);
        var result = JawboneNetworking.ReceiveFromV4(
            _handle,
            out buffer[0],
            buffer.Length,
            out var address,
            out var rawPort,
            out var errorCode,
            milliseconds);

        SocketException.ThrowOnError(errorCode, "Error on receive.");

        origin = new Endpoint<Address32>
        {
            Address = address,
            RawPort = rawPort
        };

        return result;
    }

    public Endpoint<Address32> GetEndpoint()
    {
        var result = JawboneNetworking.GetV4SocketName(
            _handle,
            out var address,
            out var rawPort);

        SocketException.ThrowOnError(result, "Unable to get socket name.");

        return new Endpoint<Address32>
        {
            Address = address,
            RawPort = rawPort
        };
    }
}
