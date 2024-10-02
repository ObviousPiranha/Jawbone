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
            // NetworkTest();
            // BindTest();
            PngTest(args[0]);
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
        socket6A.Receive(buffer, timeout, out var result6);
        result6.ThrowOnErrorOrTimeout();
        Console.WriteLine(Encoding.UTF8.GetString(buffer.AsSpan(0, result6.ReceivedByteCount)));

        socket4A.Receive(buffer, timeout, out var result4);
        result4.ThrowOnErrorOrTimeout();
        Console.WriteLine(Encoding.UTF8.GetString(buffer.AsSpan(0, result4.ReceivedByteCount)));

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
        server.Receive(buffer, TimeSpan.FromSeconds(2), out var result);
        result.ThrowOnError();

        if (result.State == UdpReceiveState.Timeout)
        {
            Console.WriteLine("Timed out.");
        }
        else
        {
            var text = Encoding.UTF8.GetString(buffer.AsSpan(0, result.ReceivedByteCount));
            Console.WriteLine($"Received from {result.Origin}: {text}");
        }
    }
}
