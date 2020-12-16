namespace Piranha.Sdl
{
    public static class SdlEvent
    {
        public const uint Quit = 0x100;
        public const uint DisplayEvent = 0x150;
        public const uint WindowEvent = 0x200;
        public const uint SysWmEvent = 0x201;
        public const uint KeyDown = 0x300;
        public const uint KeyUp = 0x301;
        public const uint TextEditing = 0x302;
        public const uint TextInput = 0x303;
        public const uint KeyMapChanged = 0x304;
        public const uint MouseMotion = 0x400;
        public const uint MouseButtonDown = 0x401;
        public const uint MouseButtonUp = 0x402;
        public const uint MouseWheel = 0x403;
        public const uint JoyAxisMotion = 0x600;
        public const uint JoyBallMotion = 0x601;
        public const uint JoyHatMotion = 0x602;
        public const uint JoyButtonDown = 0x603;
        public const uint JoyButtonUp = 0x604;
        public const uint JoyDeviceAdded = 0x605;
        public const uint JoyDeviceRemoved = 0x606;
        public const uint ControllerAxisMotion = 0x650;
        public const uint ControllerButtonDown = 0x651;
        public const uint ControllerButtonUp = 0x652;
        public const uint ControllerDeviceAdded = 0x653;
        public const uint ControllerDeviceRemoved = 0x654;
        public const uint ControllerDeviceRemapped = 0x655;
        public const uint FingerDown = 0x700;
        public const uint FingerUp = 0x701;
        public const uint FingerMotion = 0x702;
        public const uint DollarGesture = 0x800;
        public const uint DollarRecord = 0x801;
        public const uint Multigesture = 0x802;
        public const uint ClipboardUpdate = 0x900;
        public const uint DropFile = 0x1000;
        public const uint DropText = 0x1001;
        public const uint DropBegin = 0x1002;
        public const uint DropComplete = 0x1003;
        public const uint AudioDeviceAdded = 0x1100;
        public const uint AudioDeviceRemoved = 0x1101;
        public const uint SensorUpdate = 0x1200;
        public const uint RenderTargetsReset = 0x2000;
        public const uint RenderDeviceReset = 0x2001;
        public const uint UserEvent = 0x8000;
    }
}