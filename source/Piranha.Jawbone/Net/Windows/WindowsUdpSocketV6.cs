using System;
using System.Diagnostics;

namespace Piranha.Jawbone.Net.Windows;

sealed class WindowsUdpSocketV6 : IUdpSocket<AddressV6>
{
    private readonly nuint _fd;
    private SockAddrStorage _address;

    public InterruptHandling HandleInterruptOnSend { get; set; }
    public InterruptHandling HandleInterruptOnReceive { get; set; }

    private WindowsUdpSocketV6(nuint fd)
    {
        _fd = fd;
    }

    public void Dispose()
    {
        var result = Sys.CloseSocket(_fd);
        if (result == -1)
            Sys.Throw(ExceptionMessages.CloseSocket);
    }

    public unsafe TransferResult Send(ReadOnlySpan<byte> message, Endpoint<AddressV6> destination)
    {
        var sa = SockAddrIn6.FromEndpoint(destination);

    retry:
        var result = Sys.SendToV6(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0,
            sa,
            SockAddrIn6.Len);

        if (result == -1)
        {
            var error = Sys.WsaGetLastError();
            if (!Error.IsInterrupt(error) || HandleInterruptOnSend == InterruptHandling.Error)
                Sys.Throw(error, ExceptionMessages.SendDatagram);
            if (HandleInterruptOnSend != InterruptHandling.Abort)
                goto retry;
            return new(SocketResult.Interrupt);
        }

        return new((int)result);
    }

    public unsafe TransferResult Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out Endpoint<AddressV6> origin)
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

                origin = _address.GetV6(addressLength);
                return new((int)receiveResult);
            }
            else
            {
                throw CreateExceptionFor.BadPoll();
            }
        }
        else if (pollResult == -1)
        {
            var error = Sys.WsaGetLastError();
            if (!Error.IsInterrupt(error) || HandleInterruptOnReceive == InterruptHandling.Error)
            {
                Sys.Throw(error, ExceptionMessages.Poll);
            }
            else if (HandleInterruptOnReceive != InterruptHandling.Abort)
            {
                var elapsed = Stopwatch.GetElapsedTime(start);
                milliseconds = Core.GetMilliseconds(timeout - elapsed);
                goto retry;
            }
            else
            {
                origin = default;
                return new(SocketResult.Interrupt);
            }
        }

        origin = default;
        return new(SocketResult.Timeout);
    }

    public unsafe Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = SockAddrStorage.Len;
        var result = Sys.GetSockName(_fd, out _address, ref addressLength);
        if (result == -1)
            Sys.Throw(ExceptionMessages.GetSocketName);
        return _address.GetV6(addressLength);
    }

    public static WindowsUdpSocketV6 Create(bool allowV4)
    {
        var socket = CreateSocket(allowV4);
        return new WindowsUdpSocketV6(socket);
    }

    public static WindowsUdpSocketV6 Bind(Endpoint<AddressV6> endpoint, bool allowV4)
    {
        var socket = CreateSocket(allowV4);

        try
        {
            So.SetReuseAddr(socket);
            var sa = SockAddrIn6.FromEndpoint(endpoint);
            var bindResult = Sys.BindV6(socket, sa, SockAddrIn6.Len);

            if (bindResult == -1)
                Sys.Throw($"Failed to bind socket to address {endpoint}.");

            return new WindowsUdpSocketV6(socket);
        }
        catch
        {
            _ = Sys.CloseSocket(socket);
            throw;
        }
    }

    private static nuint CreateSocket(bool allowV4)
    {
        var socket = Sys.Socket(Af.INet6, Sock.DGram, IpProto.Udp);

        if (socket == Sys.InvalidSocket)
            Sys.Throw(ExceptionMessages.OpenSocket);

        try
        {
            Ipv6.SetIpv6Only(socket, allowV4);
            return socket;
        }
        catch
        {
            _ = Sys.CloseSocket(socket);
            throw;
        }
    }
}
