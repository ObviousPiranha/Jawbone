namespace Piranha.Sdl
{
    public static class SdlKeymod
    {
        public const int None = 0x0000;
        public const int LShift = 0x0001;
        public const int RShift = 0x0002;
        public const int LCtrl = 0x0040;
        public const int RCtrl = 0x0080;
        public const int LAlt = 0x0100;
        public const int RAlt = 0x0200;
        public const int LGui = 0x0400;
        public const int RGui = 0x0800;
        public const int Num = 0x1000;
        public const int Caps = 0x2000;
        public const int Mode = 0x4000;
        public const int Reserved = 0x8000;

        public const int Ctrl = LCtrl | RCtrl;
        public const int Shift = LShift | RShift;
        public const int Alt = LAlt | RAlt;
        public const int Gui = LGui | RGui;
    }
}
