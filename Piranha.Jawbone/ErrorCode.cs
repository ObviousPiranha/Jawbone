namespace Piranha.Jawbone;

public readonly struct ErrorCode
{
    public static readonly ErrorCode None = new(0, "", "No error");
    
    public readonly int Id { get; init; }
    public readonly string Name { get; init; }
    public readonly string Description { get; init; }

    public ErrorCode(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public override string ToString() => $"{Name}({Id}) {Description}";
}