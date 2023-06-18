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
        socketB.Send(sendBuffer, Endpoint.Local32(endpointA.Port));

        var length = socketA.Receive(receiveBuffer, out var origin);
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
        socketB.Send(sendBuffer, Endpoint.Local128(endpointA.Port));

        var length = socketA.Receive(receiveBuffer, out var origin);
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

        using var socketA = new UdpSocket32(Endpoint.Any);
        var endpointA = socketA.GetEndpoint();
        var destinationA = Endpoint.Create(Address32.Local.MapToV6(), endpointA.Port);

        using var socketB = new UdpSocket128(Endpoint.Any, true);
        var endpointB = socketB.GetEndpoint();
        var destinationB = Endpoint.Local32(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var lengthV6 = socketB.Receive(receiveBuffer, out var originV6);
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
    public void CannotBindSamePort()
    {
        using var socketA = new UdpSocket32(Endpoint.Any);
        var endpointA = socketA.GetEndpoint();

        Assert.Throws<SocketException>(() =>
        {
            using var socketB = new UdpSocket32(Endpoint.Any32(endpointA.Port));
        });

        if (!OperatingSystem.IsWindows())
        {
            // I guess Windows is cool with this.
            Assert.Throws<SocketException>(() =>
            {
                using var socketB = new UdpSocket128(Endpoint.Any128(endpointA.Port), true);
            });
        }
    }
}
