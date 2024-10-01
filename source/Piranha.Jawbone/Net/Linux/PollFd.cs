namespace Piranha.Jawbone.Net.Linux;

struct PollFd
{
    public int Fd;
    public short Events;
    public short REvents;
}
