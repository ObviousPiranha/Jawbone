namespace Piranha.Jawbone.Net.Windows;

struct WsaPollFd
{
    public nuint Fd;
    public short Events;
    public short REvents;
}
