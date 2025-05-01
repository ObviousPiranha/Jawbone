using System;
using System.Diagnostics;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacUdpSocketV4 : IUdpSocket<AddressV4>
{
    private readonly int _fd;

    private MacUdpSocketV4(int fd)
    {
        _fd = fd;
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public unsafe int Send(ReadOnlySpan<byte> message, Endpoint<AddressV4> destination)
    {
        var sa = SockAddrIn.FromEndpoint(destination);

        var result = Sys.SendToV4(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0,
            sa,
            AddrLen);

        if (result == -1)
            Sys.Throw("Unable to send datagram.");

        return (int)result;
    }

    public unsafe void Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out UdpReceiveResult<Endpoint<AddressV4>> result)
    {
        result = default;
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addressLength = AddrLen;
                var receiveResult = Sys.RecvFromV4(
                    _fd,
                    out buffer.GetPinnableReference(),
                    (nuint)buffer.Length,
                    0,
                    out var address,
                    ref addressLength);

                AssertAddrLen(addressLength);

                if (receiveResult == -1)
                    Sys.Throw("Unable to receive data.");

                result.State = UdpReceiveState.Success;
                result.Origin = address.ToEndpoint();
                result.ReceivedByteCount = (int)receiveResult;
                result.Received = buffer[..(int)receiveResult];
            }
        }
        else if (pollResult < 0)
        {
            result.State = UdpReceiveState.Failure;
            result.Error = Sys.ErrNo();
        }
        else
        {
            result.State = UdpReceiveState.Timeout;
        }
    }

    public unsafe Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV4(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        AssertAddrLen(addressLength);
        return address.ToEndpoint();
    }

    public static MacUdpSocketV4 Create()
    {
        var fd = CreateSocket();
        return new MacUdpSocketV4(fd);
    }

    public static MacUdpSocketV4 Bind(Endpoint<AddressV4> endpoint)
    {
        var fd = CreateSocket();

        try
        {
            var sa = SockAddrIn.FromEndpoint(endpoint);
            var bindResult = Sys.BindV4(fd, sa, AddrLen);

            if (bindResult == -1)
                Sys.Throw($"Failed to bind socket to address {endpoint}.");

            return new MacUdpSocketV4(fd);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static int CreateSocket()
    {
        int fd = Sys.Socket(Af.INet, Sock.DGram, IpProto.Udp);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        return fd;
    }

    private static void AssertAddrLen(uint addrLen)
    {
        Debug.Assert(
            addrLen == AddrLen,
            "The returned address length does not match.");
    }

    private static uint AddrLen => Sys.SockLen<SockAddrIn>();
}
