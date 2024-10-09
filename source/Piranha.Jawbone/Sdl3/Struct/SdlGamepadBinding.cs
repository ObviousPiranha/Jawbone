namespace Piranha.Jawbone.Sdl3;

public struct SdlGamepadBinding // SDL_GamepadBinding
{
    public SdlGamepadBindingType InputType; // SDL_GamepadBindingType input_type
    public nint Input; //  input
    public SdlGamepadBindingType OutputType; // SDL_GamepadBindingType output_type
    public nint Output; //  output
}
