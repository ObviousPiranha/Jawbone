using Piranha.Jawbone.Net;
using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Piranha.Jawbone.Test.Native;

public class NetworkTest
{
    [Fact]
    public void SendUdp32()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = new UdpSocket32(Endpoint.Any);
        var endpointA = socketA.GetEndpoint();
        using var socketB = new UdpSocket32(Endpoint.Any);
        var endpointB = socketB.GetEndpoint();
        socketB.Send(sendBuffer, Address32.Local.OnPort(endpointA.Port));

        var length = socketA.Receive(receiveBuffer, out var origin, TimeSpan.FromSeconds(1));
        Assert.Equal(endpointB.Port, origin.Port);
        Assert.Equal(Address32.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
        Assert.True(receiveBuffer.AsSpan(0, length).SequenceEqual(sendBuffer));
    }

    [Fact]
    public void SendUdp128()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = new UdpSocket128(Endpoint.Any, false);
        var endpointA = socketA.GetEndpoint();
        using var socketB = new UdpSocket128(Endpoint.Any, false);
        var endpointB = socketB.GetEndpoint();
        socketB.Send(sendBuffer, Address128.Local.OnPort(endpointA.Port));

        var length = socketA.Receive(receiveBuffer, out var origin, TimeSpan.FromSeconds(1));
        Assert.Equal(endpointB.Port, origin.Port);
        Assert.Equal(Address128.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
        Assert.True(receiveBuffer.AsSpan(0, length).SequenceEqual(sendBuffer));
    }

    [Fact]
    public void CanSendUdp32And128WhenAllowed()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = new UdpSocket32(Endpoint.Any);
        var endpointA = socketA.GetEndpoint();
        var destinationA = Address32.Local.MapToV6().OnPort(endpointA.Port);

        using var socketB = new UdpSocket128(Endpoint.Any, true);
        var endpointB = socketB.GetEndpoint();
        var destinationB = Address32.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var lengthV6 = socketB.Receive(receiveBuffer, out var originV6, TimeSpan.FromSeconds(1));
        Assert.Equal(Address32.Local.MapToV6(), originV6.Address);
        Assert.Equal(sendBuffer.Length, lengthV6);
        Assert.True(receiveBuffer.AsSpan(0, lengthV6).SequenceEqual(sendBuffer));

        receiveBuffer.AsSpan().Clear();
        Assert.False(receiveBuffer.AsSpan(0, lengthV6).SequenceEqual(sendBuffer));

        socketB.Send(sendBuffer, destinationA);
        int lengthV4 = socketA.Receive(receiveBuffer, out var originV4);
        Assert.Equal(Address32.Local, originV4.Address);
        Assert.Equal(sendBuffer.Length, lengthV4);
        Assert.True(receiveBuffer.AsSpan(0, lengthV4).SequenceEqual(sendBuffer));
    }

    [Fact]
    public void CannotSendUdp32And128WhenDisallowed()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = new UdpSocket32(Endpoint.Any);
        var endpointA = socketA.GetEndpoint();
        var destinationA = Address32.Local.MapToV6().OnPort(endpointA.Port);

        using var socketB = new UdpSocket128(Endpoint.Any, false);
        var endpointB = socketB.GetEndpoint();
        var destinationB = Address32.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var lengthV6 = socketB.Receive(receiveBuffer, out var originV6, TimeSpan.FromSeconds(1));
        Assert.Equal(0, lengthV6);
        Assert.True(originV6.IsDefault);
    }

    [Fact]
    public void CannotBindSamePort32()
    {
        using var socketA = new UdpSocket32(Endpoint.Any);
        var endpoint = socketA.GetEndpoint();

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = new UdpSocket32(AnyAddress.OnPort(endpoint.Port));
        });
    }

    [Fact]
    public void CannotBindSamePort128()
    {
        using var socketA = new UdpSocket128(Endpoint.Any, false);
        var endpoint = socketA.GetEndpoint();

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = new UdpSocket128(AnyAddress.OnPort(endpoint.Port), false);
        });
    }
}
