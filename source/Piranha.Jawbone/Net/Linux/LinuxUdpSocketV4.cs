using System;
using System.Diagnostics;

namespace Piranha.Jawbone.Net.Linux;

sealed class LinuxUdpSocketV4 : IUdpSocket<AddressV4>
{
    private readonly int _fd;

    private LinuxUdpSocketV4(int fd)
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
            SockAddrIn.Len);

        if (result == -1)
            Sys.Throw("Unable to send datagram.");

        return (int)result;
    }

    public unsafe int? Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out Endpoint<AddressV4> origin)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addressLength = SockAddrIn.Len;
                var receiveResult = Sys.RecvFromV4(
                    _fd,
                    out buffer.GetPinnableReference(),
                    (nuint)buffer.Length,
                    0,
                    out var address,
                    ref addressLength);

                if (receiveResult == -1)
                    Sys.Throw("Unable to receive data.");

                origin = address.ToEndpoint(addressLength);
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

    public unsafe Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = SockAddrIn.Len;
        var result = Sys.GetSockNameV4(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.ToEndpoint(addressLength);
    }

    public static LinuxUdpSocketV4 Create()
    {
        var fd = CreateSocket();
        return new LinuxUdpSocketV4(fd);
    }

    public static LinuxUdpSocketV4 Bind(Endpoint<AddressV4> endpoint)
    {
        var fd = CreateSocket();

        try
        {
            var sa = SockAddrIn.FromEndpoint(endpoint);
            var bindResult = Sys.BindV4(fd, sa, SockAddrIn.Len);

            if (bindResult == -1)
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to bind socket to address {endpoint}.");
            }

            return new LinuxUdpSocketV4(fd);
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
}
