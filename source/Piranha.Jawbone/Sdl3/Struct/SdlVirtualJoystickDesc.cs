namespace Piranha.Jawbone.Sdl3;

public struct SdlVirtualJoystickDesc // SDL_VirtualJoystickDesc
{
    public uint Version; // Uint32 version
    public ushort Type; // Uint16 type
    public ushort Padding; // Uint16 padding
    public ushort VendorId; // Uint16 vendor_id
    public ushort ProductId; // Uint16 product_id
    public ushort Naxes; // Uint16 naxes
    public ushort Nbuttons; // Uint16 nbuttons
    public ushort Nballs; // Uint16 nballs
    public ushort Nhats; // Uint16 nhats
    public ushort Ntouchpads; // Uint16 ntouchpads
    public ushort Nsensors; // Uint16 nsensors
    public nint Padding2; // Uint16[2] padding2
    public uint ButtonMask; // Uint32 button_mask
    public uint AxisMask; // Uint32 axis_mask
    public nint Name; // char const * name
    public nint Touchpads; // SDL_VirtualJoystickTouchpadDesc const * touchpads
    public nint Sensors; // SDL_VirtualJoystickSensorDesc const * sensors
    public nint Userdata; // void * userdata
    public nint Update; // void (*)(void * userdata) * Update
    public nint SetPlayerIndex; // void (*)(void * userdata, int player_index) * SetPlayerIndex
    public nint Rumble; // bool (*)(void * userdata, Uint16 low_frequency_rumble, Uint16 high_frequency_rumble) * Rumble
    public nint RumbleTriggers; // bool (*)(void * userdata, Uint16 left_rumble, Uint16 right_rumble) * RumbleTriggers
    public nint SetLED; // bool (*)(void * userdata, Uint8 red, Uint8 green, Uint8 blue) * SetLED
    public nint SendEffect; // bool (*)(void * userdata, void const * data, int size) * SendEffect
    public nint SetSensorsEnabled; // bool (*)(void * userdata, bool enabled) * SetSensorsEnabled
    public nint Cleanup; // void (*)(void * userdata) * Cleanup
}
