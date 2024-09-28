using Piranha.Jawbone.Net;
using System;
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

        using var socketA = UdpSocketV4.BindLocalIp();
        var endpointA = socketA.GetSocketName();
        using var socketB = UdpSocketV4.Create();
        socketB.Send(sendBuffer, AddressV4.Local.OnPort(endpointA.Port));
        var endpointB = socketB.GetSocketName();

        socketA.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out var result);
        Assert.Equal(UdpReceiveState.Success, result.State);
        Assert.Equal(endpointB.Port, result.Origin.Port);
        Assert.Equal(AddressV4.Local, result.Origin.Address);
        Assert.Equal(sendBuffer.Length, result.ReceivedByteCount);
        Assert.True(receiveBuffer.AsSpan(0, result.ReceivedByteCount).SequenceEqual(sendBuffer));

        socketA.Send(sendBuffer, AddressV4.Local.OnPort(endpointB.Port));
        socketB.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out result);
        Assert.Equal(UdpReceiveState.Success, result.State);
        Assert.Equal(endpointA.Port, result.Origin.Port);
        Assert.Equal(AddressV4.Local, result.Origin.Address);
        Assert.Equal(sendBuffer.Length, result.ReceivedByteCount);
    }

    [Fact]
    public void SendUdp128()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = UdpSocketV6.BindLocalIp();
        var endpointA = socketA.GetSocketName();
        using var socketB = UdpSocketV6.Create();
        socketB.Send(sendBuffer, AddressV6.Local.OnPort(endpointA.Port));
        var endpointB = socketB.GetSocketName();

        socketA.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out var result);
        Assert.Equal(UdpReceiveState.Success, result.State);
        Assert.Equal(endpointB.Port, result.Origin.Port);
        Assert.Equal(AddressV6.Local, result.Origin.Address);
        Assert.Equal(sendBuffer.Length, result.ReceivedByteCount);
        Assert.True(receiveBuffer.AsSpan(0, result.ReceivedByteCount).SequenceEqual(sendBuffer));

        socketA.Send(sendBuffer, AddressV6.Local.OnPort(endpointB.Port));
        socketB.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out result);
        Assert.Equal(UdpReceiveState.Success, result.State);
        Assert.Equal(endpointA.Port, result.Origin.Port);
        Assert.Equal(AddressV6.Local, result.Origin.Address);
        Assert.Equal(sendBuffer.Length, result.ReceivedByteCount);
    }

    [Fact]
    public void CanSendUdp32And128WhenAllowed()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = UdpSocketV4.Create();

        using var socketB = UdpSocketV6.BindAnyIp(true);
        var endpointB = socketB.GetSocketName();
        var destinationB = AddressV4.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var endpointA = socketA.GetSocketName();
        var v4LocalAsV6 = (AddressV6)AddressV4.Local;
        var destinationA = v4LocalAsV6.OnPort(endpointA.Port);
        socketB.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out var resultV6);
        var debug1 = v4LocalAsV6.ToString();
        var debug2 = resultV6.Origin.ToString();
        var debug3 = endpointB.ToString();
        Assert.Equal(UdpReceiveState.Success, resultV6.State);
        Assert.Equal(v4LocalAsV6, resultV6.Origin.Address);
        Assert.Equal(sendBuffer.Length, resultV6.ReceivedByteCount);
        Assert.True(receiveBuffer.AsSpan(0, resultV6.ReceivedByteCount).SequenceEqual(sendBuffer));

        receiveBuffer.AsSpan().Clear();
        Assert.False(receiveBuffer.AsSpan(0, resultV6.ReceivedByteCount).SequenceEqual(sendBuffer));

        socketB.Send(sendBuffer, destinationA);
        socketA.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out var resultV4);
        Assert.Equal(UdpReceiveState.Success, resultV4.State);
        Assert.Equal(AddressV4.Local, resultV4.Origin.Address);
        Assert.Equal(sendBuffer.Length, resultV4.ReceivedByteCount);
        Assert.True(receiveBuffer.AsSpan(0, resultV4.ReceivedByteCount).SequenceEqual(sendBuffer));
    }

    [Fact]
    public void CannotSendUdp32And128WhenDisallowed()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = UdpSocketV4.Create();
        using var socketB = UdpSocketV6.BindLocalIp();
        var endpointB = socketB.GetSocketName();
        var destinationB = AddressV4.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        socketB.Receive(receiveBuffer, TimeSpan.FromSeconds(1), out var result);
        Assert.Equal(UdpReceiveState.Timeout, result.State);
    }

    [Fact]
    public void CannotBindSamePort32()
    {
        using var socketA = UdpSocketV4.BindLocalIp();
        var endpointA = socketA.GetSocketName();

        Assert.NotEqual(0, endpointA.Port.HostValue);

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = UdpSocketV4.BindLocalIp(endpointA.Port);
            _ = socketB.GetSocketName();
        });
    }

    [Fact]
    public void CannotBindSamePort128()
    {
        using var socketA = UdpSocketV6.BindLocalIp();
        var endpoint = socketA.GetSocketName();

        Assert.NotEqual(0, endpoint.Port.HostValue);

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = UdpSocketV6.BindLocalIp(endpoint.Port);
            _ = socketB.GetSocketName();
        });
    }
}
