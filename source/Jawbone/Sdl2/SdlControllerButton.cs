namespace Jawbone.Sdl2;

public enum SdlControllerButton
{
    Invalid = -1,
    A,
    B,
    X,
    Y,
    Back,
    Guide,
    Start,
    LeftStick,
    RightStick,
    LeftShoulder,
    RightShoulder,
    DPadUp,
    DPadDown,
    DPadLeft,
    DPadRight,
    Misc1, /* Xbox Series X share button, PS5 microphone button, Nintendo Switch Pro capture button, Amazon Luna microphone button */
    Paddle1, /* Xbox Elite paddle P1 (upper left, facing the back) */
    Paddle2, /* Xbox Elite paddle P3 (upper right, facing the back) */
    Paddle3, /* Xbox Elite paddle P2 (lower left, facing the back) */
    Paddle4, /* Xbox Elite paddle P4 (lower right, facing the back) */
    Touchpad, /* PS4/PS5 touchpad button */
    Max
}
