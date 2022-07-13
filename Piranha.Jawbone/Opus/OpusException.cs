namespace Piranha.Jawbone.Opus;

[System.Serializable]
public class OpusException : System.Exception
{
    internal static void ThrowOnError(int error)
    {
        if (error < 0)
        {
            var errorText = error switch
            {
                -1 => "Bad arg",
                -2 => "Buffer too small",
                -3 => "Internal error",
                -4 => "Invalid packet",
                -5 => "Unimplemented",
                -6 => "Invalid state",
                -7 => "Alloc fail",
                _ => "Unknown error value: " + error
            };

            throw new OpusException(errorText);
        }
    }

    public OpusException() { }
    public OpusException(string message) : base(message) { }
    public OpusException(string message, System.Exception inner) : base(message, inner) { }
    protected OpusException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
