using System;

namespace Piranha.Jawbone.Net.Linux;

sealed class LinuxTcpSocketV6 : ITcpSocket<AddressV6>
{
    private readonly int _fd;

    public Endpoint<AddressV6> Origin { get; }

    public LinuxTcpSocketV6(int fd, Endpoint<AddressV6> origin)
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

    public Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        // AssertAddrLen(addressLength);
        return address.ToEndpoint();
    }

    public static LinuxTcpSocketV6 Connect(Endpoint<AddressV6> endpoint)
    {
        int fd = Sys.Socket(Af.INet6, Sock.Stream, 0);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            Tcp.SetNoDelay(fd);
            var addr = SockAddrIn6.FromEndpoint(endpoint);
            var connectResult = Sys.ConnectV6(fd, addr, AddrLen);
            if (connectResult == -1)
                Sys.Throw($"Failed to connect to {endpoint}.");

            return new LinuxTcpSocketV6(fd, endpoint);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }

    private static uint AddrLen => Sys.SockLen<SockAddrIn6>();
}
