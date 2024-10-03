using System;

namespace Piranha.Jawbone.Net;

public struct UdpReceiveResult<TAddress> where TAddress : unmanaged, IAddress<TAddress>
{
    public UdpReceiveState State;
    public int ReceivedByteCount;
    public int Error;
    public Endpoint<TAddress> Origin;
}

public static class UdpReceiveResult
{
    public static ErrorCode GetErrorCode<TAddress>(
        in this UdpReceiveResult<TAddress> udpReceiveResult
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        return SocketException.GetErrorCode(udpReceiveResult.Error);
    }

    public static void ThrowOnError<TAddress>(
        in this UdpReceiveResult<TAddress> udpReceiveResult
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        if (udpReceiveResult.State == UdpReceiveState.Failure)
        {
            throw new SocketException("Error on UDP receive.")
            {
                Error = udpReceiveResult.Error
            };
        }
    }

    public static void ThrowOnTimeout<TAddress>(
        in this UdpReceiveResult<TAddress> udpReceiveResult
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        if (udpReceiveResult.State == UdpReceiveState.Timeout)
            throw new TimeoutException("UDP socket timed out.");
    }

    public static void ThrowOnErrorOrTimeout<TAddress>(
        in this UdpReceiveResult<TAddress> udpReceiveResult
        ) where TAddress : unmanaged, IAddress<TAddress>
    {
        udpReceiveResult.ThrowOnError();
        udpReceiveResult.ThrowOnTimeout();
    }
}
