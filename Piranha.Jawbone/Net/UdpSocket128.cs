using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocket128 : IUdpSocket<Address128>
{
    private readonly long _handle;

    internal UdpSocket128(Endpoint<Address128> endpoint)
    {
        JawboneNetworking.CreateAndBindUdpV6Socket(
            endpoint.Address,
            endpoint.RawPort,
            out _handle,
            out var socketError,
            out var bindError);
        
        if (socketError != 0)
        {
            throw new SocketException("Socket creation error: " + socketError);
        }

        if (bindError != 0)
        {
            throw new SocketException("Socket bind error: " + bindError);
        }
    }

    public void Shutdown()
    {
        JawboneNetworking.ShutdownSocket(_handle);
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
            destination.RawPort);
        
        return result;
    }

    public int Receive(Span<byte> buffer, out Endpoint<Address128> origin, TimeSpan timeout)
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
        
        if (errorCode != 0)
            throw new SocketException("Error on receive: " + errorCode);
        
        origin = new Endpoint<Address128>
        {
            Address = address,
            RawPort = rawPort
        };

        return result;
    }

    public Endpoint<Address128> GetEndpoint()
    {
        var result = JawboneNetworking.GetV6SocketName(
            _handle,
            out var address,
            out var rawPort);

        if (result != 0)
        {
            throw new SocketException("Failed to get socket name: " + result);
        }

        return new Endpoint<Address128>
        {
            Address = address,
            RawPort = rawPort
        };
    }
}
