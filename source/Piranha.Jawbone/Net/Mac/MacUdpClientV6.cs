using System;
using System.Diagnostics;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacUdpClientV6 : IUdpClient<AddressV6>
{
    private readonly int _fd;

    public Endpoint<AddressV6> Origin { get; }

    public MacUdpClientV6(int fd, Endpoint<AddressV6> origin)
    {
        _fd = fd;
        Origin = origin;
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
    }

    public int? Receive(Span<byte> buffer, TimeSpan timeout)
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

                Debug.Assert(address.GetV6(addressLength) == Origin);
                return (int)receiveResult;
            }
        }
        else if (pollResult < 0)
        {
            Sys.Throw("Unable to poll socket.");
        }
        return null;
    }

    public int Send(ReadOnlySpan<byte> message)
    {
        var result = Sys.Send(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0);

        if (result == -1)
            Sys.Throw("Unable to send data.");

        return (int)result;
    }

    public static MacUdpClientV6 Connect(Endpoint<AddressV6> endpoint)
    {
        var fd = CreateSocket();

        try
        {
            var sa = SockAddrIn6.FromEndpoint(endpoint);
            var result = Sys.ConnectV6(fd, sa, AddrLen);
            if (result == -1)
                Sys.Throw($"Failed to connect to {endpoint}.");

            return new MacUdpClientV6(fd, endpoint);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static int CreateSocket()
    {
        int fd = Sys.Socket(Af.INet6, Sock.DGram, IpProto.Udp);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        return fd;
    }

    private static uint AddrLen => Sys.SockLen<SockAddrIn6>();
}
