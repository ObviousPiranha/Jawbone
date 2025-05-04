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

    public unsafe int? Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out Endpoint<AddressV6> origin)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
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

                if (receiveResult == -1)
                    Sys.Throw("Unable to receive data.");

                origin = address.GetV6(addressLength);
                return (int)receiveResult;
            }
            else
            {
                throw Core.CreateBadPollException();
            }
        }
        else if (pollResult < 0)
        {
            var errNo = Sys.ErrNo();
            Sys.Throw(errNo, "Unable to poll socket.");
        }

        origin = default;
        return null;
    }

    public Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
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
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to bind socket to address {endpoint}.");
            }

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
            Ipv6.SetIpv6Only(fd, allowV4);
            return fd;
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static uint AddrLen => (uint)Unsafe.SizeOf<SockAddrIn6>();
}
