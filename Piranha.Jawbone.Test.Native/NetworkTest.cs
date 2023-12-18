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

        using var socketA = UdpSocketV4.BindAnyIp();
        var endpointA = socketA.GetEndpoint();
        using var socketB = UdpSocketV4.CreateWithoutBinding();
        socketB.Send(sendBuffer, AddressV4.Local.OnPort(endpointA.Port));
        var endpointB = socketB.GetEndpoint();

        var length = socketA.Receive(receiveBuffer, out var origin, TimeSpan.FromSeconds(1));
        Assert.Equal(endpointB.Port, origin.Port);
        Assert.Equal(AddressV4.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
        Assert.True(receiveBuffer.AsSpan(0, length).SequenceEqual(sendBuffer));

        socketA.Send(sendBuffer, endpointB);
        length = socketB.Receive(receiveBuffer, out origin, TimeSpan.FromSeconds(1));
        Assert.Equal(endpointA.Port, origin.Port);
        Assert.Equal(AddressV4.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
    }

    [Fact]
    public void SendUdp128()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = UdpSocketV6.BindAnyIp();
        var endpointA = socketA.GetEndpoint();
        using var socketB = UdpSocketV6.CreateWithoutBinding();
        socketB.Send(sendBuffer, AddressV6.Local.OnPort(endpointA.Port));
        var endpointB = socketB.GetEndpoint();

        var length = socketA.Receive(receiveBuffer, out var origin, TimeSpan.FromSeconds(1));
        Assert.Equal(endpointB.Port, origin.Port);
        Assert.Equal(AddressV6.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
        Assert.True(receiveBuffer.AsSpan(0, length).SequenceEqual(sendBuffer));

        socketA.Send(sendBuffer, endpointB);
        length = socketB.Receive(receiveBuffer, out origin, TimeSpan.FromSeconds(1));
        Assert.Equal(endpointA.Port, origin.Port);
        Assert.Equal(AddressV6.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
    }

    [Fact]
    public void CanSendUdp32And128WhenAllowed()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = UdpSocketV4.CreateWithoutBinding();

        using var socketB = UdpSocketV6.BindAnyIp(true);
        var endpointB = socketB.GetEndpoint();
        var destinationB = AddressV4.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var endpointA = socketA.GetEndpoint();
        var destinationA = ((AddressV6)AddressV4.Local).OnPort(endpointA.Port);
        var lengthV6 = socketB.Receive(receiveBuffer, out var originV6, TimeSpan.FromSeconds(1));
        Assert.Equal((AddressV6)AddressV4.Local, originV6.Address);
        Assert.Equal(sendBuffer.Length, lengthV6);
        Assert.True(receiveBuffer.AsSpan(0, lengthV6).SequenceEqual(sendBuffer));

        receiveBuffer.AsSpan().Clear();
        Assert.False(receiveBuffer.AsSpan(0, lengthV6).SequenceEqual(sendBuffer));

        socketB.Send(sendBuffer, destinationA);
        int lengthV4 = socketA.Receive(receiveBuffer, out var originV4);
        Assert.Equal(AddressV4.Local, originV4.Address);
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

        using var socketA = UdpSocketV4.CreateWithoutBinding();
        using var socketB = UdpSocketV6.BindAnyIp();
        var endpointB = socketB.GetEndpoint();
        var destinationB = AddressV4.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var lengthV6 = socketB.Receive(receiveBuffer, out var originV6, TimeSpan.FromSeconds(1));
        Assert.Equal(0, lengthV6);
        Assert.True(originV6.IsDefault);
    }

    [Fact]
    public void CannotBindSamePort32()
    {
        using var socketA = UdpSocketV4.BindAnyIp();
        var endpointA = socketA.GetEndpoint();

        Assert.NotEqual(0, endpointA.Port.HostValue);

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = UdpSocketV4.BindAnyIp(endpointA.Port);
            _ = socketB.GetEndpoint();
        });
    }

    [Fact]
    public void CannotBindSamePort128()
    {
        using var socketA = UdpSocketV6.BindAnyIp();
        var endpoint = socketA.GetEndpoint();

        Assert.NotEqual(0, endpoint.Port.HostValue);

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = UdpSocketV6.BindAnyIp(endpoint.Port);
            _ = socketB.GetEndpoint();
        });
    }
}
