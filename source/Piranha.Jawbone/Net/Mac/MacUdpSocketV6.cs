using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacUdpSocketV6 : IUdpSocket<AddressV6>
{
    private readonly int _fd;

    private MacUdpSocketV6(int fd)
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
            SockAddrIn6.Len);

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
                var addressLength = SockAddrIn6.Len;
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

    public unsafe Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = SockAddrIn6.Len;
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
    }

    public static MacUdpSocketV6 Create(bool allowV4)
    {
        var fd = CreateSocket(allowV4);
        return new MacUdpSocketV6(fd);
    }

    public static MacUdpSocketV6 Bind(Endpoint<AddressV6> endpoint, bool allowV4)
    {
        var fd = CreateSocket(allowV4);

        try
        {
            var sa = SockAddrIn6.FromEndpoint(endpoint);
            var bindResult = Sys.BindV6(fd, sa, SockAddrIn6.Len);

            if (bindResult == -1)
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to bind socket to address {endpoint}.");
            }

            return new MacUdpSocketV6(fd);
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
}
