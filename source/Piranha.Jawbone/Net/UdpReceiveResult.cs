using System;

namespace Piranha.Jawbone.Net;

public ref struct UdpReceiveResult<T>
{
    public ReadOnlySpan<byte> Received;
    public UdpReceiveState State;
    public ErrorCode Error;
    public int ReceivedByteCount;
    public T Origin;
}

public static class UdpReceiveResult
{
    public static void ThrowOnError<T>(
        in this UdpReceiveResult<T> udpReceiveResult)
    {
        if (udpReceiveResult.State == UdpReceiveState.Failure)
        {
            throw new SocketException("Error on UDP receive.")
            {
                Code = udpReceiveResult.Error
            };
        }
    }

    public static void ThrowOnTimeout<T>(
        in this UdpReceiveResult<T> udpReceiveResult)
    {
        if (udpReceiveResult.State == UdpReceiveState.Timeout)
            throw new TimeoutException("UDP socket timed out.");
    }

    public static void ThrowOnErrorOrTimeout<T>(
        in this UdpReceiveResult<T> udpReceiveResult)
    {
        udpReceiveResult.ThrowOnError();
        udpReceiveResult.ThrowOnTimeout();
    }
}
