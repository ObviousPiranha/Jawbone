using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Piranha.Jawbone.Net;

public sealed class LinuxUdpSocketV4 : IDisposable
{
    private int _socket;

    private LinuxUdpSocketV4(int socket)
    {
        _socket = socket;
    }

    public void Dispose()
    {
        var result = Linux.Close(_socket);
        if (result == -1)
            Throw("Unable to close socket.");
    }

    public unsafe int Send(ReadOnlySpan<byte> message, Endpoint<AddressV4> destination)
    {
        var sa = new Linux.SockAddrIn
        {
            SinFamily = Linux.Af.INet,
            SinPort = destination.Port.NetworkValue,
            SinAddr = destination.Address.DataU32
        };

        var result = Linux.SendTo(
            _socket,
            message.GetPinnableReference(),
            (nuint)message.Length,
            0,
            sa,
            (uint)sizeof(Linux.SockAddrIn));

        if (result == -1)
            Throw("Unable to send datagram.");

        return (int)result;
    }

    public unsafe int Receive(
        Span<byte> buffer,
        out Endpoint<AddressV4> origin,
        TimeSpan timeout)
    {
        var milliseconds = (int)(timeout.Ticks / TimeSpan.TicksPerMillisecond);
        var pfd = new Linux.PollFd { Fd = _socket, Events = Linux.EventTypes.PollIn };
        var pollResult = Linux.Poll(ref pfd, 1, milliseconds);

        if (0 < pollResult)
        {
            if ((pfd.REvents & Linux.EventTypes.PollIn) != 0)
            {
                var addressSize = (uint)sizeof(Linux.SockAddrIn);
                var receiveResult = Linux.RecvFrom(
                    _socket,
                    out buffer.GetPinnableReference(),
                    (nuint)buffer.Length,
                    0,
                    out var address,
                    ref addressSize);

                if (receiveResult == -1)
                    Throw("Unable to receive data.");

                origin = Endpoint.Create(
                    new AddressV4(address.SinAddr),
                    new NetworkPort { NetworkValue = address.SinPort });
                return (int)receiveResult;
            }
            else
            {
                origin = default;
                return 0;
            }
        }
        else // 0 indicates timeout
        {
            if (pollResult < 0)
                Throw("Poll failed.");
            origin = default;
            return 0;
        }
    }

    public static unsafe LinuxUdpSocketV4 Create()
    {
        int socket = Linux.Socket(
            Linux.Af.INet,
            Linux.Sock.DGram,
            Linux.IpProto.Udp);

        if (socket == -1)
            Throw("Unable to open socket.");

        return new LinuxUdpSocketV4(socket);
    }

    public static unsafe LinuxUdpSocketV4 Bind(Endpoint<AddressV4> endpoint)
    {
        int socket = Linux.Socket(
            Linux.Af.INet,
            Linux.Sock.DGram,
            Linux.IpProto.Udp);

        if (socket == -1)
            Throw("Unable to open socket.");

        try
        {
            var sa = new Linux.SockAddrIn
            {
                SinFamily = Linux.Af.INet,
                SinPort = endpoint.Port.NetworkValue,
                SinAddr = endpoint.Address.DataU32
            };

            var bindResult = Linux.Bind(socket, sa, (uint)sizeof(Linux.SockAddrIn));

            if (bindResult == -1)
                Throw($"Failed to bind socket to address {endpoint}.");

            return new LinuxUdpSocketV4(socket);
        }
        catch
        {
            Linux.Close(socket);
            throw;
        }
    }

    [DoesNotReturn]
    private static void Throw(string message)
    {
        var errno = Linux.ErrNo();
        var errorCode = ErrorCode.None;
        if (0 < errno && errno < Linux.ErrorCodes.Length)
            errorCode = Linux.ErrorCodes[errno];
        var exception = new SocketException(message + " " + errorCode.ToString())
        {
            Code = errorCode
        };
        throw exception;
    }
}
