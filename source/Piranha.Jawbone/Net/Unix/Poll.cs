namespace Piranha.Jawbone.Net.Unix;

static class Poll
{
    public const short In = 1 << 0;
    public const short Pri = 1 << 1;
    public const short Out = 1 << 2;
}
