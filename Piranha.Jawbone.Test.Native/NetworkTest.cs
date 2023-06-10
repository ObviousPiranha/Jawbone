using Piranha.Jawbone.Net;
using System;
using System.Security.Cryptography;
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
}
