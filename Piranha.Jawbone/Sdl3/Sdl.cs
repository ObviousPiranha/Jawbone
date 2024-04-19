namespace Piranha.Jawbone.Sdl3;

public static class Sdl
{
    public static class Audio
    {
        public static class Device
        {
            public static class Default
            {
                public const uint Output = uint.MaxValue;
                public const uint Capture = 0xfffffffe;
            }
        }
    }

    public static class Gl
    {
        public static class Context
        {
            public const int DebugFlag = 1 << 0;
            public const int ForwardCompatibleFlag = 1 << 1;
            public const int RobustAccessFlag = 1 << 2;
            public const int ResetIsolationFlag = 1 << 3;

            public static class Profile
            {
                public const int Core = 1 << 0;
                public const int Compatibility = 1 << 1;
                public const int Es = 1 << 2;
            }
        }
    }
}
