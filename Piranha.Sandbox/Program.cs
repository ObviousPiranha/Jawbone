using System;
using System.Runtime.CompilerServices;
using Piranha.Jawbone.Net;

namespace Piranha.Sandbox;

class Program
{
    static void Dump(AddressInfo addressInfo)
    {
        Console.WriteLine("v4: " + string.Join(", ", addressInfo.V4));
        Console.WriteLine("v6: " + string.Join(", ", addressInfo.V6));
    }

    static void ShowSize<T>()
    {
        Console.WriteLine($"{typeof(T)}: {Unsafe.SizeOf<T>()}");
    }

    static void Main(string[] args)
    {
        var bytes = new byte[16];
        Random.Shared.NextBytes(bytes);
        var v6 = new Address128(bytes);
        Console.WriteLine(v6);

        Console.WriteLine(new Address128());
        Console.WriteLine(Address128.Create(0,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14));
        Console.WriteLine(Address128.Create(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15));
        Console.WriteLine(Address128.Create(0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1));
        Console.WriteLine(Address128.Create(0,0,0,0,1,1));
        Console.WriteLine(Address128.Create(0,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0));

        var v4 = new Address32(192, 168, 0, 1);
        Console.WriteLine(v4);

        ShowSize<Endpoint<Address32>>();
        ShowSize<Endpoint<Address128>>();
        using var socketProvider = new SocketProvider();
        var info1 = socketProvider.GetAddressInfo("google.com", "443");
        Dump(info1);
        var info2 = socketProvider.GetAddressInfo("thebuzzsaw.duckdns.org", null);
        Dump(info2);
    }
}