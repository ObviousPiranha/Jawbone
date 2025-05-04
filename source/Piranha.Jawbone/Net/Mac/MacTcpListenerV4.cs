using System;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacTcpListenerV4 : ITcpListener<AddressV4>
{
    private readonly int _fd;

    private MacTcpListenerV4(int fd) => _fd = fd;

    public ITcpClient<AddressV4>? Accept(TimeSpan timeout)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addrLen = AddrLen;
                var fd = Sys.AcceptV4(_fd, out var addr, ref addrLen);
                if (fd < 0)
                    Sys.Throw("Failed to accept socket.");

                try
                {
                    Tcp.SetNoDelay(fd);
                    var endpoint = addr.ToEndpoint();
                    var result = new MacTcpClientV4(fd, endpoint);
                    return result;
                }
                catch
                {
                    _ = Sys.Close(fd);
                    throw;
                }

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

    public Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV4(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.ToEndpoint();
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public static MacTcpListenerV4 Listen(Endpoint<AddressV4> bindEndpoint, int backlog)
    {
        int fd = Sys.Socket(Af.INet, Sock.Stream, 0);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            var sa = SockAddrIn.FromEndpoint(bindEndpoint);
            var bindResult = Sys.BindV4(fd, sa, AddrLen);

            if (bindResult == -1)
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to bind socket to address {bindEndpoint}.");
            }

            var listenResult = Sys.Listen(fd, backlog);

            if (listenResult == -1)
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to listen on socket bound to {bindEndpoint}.");
            }

            return new MacTcpListenerV4(fd);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static uint AddrLen => Sys.SockLen<SockAddrIn>();
}
