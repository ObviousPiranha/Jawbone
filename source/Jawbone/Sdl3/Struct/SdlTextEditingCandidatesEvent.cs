namespace Jawbone.Sdl3;

public struct SdlTextEditingCandidatesEvent // SDL_TextEditingCandidatesEvent
{
    public SdlEventType Type; // SDL_EventType type
    public uint Reserved; // Uint32 reserved
    public ulong Timestamp; // Uint64 timestamp
    public uint WindowID; // SDL_WindowID windowID
    public nint Candidates; // char const * const * candidates
    public int NumCandidates; // Sint32 num_candidates
    public int SelectedCandidate; // Sint32 selected_candidate
    public byte Horizontal; // bool horizontal
    public byte Padding1; // Uint8 padding1
    public byte Padding2; // Uint8 padding2
    public byte Padding3; // Uint8 padding3
}
