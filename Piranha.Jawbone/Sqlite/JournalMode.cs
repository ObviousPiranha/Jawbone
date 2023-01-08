namespace Piranha.Jawbone.Sqlite;

public enum JournalMode : int
{
    Delete = 0,
    Truncate = 1,
    Persist = 2,
    Memory = 3,
    WriteAheadLog = 4,
    Off = 5
}
