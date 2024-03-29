using Microsoft.VisualBasic;
using Piranha.Jawbone;
using Piranha.Jawbone.Net;
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Piranha.Sandbox;

class NetworkShenanigans
{
    static Span<byte> GenericTest<T>(T address) where T : unmanaged, IAddress<T>
    {
        var span = T.AsBytes(ref address);
        return default;
    }

    static void GetAndDump(string? node, string? service)
    {
        var addressInfo = AddressInfo.Get(node, service);
        Console.WriteLine($"({node}) ({service})");
        Dump(addressInfo);
    }

    static void Dump(AddressInfo addressInfo)
    {
        Console.WriteLine("  v4: " + string.Join(", ", addressInfo.V4));
        Console.WriteLine("  v6: " + string.Join(", ", addressInfo.V6));
    }

    static void ShowSize<T>()
    {
        Console.WriteLine($"{typeof(T)}: {Unsafe.SizeOf<T>()}");
    }

    static void AddressShenanigans()
    {
        var bytes = new byte[16];
        Random.Shared.NextBytes(bytes);
        var v6 = new AddressV6(bytes);
        Console.WriteLine(v6);

        var byteBuffer = new ByteBuffer();
        var endpoints = new Endpoint<AddressV6>[16];
        var randomAddress = AddressV6.Create(static span => RandomNumberGenerator.Fill(span));
        endpoints.AsSpan().Fill(Endpoint.Create(randomAddress, 200));
        byteBuffer.AddAllAsBytes(endpoints);

        Console.WriteLine((AddressV6)AddressV4.Local);
        Console.WriteLine(new AddressV6());
        Console.WriteLine(AddressV6.Local);
        Console.WriteLine(AddressV6.Create(0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
        Console.WriteLine(AddressV6.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
        Console.WriteLine(AddressV6.Create(0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0));

        var v4 = new AddressV4(192, 168, 0, 1);
        Console.WriteLine(v4);

        ShowSize<Endpoint<AddressV4>>();
        ShowSize<Endpoint<AddressV6>>();
        var info1 = AddressInfo.Get("google.com", "443");
        Dump(info1);
        var info3 = AddressInfo.Get("192.168.50.1", "8080");
        Dump(info3);

        Console.WriteLine("yo yo yo");

        using (var socket = UdpSocketV4.CreateWithoutBinding())
            Console.WriteLine(socket.GetEndpoint().ToString());

        using (var socket = UdpSocketV4.CreateWithoutBinding())
            Console.WriteLine(socket.GetEndpoint().ToString());
    }

    static void AllowV4(bool allowV4)
    {
        var sendBuffer = new byte[256];
        var receiveBuffer = new byte[sendBuffer.Length];

        RandomNumberGenerator.Fill(sendBuffer);
        using (var socketV6 = UdpSocketV4.CreateWithoutBinding())
        {
            var endpointV6 = socketV6.GetEndpoint();
            var word = allowV4 ? "enabled" : "disabled";
            Console.WriteLine($"Bound on {endpointV6} with V4 {word}.");
            var destination = Endpoint.Create(AddressV4.Local, endpointV6.Port);
            using (var socketV4 = UdpSocketV4.CreateWithoutBinding())
            {
                socketV4.Send(sendBuffer, destination);
                var endpointV4 = socketV4.GetEndpoint();
                Console.WriteLine($"Sent message from {endpointV4}.");
            }

            var length = socketV6.Receive(receiveBuffer, out var origin, TimeSpan.FromSeconds(1));
            if (length == sendBuffer.Length && sendBuffer.AsSpan().SequenceEqual(receiveBuffer))
            {
                Console.WriteLine($"Received correct message from {origin}.");
            }
            else
            {
                Console.WriteLine($"Received nothing from {origin}. :(");
            }
        }
    }

    static void FancyBinding()
    {
        var endpointA = new AddressV4(192, 168, 50, 181).OnPort(7777);
        using var socketA = UdpSocketV4.Bind(endpointA);
        Console.WriteLine($"Socket A on {endpointA}.");

        using var socketB = UdpSocketV4.CreateWithoutBinding();
        var endpointB = socketB.GetEndpoint();
        Console.WriteLine($"Socket B on {endpointB}.");

        socketB.Send("Let's do this"u8, endpointA);

        var buffer = new byte[2048];
        var length = socketA.Receive(buffer, out var origin, TimeSpan.FromSeconds(1));

        if (origin.IsDefault)
        {
            Console.WriteLine("Received nothing. :(");
            return;
        }

        Console.WriteLine($"Received {length} bytes from {origin}.");
    }

    static void ErrorOnPurpose()
    {
        try
        {
            using var socketA = UdpSocketV4.CreateWithoutBinding();
            var endpoint = socketA.GetEndpoint();
            Console.WriteLine($"Bound socket on {endpoint}.");

            // using var socketB = new UdpSocketV4(AnyAddress.OnPort(endpoint.Port));
            Console.WriteLine("Hm. This shouldn't be possible.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    static void GetSomeAddresses()
    {
        GetAndDump(null, null);
        GetAndDump("localhost", null);
        GetAndDump("localhost", "25");
        GetAndDump("thebuzzsaw.duckdns.org", null);
    }

    static void ParseSomeAddresses(AddressV6 address)
    {
        var asString = address.ToString();
        Console.WriteLine("Original: " + asString);
        var parsed = AddressV6.Parse(asString, null);
        Console.WriteLine("Parsed:   " + parsed.ToString());
    }

    static void CleverAssignment<TAddress>(TAddress address) where TAddress : unmanaged, IAddress<TAddress>
    {
    }

    static void CleverForwarding<TAddress>() where TAddress : unmanaged, IAddress<TAddress>
    {
        CleverAssignment(TAddress.Local);
    }

    static void BindToLinkLocal()
    {
        GetAndDump("fe80::ccb6:72b9:6d63:6863%wlp2s0", "7777");
        var address = AddressV6.Parse("fe80::ccb6:72b9:6d63:6863", null);
        Console.WriteLine("Address: " + address);
        using var socketA = UdpSocketV6.Bind(address.WithScopeId(2).OnPort(7777));
        var endpointA = socketA.GetEndpoint();
        Console.WriteLine("Bound to " + endpointA.ToString());

        using var socketB = UdpSocketV6.Bind(address.WithScopeId(2).OnPort(9999));
        var endpointB = socketB.GetEndpoint();
        Console.WriteLine("Bound to " + endpointB.ToString());
        var message = "HOORAH"u8;

        var destination = address.WithScopeId(2).OnPort(7777);
        Console.WriteLine("Sending message to " + destination);
        socketB.Send(message, destination);

        var buffer = new byte[256];
        int length = socketB.Receive(buffer, out var origin, TimeSpan.FromSeconds(1));
        if (origin.IsDefault)
        {
            Console.WriteLine("Received nothing...");
        }
        else
        {
            var received = Encoding.UTF8.GetString(buffer.AsSpan(0, length));
            Console.WriteLine($"Received from {origin}: {received}");
        }
    }

    static void ProjectSomeLines()
    {
        var ls = new LineSegment32(
            new Point32(0, 0),
            new Point32(7, 1));

        var x = 30;
        Line.IntersectsAtX(ls, x, out var y);
        Console.WriteLine($"Intersects at {x}, {y}");

        y = 200;
        Line.IntersectsAtY(ls, y, out x);
        Console.WriteLine($"Intersects at {x}, {y}");
    }

    static void ReadSomeCsv()
    {
        var csv = "aaaaaa\nbbb\ncccc,ccc,,ccccccc\r\nddddd";
        var utf8 = Encoding.UTF8.GetBytes(csv);
        using var stream = new MemoryStream(utf8);
        var reader = new CsvReader();
        reader.Start(stream.Read);
        while (reader.TryReadRow())
        {
            var word = reader.FieldCount == 1 ? "field" : "fields";
            Console.WriteLine($"{reader.FieldCount} {word}:");

            for (int i = 0; i < reader.FieldCount; ++i)
            {
                var text = Encoding.UTF8.GetString(reader.GetField(i));
                Console.WriteLine("  - " + text);
            }
        }
    }

    static void SimpleUdp(string[] args)
    {
        var port = int.Parse(args[0]);
        if (1 < args.Length)
        {
            var targetInfo = AddressInfo.Get(args[1], args[0]);
            var target = targetInfo.V6[0];
            Console.WriteLine($"Targeting {target}...");
            using var socket = UdpSocketV6.CreateWithoutBinding();

            while (true)
            {
                Console.WriteLine("Enter message:");
                var message = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(message))
                    break;

                var bytes = Encoding.UTF8.GetBytes(message);
                socket.Send(bytes, target);
            }
        }
        else
        {
            // Server
            bool keepListening = true;
            Console.CancelKeyPress += (e, ea) =>
            {
                ea.Cancel = true;
                keepListening = false;
                Console.WriteLine("Quitting...");
            };

            using var socket = UdpSocketV6.BindAnyIp(port, true);
            var endpoint = socket.GetEndpoint();
            Console.WriteLine($"Listening on {endpoint}...");
            var buffer = new byte[4096];
            while (keepListening)
            {
                var n = socket.Receive(buffer, out var origin, TimeSpan.FromSeconds(1));

                if (origin.IsDefault)
                    continue;

                var message = Encoding.UTF8.GetString(buffer, 0, n);
                Console.WriteLine($"Received from {origin}: {message}");
            }
        }
    }

    static void QuadContains()
    {
        var q = Quad.Create(
            new Vector2(0f, 1f),
            new Vector2(1f, 2f),
            new Vector2(2f, 1f),
            new Vector2(1f, 0f));

        Console.WriteLine(q.ToString());

        while (true)
        {
            Console.WriteLine("Enter test:");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                break;

            var space = input.IndexOf(' ');
            if (space == -1)
                continue;

            if (!float.TryParse(input.AsSpan(0, space), out var x))
                continue;

            if (!float.TryParse(input.AsSpan(space + 1), out var y))
                continue;

            var v = new Vector2(x, y);
            Console.WriteLine(q.Contains(v));
        }
    }

    static void TryOutV6()
    {
        // var clientInfo = socketProvider.GetAddressInfo(null, null);
        // Dump(clientInfo);

        var serverInfo = AddressInfo.Get(null, "12345");
        Dump(serverInfo);

        using var myServer = UdpSocketV6.Bind(serverInfo.V6[0]);
        // using var myServer = socketProvider.CreateAndBindUdpSocketV6(new Endpoint<AddressV6>(AddressV6.Create(span => span.Fill((byte)0xff)), 1));
        Console.WriteLine("Server bound!");
        var serverEndpoint = myServer.GetEndpoint();
        Console.WriteLine(serverEndpoint);

        using var myClient = UdpSocketV6.CreateWithoutBinding();
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

