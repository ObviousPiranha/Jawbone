using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsTcpListenerV6 : ITcpListener<AddressV6>
{
    private readonly nuint _fd;

    private WindowsTcpListenerV6(nuint fd) => _fd = fd;

    public ITcpClient<AddressV6>? Accept(TimeSpan timeout)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new WsaPollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.WsaPoll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addrLen = Unsafe.SizeOf<SockAddrStorage>();
                var fd = Sys.AcceptV6(_fd, out var addr, ref addrLen);
                if (fd < 0)
                    Sys.Throw("Failed to accept socket.");
                Tcp.SetNoDelay(fd);
                var endpoint = addr.GetV6(addrLen);
                var result = new WindowsTcpClientV6(fd, endpoint);
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

    public Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = Unsafe.SizeOf<SockAddrStorage>();
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
    }

    public void Dispose()
    {
        var result = Sys.CloseSocket(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public static WindowsTcpListenerV6 Listen(Endpoint<AddressV6> bindEndpoint, int backlog)
    {
        var fd = Sys.Socket(Af.INet6, Sock.Stream, 0);

        if (fd == Sys.InvalidSocket)
            Sys.Throw("Unable to open socket.");

        try
        {
            var sa = SockAddrIn6.FromEndpoint(bindEndpoint);
            var bindResult = Sys.BindV6(fd, sa, Unsafe.SizeOf<SockAddrIn6>());

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

            return new WindowsTcpListenerV6(fd);
        }
        catch
        {
            _ = Sys.CloseSocket(fd);
            throw;
        }
    }
}
