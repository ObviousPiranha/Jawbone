namespace Piranha.Jawbone
{
    public readonly struct UpdateEvent
    {
        public const int Resize = 1;

        public int EventType { get; }
        public uint WindowId { get; }
        public int Data1 { get; }
        public int Data2 { get; }

        public UpdateEvent(int eventType, uint windowId, int data1, int data2)
        {
            WindowId = windowId;
            EventType = eventType;
            Data1 = data1;
            Data2 = data2;
        }
    }
}
