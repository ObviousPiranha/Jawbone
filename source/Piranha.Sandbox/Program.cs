using Piranha.Jawbone;
using Piranha.Jawbone.Net;
using Piranha.Jawbone.Png;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (0 < args.Length)
            {
                if (2 < args.Length)
                {
                    RunClient(args[0], args[1], args[2]);
                }
                else
                {
                    RunServer(int.Parse(args[0]));
                }
            }
            else
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

    static void RunServer(int port)
    {
        using var cancellationTokenSource = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                Console.WriteLine("Stopping server...");
                e.Cancel = true;
                cancellationTokenSource.Cancel();
            }
        };

        using var listener = TcpListenerV4.Listen(AddressV4.Any.OnPort(port), 1);
        Console.WriteLine("Listening on port " + port);
        var timeout = TimeSpan.FromSeconds(1);

        var buffer = new byte[2048];
        while (!cancellationTokenSource.IsCancellationRequested)
        {
            using var server = Accept();

            if (server is null)
                continue;

            var name = server.GetSocketName();
            Console.WriteLine("Connected to " + name);
            var result = server.Receive(buffer, timeout);

            if (result.HasValue)
            {
                var message = buffer.AsSpan(0, result.Value);
                var asText = Encoding.UTF8.GetString(message);
                Console.WriteLine("Client sent: " + asText);
                message.Reverse();
                server.Send(message);
            }
            else
            {
                Console.WriteLine("Connection timed out.");
            }
        }

        ITcpClient<AddressV4>? Accept()
        {
            try
            {
                var result = listener.Accept(timeout);
                return result;
            }
            catch (SocketException ex)
            {
                if (!ex.Code.Name.Contains("EINTR"))
                    throw;
                return null;
            }
        }
    }

    static void RunClient(string port, string host, string message)
    {
        var info = AddressInfo.Get(host, port);
        var destination = info.V4[0];
        Console.WriteLine("Connecting to " + destination);
        using var client = TcpClientV4.Connect(destination);

        Console.WriteLine("Connected! Sending: " + message);
        var utf8 = Encoding.UTF8.GetBytes(message);
        client.Send(utf8);

        var buffer = new byte[2048];
        var result = client.Receive(buffer, TimeSpan.FromSeconds(4));
        if (result.HasValue)
        {
            var bytes = buffer.AsSpan(0, result.Value);
            var received = Encoding.UTF8.GetString(bytes);
            Console.WriteLine("Received: " + received);
        }
        else
        {
            Console.WriteLine("Timed out waiting for response.");
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
        client.Send("Hello, IPv6!"u8, AddressV4.Local.OnPort(port));
        var clientSocketName = client.GetSocketName();
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
