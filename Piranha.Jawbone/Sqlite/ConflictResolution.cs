namespace Piranha.Jawbone.Sqlite;

// https://www.sqlite.org/lang_conflict.html
public enum ConflictResolution
{
    Default = 0,
    Replace = 1,
    Rollback = 2,
    Abort = 3,
    Fail = 4,
    Ignore = 5
}
