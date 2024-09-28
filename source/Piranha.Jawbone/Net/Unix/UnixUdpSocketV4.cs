using System;
using System.Diagnostics.CodeAnalysis;

namespace Piranha.Jawbone.Net.Unix;

public sealed class UnixUdpSocketV4 : IDisposable
{
    private readonly int _fd;

    private UnixUdpSocketV4(int fd)
    {
        _fd = fd;
    }

    public void Dispose()
    {
        var result = Sys.Close(_fd);
        if (result == -1)
            Throw("Unable to close socket.");
    }

    public unsafe int Send(ReadOnlySpan<byte> message, Endpoint<AddressV4> destination)
    {
        var sa = new SockAddrIn
        {
            SinFamily = Af.INet,
            SinPort = destination.Port.NetworkValue,
            SinAddr = destination.Address.DataU32
        };

        var result = Sys.SendToV4(
            _fd,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0,
            sa,
            (uint)sizeof(SockAddrIn));

        if (result == -1)
            Throw("Unable to send datagram.");

        return (int)result;
    }

    public unsafe void Receive(
        Span<byte> buffer,
        TimeSpan timeout,
        out UdpReceiveResult<AddressV4> result)
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
                var addressSize = (uint)sizeof(SockAddrIn);
                var receiveResult = Sys.RecvFromV4(
                    _fd,
                    out buffer.GetPinnableReference(),
                    (nuint)buffer.Length,
                    0,
                    out var address,
                    ref addressSize);

                if (receiveResult == -1)
                    Throw("Unable to receive data.");

                result.State = UdpReceiveState.Success;
                result.Origin = Endpoint.Create(
                    new AddressV4(address.SinAddr),
                    new NetworkPort { NetworkValue = address.SinPort });
                result.ReceivedByteCount = (int)receiveResult;
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

    public static unsafe UnixUdpSocketV4 Create()
    {
        int socket = Sys.Socket(
            Af.INet,
            Sock.DGram,
            IpProto.Udp);

        if (socket == -1)
            Throw("Unable to open socket.");

        return new UnixUdpSocketV4(socket);
    }

    public static unsafe UnixUdpSocketV4 Bind(Endpoint<AddressV4> endpoint)
    {
        int socket = Sys.Socket(Af.INet, Sock.DGram, IpProto.Udp);

        if (socket == -1)
            Throw("Unable to open socket.");

        try
        {
            var sa = new SockAddrIn
            {
                SinFamily = Af.INet,
                SinPort = endpoint.Port.NetworkValue,
                SinAddr = endpoint.Address.DataU32
            };

            var bindResult = Sys.BindV4(socket, sa, (uint)sizeof(SockAddrIn));

            if (bindResult == -1)
                Throw($"Failed to bind socket to address {endpoint}.");

            return new UnixUdpSocketV4(socket);
        }
        catch
        {
            _ = Sys.Close(socket);
            throw;
        }
    }

    [DoesNotReturn]
    private static void Throw(string message)
    {
        var exception = new SocketException(message)
        {
            Error = Sys.ErrNo()
        };

        throw exception;
    }
}
