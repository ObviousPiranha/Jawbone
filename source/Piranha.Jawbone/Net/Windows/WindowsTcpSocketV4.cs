using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsTcpSocketV4 : ITcpSocket<AddressV4>
{
    private readonly nuint _fd;

    public Endpoint<AddressV4> Origin { get; }

    public WindowsTcpSocketV4(nuint fd, Endpoint<AddressV4> origin)
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

        return writeResult;
    }

    public Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = Unsafe.SizeOf<SockAddrIn>();
        var result = Sys.GetSockNameV4(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.ToEndpoint();
    }

    public static WindowsTcpSocketV4 Connect(Endpoint<AddressV4> endpoint)
    {
        var fd = Sys.Socket(Af.INet, Sock.Stream, 0);

        if (fd == Sys.InvalidSocket)
            Sys.Throw("Unable to open socket.");

        try
        {
            Tcp.SetNoDelay(fd);
            var addr = SockAddrIn.FromEndpoint(endpoint);
            var result = Sys.ConnectV4(fd, addr, Unsafe.SizeOf<SockAddrIn>());
            if (result == -1)
            {
                var error = Sys.WsaGetLastError();
                Sys.Throw(error, $"Failed to connect to {endpoint}.");
            }

            return new WindowsTcpSocketV4(fd, endpoint);
        }
        catch
        {
            _ = Sys.CloseSocket(fd);
            throw;
        }
    }
}
