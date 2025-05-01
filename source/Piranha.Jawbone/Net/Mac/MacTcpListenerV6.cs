using System;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacTcpListenerV6 : ITcpListener<AddressV6>
{
    private readonly int _fd;

    private MacTcpListenerV6(int fd) => _fd = fd;

    public ITcpSocket<AddressV6>? Accept(TimeSpan timeout)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addrLen = AddrLen;
                var fd = Sys.AcceptV6(_fd, out var addr, ref addrLen);
                if (fd < 0)
                    Sys.Throw("Failed to accept socket.");
                Tcp.SetNoDelay(fd);
                var endpoint = addr.GetV6(addrLen);
                var result = new MacTcpSocketV6(fd, endpoint);
                return result;
            }
            else
            {
                throw new InvalidOperationException("Unexpected poll event.");
            }
        }
        else if (pollResult < 0)
        {
            Sys.Throw("Error while polling socket.");
            return null;
        }
        else
        {
            return null;
        }
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public static MacTcpListenerV6 Listen(Endpoint<AddressV6> bindEndpoint, int backlog)
    {
        int fd = Sys.Socket(Af.INet6, Sock.Stream, 0);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            var sa = SockAddrIn6.FromEndpoint(bindEndpoint);
            var bindResult = Sys.BindV6(fd, sa, AddrLen);

            if (bindResult == -1)
                Sys.Throw($"Failed to bind socket to address {bindEndpoint}.");

            var listenResult = Sys.Listen(fd, backlog);

            if (listenResult == -1)
                Sys.Throw($"Failed to listen on socket bound to {bindEndpoint}.");

            return new MacTcpListenerV6(fd);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static uint AddrLen => Sys.SockLen<SockAddrIn6>();
}
