namespace Piranha.Jawbone.Net.Unix;

struct PollFd
{
    public int Fd;
    public short Events;
    public short REvents;
}
