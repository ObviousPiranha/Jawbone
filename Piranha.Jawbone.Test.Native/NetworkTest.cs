using Piranha.Jawbone.Net;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Xunit;

namespace Piranha.Jawbone.Test.Native;

public class NetworkTest
{
    [Fact]
    public void AddressesAreExactSizes()
    {
        Assert.Equal(4, Unsafe.SizeOf<Address32>());
        Assert.Equal(16, Unsafe.SizeOf<Address128>());
    }

    [Fact]
    public void SendUdp32()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = new UdpSocket32(default);
        var endpointA = socketA.GetEndpoint();
        using var socketB = new UdpSocket32(default);
        var endpointB = socketB.GetEndpoint();
        socketB.Send(sendBuffer, Endpoint.Local32(endpointA.Port));

        var length = socketA.Receive(receiveBuffer, out var origin, TimeSpan.Zero);
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

        using var socketA = new UdpSocket128(default, false);
        var endpointA = socketA.GetEndpoint();
        using var socketB = new UdpSocket128(default, false);
        var endpointB = socketB.GetEndpoint();
        socketB.Send(sendBuffer, Endpoint.Local128(endpointA.Port));

        var length = socketA.Receive(receiveBuffer, out var origin, TimeSpan.Zero);
        Assert.Equal(endpointB.Port, origin.Port);
        Assert.Equal(Address128.Local, origin.Address);
        Assert.Equal(sendBuffer.Length, length);
        Assert.True(receiveBuffer.AsSpan(0, length).SequenceEqual(sendBuffer));
    }

    [Fact]
    public void SendUdp32And128()
    {
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = new UdpSocket32(default);
        var endpointA = socketA.GetEndpoint();
        var destinationA = Endpoint.Create(Address128.Create(Address32.Local), endpointA.Port);

        using var socketB = new UdpSocket128(default, true);
        var endpointB = socketB.GetEndpoint();
        var destinationB = Endpoint.Local32(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var lengthV6 = socketB.Receive(receiveBuffer, out var originV6, TimeSpan.Zero);
        Assert.Equal(Address128.Create(Address32.Local), originV6.Address);
        Assert.Equal(sendBuffer.Length, lengthV6);
        Assert.True(receiveBuffer.AsSpan(0, lengthV6).SequenceEqual(sendBuffer));

        receiveBuffer.AsSpan().Clear();
        socketB.Send(sendBuffer, destinationA);
        int lengthV4 = socketA.Receive(receiveBuffer, out var originV4, TimeSpan.Zero);
        Assert.Equal(Address32.Local, originV4.Address);
        Assert.Equal(sendBuffer.Length, lengthV4);
        Assert.True(receiveBuffer.AsSpan(0, lengthV4).SequenceEqual(sendBuffer));
    }

    [Fact]
    public void CannotBindSamePort()
    {
        using var socketA = new UdpSocket32(default);
        var endpointA = socketA.GetEndpoint();

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = new UdpSocket32(Endpoint.Any32(endpointA.Port));
        });

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = new UdpSocket128(Endpoint.Any128(endpointA.Port), true);
        });
    }
}
