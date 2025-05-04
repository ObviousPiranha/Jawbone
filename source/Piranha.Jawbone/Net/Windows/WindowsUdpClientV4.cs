using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsUdpClientV4 : IUdpClient<AddressV4>
{
    private readonly nuint _fd;

    public Endpoint<AddressV4> Origin { get; }

    public WindowsUdpClientV4(nuint fd, Endpoint<AddressV4> origin)
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

    public Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = Unsafe.SizeOf<SockAddrIn>();
        var result = Sys.GetSockNameV4(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.ToEndpoint();
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
                var addressLength = Unsafe.SizeOf<SockAddrIn>();
                var receiveResult = Sys.RecvFromV4(
                    _fd,
                    out buffer.GetPinnableReference(),
                    buffer.Length,
                    0,
                    out var address,
                    ref addressLength);

                if (receiveResult == -1)
                    Sys.Throw("Unable to receive data.");

                Debug.Assert(address.ToEndpoint() == Origin);
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
            message.Length,
            0);

        if (result == -1)
            Sys.Throw("Unable to send data.");

        return (int)result;
    }

    public static WindowsUdpClientV4 Connect(Endpoint<AddressV4> endpoint)
    {
        var fd = CreateSocket();

        try
        {
            var sa = SockAddrIn.FromEndpoint(endpoint);
            var result = Sys.ConnectV4(fd, sa, Unsafe.SizeOf<SockAddrIn>());
            if (result == -1)
            {
                var errNo = Sys.WsaGetLastError();
                Sys.Throw(errNo, $"Failed to connect to {endpoint}.");
            }

            return new WindowsUdpClientV4(fd, endpoint);
        }
        catch
        {
            _ = Sys.CloseSocket(fd);
            throw;
        }
    }

    private static nuint CreateSocket()
    {
        var fd = Sys.Socket(Af.INet, Sock.DGram, IpProto.Udp);

        if (fd == Sys.InvalidSocket)
            Sys.Throw("Unable to open socket.");

        return fd;
    }
}
