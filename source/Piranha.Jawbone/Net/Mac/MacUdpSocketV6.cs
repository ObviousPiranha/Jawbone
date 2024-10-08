using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net.Mac;

sealed class MacUdpSocketV6 : IUdpSocket<AddressV6>
{
    private readonly int _fd;

    private MacUdpSocketV6(int fd)
    {
        _fd = fd;
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Sys.Throw("Unable to close socket.");
    }

    public unsafe int Send(ReadOnlySpan<byte> message, Endpoint<AddressV6> destination)
    {
        var sa = SockAddrIn6.FromEndpoint(destination);

        var result = Sys.SendToV6(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0,
            sa,
            AddrLen);

        if (result == -1)
            Sys.Throw("Unable to send datagram.");

        return (int)result;
    }

    public unsafe void Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out UdpReceiveResult<Endpoint<AddressV6>> result)
    {
        result = default;
        int milliseconds;
        {
            var ms64 = timeout.Ticks / TimeSpan.TicksPerMillisecond;
            if (int.MaxValue < ms64)
                milliseconds = int.MaxValue;
            else if (ms64 < 0)
                milliseconds = 0;
            else
                milliseconds = unchecked((int)ms64);
        }
        var pfd = new PollFd { Fd = _fd, Events = Poll.In };
        var pollResult = Sys.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Poll.In) != 0)
            {
                var addressLength = AddrLen;
                var receiveResult = Sys.RecvFromV6(
                    _fd,
                    out buffer.GetPinnableReference(),
                    (nuint)buffer.Length,
                    0,
                    out var address,
                    ref addressLength);

                Endpoint<AddressV6> endpoint;
                if (addressLength == (uint)sizeof(SockAddrIn))
                    endpoint = address.V4.ToEndpoint().MapToV6();
                else if (addressLength == (uint)sizeof(SockAddrIn6))
                    endpoint = address.V6.ToEndpoint();
                else
                    throw new SocketException("Unsupported address size: " + addressLength);
                if (receiveResult == -1)
                    Sys.Throw("Unable to receive data.");

                result.State = UdpReceiveState.Success;
                result.Origin = endpoint;
                result.ReceivedByteCount = (int)receiveResult;
                result.Received = buffer[..(int)receiveResult];
            }
        }
        else if (pollResult < 0)
        {
            result.State = UdpReceiveState.Failure;
            result.Error = Sys.ErrNo();
        }
        else
        {
            result.State = UdpReceiveState.Timeout;
        }
    }

    public unsafe Endpoint<AddressV6> GetSocketName()
    {
        var addressLength = AddrLen;
        var result = Sys.GetSockNameV6(_fd, out var address, ref addressLength);
        if (result == -1)
            Sys.Throw("Unable to get socket name.");
        AssertAddrLen(addressLength);
        return address.V6.ToEndpoint();
    }

    public static MacUdpSocketV6 Create(bool allowV4)
    {
        var socket = CreateSocket(allowV4);
        return new MacUdpSocketV6(socket);
    }

    public static MacUdpSocketV6 Bind(Endpoint<AddressV6> endpoint, bool allowV4)
    {
        var socket = CreateSocket(allowV4);

        try
        {
            var sa = SockAddrIn6.FromEndpoint(endpoint);
            var bindResult = Sys.BindV6(socket, sa, AddrLen);

            if (bindResult == -1)
                Sys.Throw($"Failed to bind socket to address {endpoint}.");

            return new MacUdpSocketV6(socket);
        }
        catch
        {
            _ = Sys.Close(socket);
            throw;
        }
    }

    private static int CreateSocket(bool allowV4)
    {
        var socket = Sys.Socket(Af.INet6, Sock.DGram, IpProto.Udp);

        if (socket == -1)
            Sys.Throw("Unable to open socket.");

        try
        {
            int yes = allowV4 ? 0 : 1;
            var result = Sys.SetSockOpt(
                socket,
                IpProto.Ipv6,
                Ipv6.V6Only,
                yes,
                Sys.SockLen<int>());

            if (result == -1)
                Sys.Throw("Unable to set socket option.");

            return socket;
        }
        catch
        {
            _ = Sys.Close(socket);
            throw;
        }
    }

    private static void AssertAddrLen(uint addrLen)
    {
        Debug.Assert(
            addrLen == AddrLen,
            "The returned address length does not match.");
    }

    private static uint AddrLen => (uint)Unsafe.SizeOf<SockAddrStorage>();
}
