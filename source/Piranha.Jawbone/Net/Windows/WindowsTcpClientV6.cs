using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsTcpClientV6 : ITcpClient<AddressV6>
{
    private readonly nuint _fd;

    public Endpoint<AddressV6> Origin { get; }

    public WindowsTcpClientV6(nuint fd, Endpoint<AddressV6> origin)
    {
        _fd = fd;
        Origin = origin;
    }

    public void Dispose()
    {
        var result = Sys.CloseSocket(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public int? Receive(Span<byte> buffer, TimeSpan timeout)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new WsaPollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.WsaPoll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var readResult = Sys.Recv(
                    _fd,
                    out buffer.GetPinnableReference(),
                    buffer.Length,
                    0);

                if (readResult == -1)
                    Sys.Throw("Unable to receive data.");

                return readResult;
            }
            else
            {
                throw new InvalidOperationException("Unexpected poll event.");
            }
        }
        else if (pollResult < 0)
        {
            Sys.Throw("Error while polling socket.");
        }

        return null;
    }

    public int Send(ReadOnlySpan<byte> message)
    {
        var writeResult = Sys.Send(
            _fd,
            message.GetPinnableReference(),
            message.Length,
            0);

        if (writeResult == -1)
            Sys.Throw("Unable to send data.");

        return (int)writeResult;
    }

    public Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = Unsafe.SizeOf<SockAddrStorage>();
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
    }

    public static WindowsTcpClientV6 Connect(Endpoint<AddressV6> endpoint)
    {
        var fd = Sys.Socket(Af.INet6, Sock.Stream, 0);

        if (fd == Sys.InvalidSocket)
            Sys.Throw("Unable to open socket.");

        try
        {
            Tcp.SetNoDelay(fd);
            var addr = SockAddrIn6.FromEndpoint(endpoint);
            var connectResult = Sys.ConnectV6(fd, addr, Unsafe.SizeOf<SockAddrIn6>());
            if (connectResult == -1)
            {
                var error = Sys.WsaGetLastError();
                Sys.Throw(error, $"Failed to connect to {endpoint}.");
            }

            return new WindowsTcpClientV6(fd, endpoint);
        }
        catch
        {
            _ = Sys.CloseSocket(fd);
            throw;
        }
    }
}
