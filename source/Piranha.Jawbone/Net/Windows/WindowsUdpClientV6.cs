using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsUdpClientV6 : IUdpClient<AddressV6>
{
    private readonly nuint _fd;

    public Endpoint<AddressV6> Origin { get; }

    public WindowsUdpClientV6(nuint fd, Endpoint<AddressV6> origin)
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

    public Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = Unsafe.SizeOf<SockAddrStorage>();
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        return address.GetV6(addressLength);
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
                var addressLength = Unsafe.SizeOf<SockAddrStorage>();
                var receiveResult = Sys.RecvFromV6(
                    _fd,
                    out buffer.GetPinnableReference(),
                    buffer.Length,
                    0,
                    out var address,
                    ref addressLength);

                if (receiveResult == -1)
                    Sys.Throw("Unable to receive data.");

                Debug.Assert(address.GetV6(addressLength) == Origin);
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

    public static WindowsUdpClientV6 Connect(Endpoint<AddressV6> endpoint)
    {
        var fd = CreateSocket();

        try
        {
            var sa = SockAddrIn6.FromEndpoint(endpoint);
            var result = Sys.ConnectV6(fd, sa, Unsafe.SizeOf<SockAddrIn6>());
            if (result == -1)
            {
                var errNo = Sys.WsaGetLastError();
                Sys.Throw(errNo, $"Failed to connect to {endpoint}.");
            }

            return new WindowsUdpClientV6(fd, endpoint);
        }
        catch
        {
            _ = Sys.CloseSocket(fd);
            throw;
        }
    }

    private static nuint CreateSocket()
    {
        var fd = Sys.Socket(Af.INet6, Sock.DGram, IpProto.Udp);

        if (fd == Sys.InvalidSocket)
            Sys.Throw("Unable to open socket.");

        return fd;
    }
}
