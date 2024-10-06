namespace Piranha.Jawbone.Sdl3;

public struct Sdlhid_device_info // SDL_hid_device_info
{
    public nint Path; // char * path
    public ushort VendorId; // unsigned short vendor_id
    public ushort ProductId; // unsigned short product_id
    public nint SerialNumber; // wchar * serial_number
    public ushort ReleaseNumber; // unsigned short release_number
    public nint ManufacturerString; // wchar * manufacturer_string
    public nint ProductString; // wchar * product_string
    public ushort UsagePage; // unsigned short usage_page
    public ushort Usage; // unsigned short usage
    public int InterfaceNumber; // int interface_number
    public int InterfaceClass; // int interface_class
    public int InterfaceSubclass; // int interface_subclass
    public int InterfaceProtocol; // int interface_protocol
    public Sdlhid_bus_type BusType; // SDL_hid_bus_type bus_type
    public nint Next; // SDL_hid_device_info * next
}
