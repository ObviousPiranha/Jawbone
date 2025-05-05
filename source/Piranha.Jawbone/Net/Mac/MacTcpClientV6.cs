using System;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacTcpClientV6 : ITcpClient<AddressV6>
{
    private readonly int _fd;

    public Endpoint<AddressV6> Origin { get; }

    public MacTcpClientV6(int fd, Endpoint<AddressV6> origin)
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
                throw CreateExceptionFor.BadPoll();
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
        var addressLength = SockAddrStorage.Len;
        var result = Sys.GetSockName(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
    }

    public static MacTcpClientV6 Connect(Endpoint<AddressV6> endpoint)
    {
        int fd = Sys.Socket(Af.INet6, Sock.Stream, 0);

        if (fd == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            Tcp.SetNoDelay(fd);
            var addr = SockAddrIn6.FromEndpoint(endpoint);
            var connectResult = Sys.ConnectV6(fd, addr, SockAddrIn6.Len);
            if (connectResult == -1)
            {
                var errNo = Sys.ErrNo();
                Sys.Throw(errNo, $"Failed to connect to {endpoint}.");
            }

            return new MacTcpClientV6(fd, endpoint);
        }
        catch
        {
            _ = Sys.Close(fd);
            throw;
        }
    }
}
