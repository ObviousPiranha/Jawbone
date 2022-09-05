using System;

namespace Piranha.Jawbone.Net;

public sealed class UdpSocket : IDisposable
{
    private readonly long _handle;

    internal UdpSocket(Endpoint<Address32> endpoint)
    {
        JawboneNetworking.CreateAndBindUdpV4Socket(
            endpoint.Address.RawAddress,
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
            destination.Address.RawAddress,
            destination.RawPort);
        
        return result;
    }

    public int Receive(Span<byte> buffer, out Endpoint<Address32> origin)
    {
        var result = JawboneNetworking.ReceiveFromV4(
            _handle,
            out buffer[0],
            buffer.Length,
            out var rawAddress,
            out var rawPort);
        
        origin = new Endpoint<Address32>
        {
            Address = new Address32 { RawAddress = rawAddress },
            RawPort = rawPort
        };

        return result;
    }

    public Endpoint<Address32> GetEndpoint()
    {
        var result = JawboneNetworking.GetV4SocketName(
            _handle,
            out var rawAddress,
            out var rawPort);

        if (result != 0)
        {
            throw new SocketException("Failed to get socket name: " + result);
        }

        return new Endpoint<Address32>
        {
            Address = new Address32 { RawAddress = rawAddress },
            RawPort = rawPort
        };
    }
}
