using System;

namespace Piranha.Jawbone.Sqlite;

// https://www.sqlite.org/c3ref/open.html
[Flags]
enum SqliteOpen : int
{
    ReadOnly = 1 << 0,
    ReadWrite = 1 << 1,
    Create = 1 << 2,
    Uri = 1 << 6,
    Memory = 1 << 7,
    NoMutex = 1 << 15,
    FullMutex = 1 << 16,
    SharedCache = 1 << 17,
    PrivateCache = 1 << 18
}
