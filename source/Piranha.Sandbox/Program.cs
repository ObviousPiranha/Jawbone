using Piranha.Jawbone;
using Piranha.Jawbone.Net;
using Piranha.Jawbone.Png;
using System;
using System.IO;
using System.Text;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            NetworkTest();
            BindTest();
            // PngTest(args[0]);
            V6Shenanigans();
            TcpV4Shenanigans();
            TcpV6Shenanigans();
            TryUdpClientV4();
            TryUdpClientV6();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            if (ex is SocketException se)
                Console.WriteLine(se.Code.ToString());
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    static void TryUdpClientV4()
    {
        using var server = UdpSocketV4.BindLocalIp();
        var endpoint = server.GetSocketName();
        using var client = UdpClientV4.Connect(endpoint);
        Console.WriteLine($"Client: {client.GetSocketName()}");
        client.Send("YO YO"u8);
        Span<byte> buffer = new byte[1024];
        server.Receive(ref buffer, TimeSpan.FromSeconds(1));
        var text = Encoding.UTF8.GetString(buffer);
        Console.WriteLine(text);
    }

    static void TryUdpClientV6()
    {
        using var server = UdpSocketV6.BindLocalIp();
        var endpoint = server.GetSocketName();
        using var client = UdpClientV6.Connect(endpoint);
        Console.WriteLine($"Client: {client.GetSocketName()}");
        client.Send("YO YO"u8);
        Span<byte> buffer = new byte[1024];
        server.Receive(ref buffer, TimeSpan.FromSeconds(1));
        var text = Encoding.UTF8.GetString(buffer);
        Console.WriteLine(text);
    }

    static void TcpV4Shenanigans()
    {
        using var listener = TcpListenerV4.Listen(AddressV4.Local.OnAnyPort(), 4);
        var endpoint = listener.GetSocketName();
        Console.WriteLine($"Listening on {endpoint}...");
        Console.WriteLine($"Connecting client to {endpoint}...");
        using var client = TcpClientV4.Connect(endpoint);
        var clientSocketName = client.GetSocketName();
        Console.WriteLine($"Client listening on {clientSocketName}.");
        Console.WriteLine("Accepting connection...");
        using var server = listener.Accept(TimeSpan.FromSeconds(2));
        if (server is null)
        {
            Console.WriteLine("Failed to accept connection.");
            return;
        }
        Console.WriteLine("Client sending message...");
        client.Send("HERRO"u8);
        Console.WriteLine("Server receiving message...");
        var buffer = new byte[1024];
        var n = server.Receive(buffer, TimeSpan.FromSeconds(1));
        if (!n.HasValue)
        {
            Console.WriteLine("Failed to receive message.");
            return;
        }
        ReadOnlyUtf8Span message = buffer.AsSpan(0, n.Value);
        Console.WriteLine("Server received message: " + message.ToString());
    }

    static void TcpV6Shenanigans()
    {
        using var listener = TcpListenerV6.Listen(AddressV6.Local.OnAnyPort(), 4);
        var endpoint = listener.GetSocketName();
        Console.WriteLine($"Listening on {endpoint}...");
        Console.WriteLine($"Connecting client to {endpoint}...");
        using var client = TcpClientV6.Connect(endpoint);
        var clientSocketName = client.GetSocketName();
        Console.WriteLine($"Client listening on {clientSocketName}.");
        Console.WriteLine("Accepting connection...");
        using var server = listener.Accept(TimeSpan.FromSeconds(2));
        if (server is null)
        {
            Console.WriteLine("Failed to accept connection.");
            return;
        }
        Console.WriteLine("Client sending message...");
        client.Send("HERRO"u8);
        Console.WriteLine("Server receiving message...");
        var buffer = new byte[1024];
        var n = server.Receive(buffer, TimeSpan.FromSeconds(1));
        if (!n.HasValue)
        {
            Console.WriteLine("Failed to receive message.");
            return;
        }
        ReadOnlyUtf8Span message = buffer.AsSpan(0, n.Value);
        Console.WriteLine("Server received message: " + message.ToString());
    }

    static void V6Shenanigans()
    {
        using var v4 = UdpSocketV4.BindLocalIp();
        var endpoint = v4.GetSocketName();
        Console.WriteLine("Server: " + endpoint);
        using var v6 = UdpSocketV6.Create(allowV4: true);
        v6.Send("Hello, IPv4!"u8, endpoint.MapToV6());
        Console.WriteLine("Client: " + v6.GetSocketName());
        Span<byte> buffer = new byte[2048];
        v4.Receive(ref buffer, TimeSpan.FromSeconds(2), out var origin);
        var message = Encoding.UTF8.GetString(buffer);
        Console.WriteLine($"Received from {origin}: {message}");
    }

    static void PngTest(string path)
    {
        var bytes = File.ReadAllBytes(path);
        Png.DebugWalk(bytes);
    }

    static void BindTest()
    {
        var port = 7777;
        using var socket6A = UdpSocketV6.BindAnyIp(port);
        using var socket4A = UdpSocketV4.BindAnyIp(port);

        using var socket6B = UdpSocketV6.Create();
        using var socket4B = UdpSocketV4.Create();

        socket6B.Send("IPv6"u8, AddressV6.Local.OnPort(port));
        socket4B.Send("IPv4"u8, AddressV4.Local.OnPort(port));

        var buffer = new byte[64];
        var timeout = TimeSpan.FromSeconds(1);
        Span<byte> span = buffer;
        socket6A.Receive(ref span, timeout);
        Console.WriteLine(Encoding.UTF8.GetString(span));

        span = buffer;
        socket4A.Receive(ref span, timeout);
        Console.WriteLine(Encoding.UTF8.GetString(span));

        Console.WriteLine("Huh. Wow.");
    }

    static void NetworkTest()
    {
        var addressInfo = AddressInfo.Get("google.com");
        Console.WriteLine("V4: " + string.Join(", ", addressInfo.V4));
        Console.WriteLine("V6: " + string.Join(", ", addressInfo.V6));

        var port = 7777;
        // var serverEndpoint = AddressV4.Local.OnPort(2);
        using var server = UdpSocketV6.BindAnyIp(port, true);
        // using var oops = LinuxUdpSocketV4.Bind(serverEndpoint);
        using var client = UdpSocketV4.Create();

        var serverSocketName = server.GetSocketName();
        Console.WriteLine("Server socket name: " + serverSocketName);
        var clientSocketName = client.GetSocketName();
        Console.WriteLine("Client socket name: " + clientSocketName);
        client.Send("Hello, IPv6!"u8, AddressV4.Local.OnPort(port));
        clientSocketName = client.GetSocketName();
        Console.WriteLine("Client socket name: " + clientSocketName);

        var buffer = new byte[2048];
        Console.WriteLine("Begin receive...");
        var result = server.Receive(buffer, TimeSpan.FromSeconds(2), out var origin);

        if (!result.HasValue)
        {
            Console.WriteLine("Timed out.");
        }
        else
        {
            var text = Encoding.UTF8.GetString(buffer.AsSpan(0, result.Value));
            Console.WriteLine($"Received from {origin}: {text}");
        }
    }
}
