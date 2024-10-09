using Piranha.Jawbone.Net;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Piranha.Jawbone.Test;

public class NetworkTest
{
    private readonly ITestOutputHelper _output;

    public NetworkTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void SendAndReceiveUdpV4()
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
    public void SendAndReceiveUdpV6()
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
    public void CanSendUdpV4ToV6WhenAllowed()
    {
        var v4LocalAsV6 = (AddressV6)AddressV4.Local;
        var sendBuffer = new byte[256];
        sendBuffer.AsSpan().Fill(0xab);

        // Ensure that the amount received doesn't match by luck.
        var receiveBuffer = new byte[sendBuffer.Length * 2];

        using var socketA = UdpSocketV4.Create();

        using var socketB = UdpSocketV6.Bind(v4LocalAsV6.OnAnyPort(), true);
        var endpointB = socketB.GetSocketName();
        var destinationB = AddressV4.Local.OnPort(endpointB.Port);

        socketA.Send(sendBuffer, destinationB);
        var endpointA = socketA.GetSocketName();
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
    public void CannotSendUdpV4ToV6WhenDisallowed()
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
    public void CannotBindSamePortV4()
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
    public void CannotBindSamePortV6()
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

    [Fact]
    public void CanBindSamePortOnV4AndV6()
    {
        // Allow up to three attempts for really unlucky port selection.
        for (int i = 0; i < 3; ++i)
        {
            try
            {
                using var v4 = UdpSocketV4.BindLocalIp();
                var endpointV4 = v4.GetSocketName();
                using var v6 = UdpSocketV6.BindLocalIp(endpointV4.Port);
                var endpointV6 = v6.GetSocketName();
                Assert.Equal(endpointV4.Port, endpointV6.Port);
                _output.WriteLine($"Bound on port {endpointV4.Port}.");
                return;
            }
            catch
            {
                _output.WriteLine($"Bind #{i + 1} was unsuccessful.");
            }
        }

        Assert.Fail("Ran out of retries. Unable to bind V4 and V6 to same port.");
    }

    [Fact]
    public void CannotBindSamePortOnV4AndV6InDualMode()
    {
        var target = ((AddressV6)AddressV4.Local).OnAnyPort();
        for (int i = 0; i < 3; ++i)
        {
            using var v6 = UdpSocketV6.Bind(target, allowV4: true);
            var endpointV6 = v6.GetSocketName();

            Assert.Throws<SocketException>(
                () =>
                {
                    using var v4 = UdpSocketV4.BindLocalIp(endpointV6.Port);
                });
        }
    }
}
