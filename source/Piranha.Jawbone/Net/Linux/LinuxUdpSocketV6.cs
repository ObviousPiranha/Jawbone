using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Linux;

sealed class LinuxUdpSocketV6 : IUdpSocket<AddressV6>
{
    private readonly int _fd;

    private LinuxUdpSocketV6(int fd)
    {
        _fd = fd;
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public unsafe int Send(ReadOnlySpan<byte> message, Endpoint<AddressV6> destination)
    {
        var sa = SockAddrIn6.FromEndpoint(destination);

        var result = Sys.SendToV6(
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
        out UdpReceiveResult<Endpoint<AddressV6>> result)
    {
        result = default;
        int milliseconds;
        {
            var ms64 = timeout.Ticks / TimeSpan.TicksPerMillisecond;
            if (int.MaxValue < ms64)
                milliseconds = int.MaxValue;
            else if (ms64 < 0)
                milliseconds = 0;
            else
                milliseconds = unchecked((int)ms64);
        }
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addressLength = AddrLen;
                var receiveResult = Sys.RecvFromV6(
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

    public unsafe Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        AssertAddrLen(addressLength);
        return address.ToEndpoint();
    }

    public static LinuxUdpSocketV6 Create(bool allowV4)
    {
        var fd = CreateSocket(allowV4);
        return new LinuxUdpSocketV6(fd);
    }

    public static LinuxUdpSocketV6 Bind(Endpoint<AddressV6> endpoint, bool allowV4)
    {
        var fd = CreateSocket(allowV4);

        try
        {
            var sa = SockAddrIn6.FromEndpoint(endpoint);
            var bindResult = Sys.BindV6(fd, sa, AddrLen);

            if (bindResult == -1)
                Sys.Throw($"Failed to bind socket to address {endpoint}.");

            return new LinuxUdpSocketV6(fd);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static int CreateSocket(bool allowV4)
    {
        var fd = Sys.Socket(Af.INet6, Sock.DGram, IpProto.Udp);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            int yes = allowV4 ? 0 : 1;
            var result = Sys.SetSockOpt(
                fd,
                IpProto.Ipv6,
                Ipv6.V6Only,
                yes,
                Sys.SockLen<int>());

            if (result == -1)
                Sys.Throw("Unable to set socket option.");

            return fd;
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static void AssertAddrLen(uint addrLen)
    {
        Debug.Assert(
            addrLen == AddrLen,
            "The returned address length does not match.");
    }

    private static uint AddrLen => (uint)Unsafe.SizeOf<SockAddrIn6>();
}
