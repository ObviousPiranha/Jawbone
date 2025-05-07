using System;
using System.Diagnostics;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsUdpSocketV4 : IUdpSocket<AddressV4>
{
    private readonly nuint _fd;
    private SockAddrStorage _address;

    public bool ThrowOnInterruptSend { get; set; }
    public bool ThrowOnInterruptReceive { get; set; }

    private WindowsUdpSocketV4(nuint fd)
    {
        _fd = fd;
    }

    public void Dispose()
    {
        var result = Sys.CloseSocket(_fd);
        if (result == -1)
            Sys.Throw(ExceptionMessages.CloseSocket);
    }

    public unsafe int Send(ReadOnlySpan<byte> message, Endpoint<AddressV4> destination)
    {
        var sa = SockAddrIn.FromEndpoint(destination);

        retry:
        var result = Sys.SendToV4(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0,
            sa,
            SockAddrIn.Len);

        if (result == -1)
        {
            var error = Sys.WsaGetLastError();
            if (Error.IsInterrupt(error) && !ThrowOnInterruptSend)
                goto retry;
            Sys.Throw(error, ExceptionMessages.SendDatagram);
        }

        return (int)result;
    }

    public unsafe int? Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out Endpoint<AddressV4> origin)
    {
        var milliseconds = Core.GetMilliseconds(timeout);
        var pfd = new WsaPollFd { Fd = _fd, Events = Poll.In };

        retry:
        var start = Stopwatch.GetTimestamp();
        var pollResult = Sys.WsaPoll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addressLength = SockAddrStorage.Len;
                var receiveResult = Sys.RecvFrom(
                    _fd,
                    out buffer.GetPinnableReference(),
                    buffer.Length,
                    0,
                    out _address,
                    ref addressLength);

                if (receiveResult == -1)
                    Sys.Throw(ExceptionMessages.ReceiveData);

                origin = _address.GetV4(addressLength);
                return (int)receiveResult;
            }
            else
            {
                throw CreateExceptionFor.BadPoll();
            }
        }
        else if (pollResult == -1)
        {
            var error = Sys.WsaGetLastError();
            if (Error.IsInterrupt(error) && !ThrowOnInterruptReceive)
            {
                var elapsed = Stopwatch.GetElapsedTime(start);
                milliseconds = Core.GetMilliseconds(timeout - elapsed);
                goto retry;
            }
            Sys.Throw(error, ExceptionMessages.Poll);
        }

        origin = default;
        return null;
    }

    public unsafe Endpoint<AddressV4> GetSocketName()
    {
        var addressLength = SockAddrStorage.Len;
        var result = Sys.GetSockName(_fd, out _address, ref addressLength);
        if (result == -1)
            Sys.Throw(ExceptionMessages.GetSocketName);
        return _address.GetV4(addressLength);
    }

    public static WindowsUdpSocketV4 Create()
    {
        var socket = CreateSocket();
        return new WindowsUdpSocketV4(socket);
    }

    public static WindowsUdpSocketV4 Bind(Endpoint<AddressV4> endpoint)
    {
        var socket = CreateSocket();

        try
        {
            So.SetReuseAddr(socket);
            var sa = SockAddrIn.FromEndpoint(endpoint);
            var bindResult = Sys.BindV4(socket, sa, SockAddrIn.Len);

            if (bindResult == -1)
                Sys.Throw($"Failed to bind socket to address {endpoint}.");

            return new WindowsUdpSocketV4(socket);
        }
        catch
        {
            _ = Sys.CloseSocket(socket);
            throw;
        }
    }

    private static nuint CreateSocket()
    {
        var socket = Sys.Socket(Af.INet, Sock.DGram, IpProto.Udp);

        if (socket == Sys.InvalidSocket)
            Sys.Throw(ExceptionMessages.OpenSocket);

        return socket;
    }
}
