using Piranha.Jawbone.Net;
using System;

namespace Piranha.Jawbone.Test;

public class TcpTest
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(1);

    [Fact]
    public void SendAndReceiveTcpV4()
    {
        using var listener = TcpListenerV4.Listen(default, 1);
        var endpoint = listener.GetSocketName();
        using var client = TcpClientV4.Connect(AddressV4.Local.OnPort(endpoint.Port));
        using var server = listener.Accept(Timeout);
        Assert.NotNull(server);

        var message = "greetings"u8;
        var sendResult = client.Send(message);
        Assert.Equal(message.Length, sendResult);

        Span<byte> buffer = new byte[64];
        var receiveResult = server.Receive(buffer, Timeout);
        Assert.NotNull(receiveResult);
        Assert.Equal(message.Length, receiveResult);
        Assert.Equal(message, buffer[..receiveResult.Value]);
    }

    [Fact]
    public void SendAndReceiveTcpV6()
    {
        using var listener = TcpListenerV6.Listen(default, 1);
        var endpoint = listener.GetSocketName();
        using var client = TcpClientV6.Connect(AddressV6.Local.OnPort(endpoint.Port));
        using var server = listener.Accept(Timeout);
        Assert.NotNull(server);

        var message = "greetings"u8;
        var sendResult = client.Send(message);
        Assert.Equal(message.Length, sendResult);

        Span<byte> buffer = new byte[64];
        var receiveResult = server.Receive(buffer, Timeout);
        Assert.NotNull(receiveResult);
        Assert.Equal(message.Length, receiveResult);
        Assert.Equal(message, buffer[..receiveResult.Value]);
    }

    [Fact]
    public void CanSendTcpV4ToV6WhenAllowed()
    {
        var bindEndpoint = ((AddressV6)AddressV4.Local).OnAnyPort();
        using var listener = TcpListenerV6.Listen(bindEndpoint, 1, true);
        var listenerName = listener.GetSocketName();
        var endpointV4 = AddressV4.Local.OnPort(listenerName.Port);
        using var client = TcpClientV4.Connect(endpointV4);
        using var server = listener.Accept(Timeout);
        Assert.NotNull(server);

        var message = "greetings"u8;
        var sendResult = client.Send(message);
        Assert.Equal(message.Length, sendResult);

        Span<byte> buffer = new byte[64];
        var receiveResult = server.Receive(buffer, Timeout);
        Assert.NotNull(receiveResult);
        Assert.Equal(message.Length, receiveResult);
        Assert.Equal(message, buffer[..receiveResult.Value]);
    }
}
