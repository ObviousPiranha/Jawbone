using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsTcpListenerV4 : ITcpListener<AddressV4>
{
    private readonly nuint _fd;
    private SockAddrStorage _address;

    private WindowsTcpListenerV4(nuint fd) => _fd = fd;

    public ITcpClient<AddressV4>? Accept(TimeSpan timeout)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new WsaPollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.WsaPoll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addressLength = SockAddrStorage.Len;
                var fd = Sys.Accept(_fd, out _address, ref addressLength);
                if (fd == Sys.InvalidSocket)
                    Sys.Throw("Failed to accept socket.");

                try
                {
                    Tcp.SetNoDelay(fd);
                    var endpoint = _address.GetV4(addressLength);
                    var result = new WindowsTcpClientV4(fd, endpoint);
                    return result;
                }
                catch
                {
                    _ = Sys.CloseSocket(fd);
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
        var addressLength = SockAddrStorage.Len;
        var result = Sys.GetSockName(_fd, out _address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return _address.GetV4(addressLength);
    }

    public void Dispose()
    {
        var result = Sys.CloseSocket(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public static WindowsTcpListenerV4 Listen(Endpoint<AddressV4> bindEndpoint, int backlog)
    {
        var fd = Sys.Socket(Af.INet, Sock.Stream, 0);

        if (fd == Sys.InvalidSocket)
            Sys.Throw("Unable to open socket.");

        try
        {
            So.SetReuseAddr(fd);
            var sa = SockAddrIn.FromEndpoint(bindEndpoint);
            var bindResult = Sys.BindV4(fd, sa, SockAddrIn.Len);

            if (bindResult == -1)
            {
                var error = Sys.WsaGetLastError();
                Sys.Throw(error, $"Failed to bind socket to address {bindEndpoint}.");
            }

            var listenResult = Sys.Listen(fd, backlog);

            if (listenResult == -1)
            {
                var error = Sys.WsaGetLastError();
                Sys.Throw(error, $"Failed to listen on socket bound to {bindEndpoint}.");
            }

            return new WindowsTcpListenerV4(fd);
        }
        catch
        {
            _ = Sys.CloseSocket(fd);
            throw;
        }
    }
}
