using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
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

    static void AddressShenanigans()
    {
        var bytes = new byte[16];
        Random.Shared.NextBytes(bytes);
        var v6 = new Address128(bytes);
        Console.WriteLine(v6);

        Console.WriteLine(new Address128());
        Console.WriteLine(Address128.Local);
        Console.WriteLine(Address128.Create(0,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14));
        Console.WriteLine(Address128.Create(0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15));
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
        var info3 = socketProvider.GetAddressInfo("192.168.50.1", "8080");
        Dump(info3);

        Console.WriteLine("yo yo yo");
    }

    static void Main(string[] args)
    {
        try
        {
            using var socketProvider = new SocketProvider();
            if (1 < args.Length)
            {
                var info = socketProvider.GetAddressInfo(args[0], args[1]);
                var endpoint = info.V4[0];
                
                using var client = socketProvider.CreateAndBindUdpSocket32(default);
                Console.WriteLine("Client bound on " + client.GetEndpoint().ToString());
                var message = Encoding.UTF8.GetBytes("Greetings!");
                client.Send(message, endpoint);
                Console.WriteLine("Message sent to " + endpoint.ToString());
            }
            else if (args.Length == 1)
            {
                var port = int.Parse(args[0]);
                var endpoint = Endpoint.Create(Address32.Any, port);
                using var server = socketProvider.CreateAndBindUdpSocket32(endpoint);
                Console.WriteLine("Listening on " + endpoint.ToString());
                var buffer = new byte[4096];
                var n = server.Receive(buffer, out var origin, TimeSpan.FromMinutes(5));

                if (origin.IsDefault)
                {
                    Console.WriteLine("Timed out.");
                }
                else
                {
                    var message = Encoding.UTF8.GetString(buffer.AsSpan(0, n));
                    Console.WriteLine("Received: " + message);
                }
            }
            else
            {
                Console.WriteLine("Client provides IP and port. Server provides port.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    static void TryOutV6(SocketProvider socketProvider)
    {
        // var clientInfo = socketProvider.GetAddressInfo(null, null);
        // Dump(clientInfo);
        
        var serverInfo = socketProvider.GetAddressInfo(null, "12345");
        Dump(serverInfo);

        using var myServer = socketProvider.CreateAndBindUdpSocket128(serverInfo.V6[0]);
        // using var myServer = socketProvider.CreateAndBindUdpSocket128(new Endpoint<Address128>(Address128.Create(span => span.Fill((byte)0xff)), 1));
        Console.WriteLine("Server bound!");
        var serverEndpoint = myServer.GetEndpoint();
        Console.WriteLine(serverEndpoint);

        using var myClient = socketProvider.CreateAndBindUdpSocket128(default);
        Console.Write("Client bound!");
        Console.WriteLine(myClient.GetEndpoint());
        
        var message = new byte[] { 0xec, 0xc0, 0xfa, 0x11 };
        myClient.Send(message, serverEndpoint);

        var buffer = new byte[1024];
        var n = myServer.Receive(buffer, out var origin, TimeSpan.Zero);
        Console.WriteLine($"Received {n} bytes!");

        if (buffer.AsSpan(0, n).SequenceEqual(message))
            Console.WriteLine("They match!");
        else
            Console.WriteLine("They do not match...");
        
        Console.WriteLine("One more receive...");
        var stopwatch = Stopwatch.StartNew();
        n = myServer.Receive(buffer, out origin, TimeSpan.FromSeconds(2));
        Console.WriteLine($"{n} : {stopwatch.Elapsed}");
        Console.WriteLine("Nevermind. Bye!");
    }
}

