namespace Piranha.Jawbone.Opus;

[System.Serializable]
public class OpusException : System.Exception
{
    internal static void ThrowOnError(int error)
    {
        // https://opus-codec.org/docs/opus_api-1.3.1/group__opus__errorcodes.html
        if (error < 0)
        {
            var errorText = error switch
            {
                -1 => "OPUS_BAD_ARG: One or more invalid/out of range arguments.",
                -2 => "OPUS_BUFFER_TOO_SMALL: Not enough bytes allocated in the buffer.",
                -3 => "OPUS_INTERNAL_ERROR: An internal error was detected.",
                -4 => "OPUS_INVALID_PACKET: The compressed data passed is corrupted.",
                -5 => "OPUS_UNIMPLEMENTED: Invalid/unsupported request number.",
                -6 => "OPUS_INVALID_STATE: An encoder or decoder structure is invalid or already freed.",
                -7 => "OPUS_ALLOC_FAIL: Memory allocation has failed.",
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
