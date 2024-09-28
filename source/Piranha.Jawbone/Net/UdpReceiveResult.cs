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
}
