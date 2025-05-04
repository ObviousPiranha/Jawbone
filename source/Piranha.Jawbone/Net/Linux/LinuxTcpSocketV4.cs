using System;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Linux;

sealed class LinuxTcpSocketV4 : ITcpSocket<AddressV4>
{
    private readonly int _fd;

    public Endpoint<AddressV4> Origin { get; }

    public LinuxTcpSocketV4(int fd, Endpoint<AddressV4> origin)
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

    public int? Receive(Span<byte> buffer, TimeSpan timeout)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var readResult = Sys.Read(
                    _fd,
                    out buffer.GetPinnableReference(),
                    (nuint)buffer.Length);

                if (readResult == -1)
                    Sys.Throw("Unable to receive data.");

                return (int)readResult;
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
        var writeResult = Sys.Write(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length);

        if (writeResult == -1)
            Sys.Throw("Unable to send data.");

        return (int)writeResult;
    }

    public Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV4(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.ToEndpoint();
    }

    public static LinuxTcpSocketV4 Connect(Endpoint<AddressV4> endpoint)
    {
        int fd = Sys.Socket(Af.INet, Sock.Stream, 0);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            Tcp.SetNoDelay(fd);
            var addr = SockAddrIn.FromEndpoint(endpoint);
            var result = Sys.ConnectV4(fd, addr, AddrLen);
            if (result == -1)
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to connect to {endpoint}.");
            }

            return new LinuxTcpSocketV4(fd, endpoint);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static uint AddrLen => Sys.SockLen<SockAddrIn>();
}
