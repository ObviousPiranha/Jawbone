using Piranha.Jawbone.Net;
using Piranha.Jawbone.Net.Unix;
using Piranha.Jawbone.Net.Windows;
using System;
using System.Text;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // UnixNetworkTest();
            WindowsNetworkTest();
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

    static void WindowsNetworkTest()
    {
        var addressInfo = WindowsAddressInfo.Get("google.com");
        Console.WriteLine("V4: " + string.Join(", ", addressInfo.V4));
        Console.WriteLine("V6: " + string.Join(", ", addressInfo.V6));

        var port = 7777;
        // var serverEndpoint = AddressV4.Local.OnPort(2);
        using var server = WindowsUdpSocketV6.BindAnyIp(port, true);
        // using var oops = UnixUdpSocketV4.Bind(serverEndpoint);
        using var client = WindowsUdpSocketV4.Create();

        var serverSocketName = server.GetSocketName();
        Console.WriteLine("Server socket name: " + serverSocketName);
        // var clientSocketName = client.GetSocketName();
        // Console.WriteLine("Client socket name: " + clientSocketName);
        client.Send("Hello, IPv6!"u8, AddressV4.Local.OnPort(port));
        var clientSocketName = client.GetSocketName();
        Console.WriteLine("Client socket name: " + clientSocketName);

        var buffer = new byte[2048];
        Console.WriteLine("Begin receive...");
        server.Receive(buffer, TimeSpan.FromSeconds(2), out var result);

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

    static void UnixNetworkTest()
    {
        var addressInfo = UnixAddressInfo.Get("google.com");
        Console.WriteLine("V4: " + string.Join(", ", addressInfo.V4));
        Console.WriteLine("V6: " + string.Join(", ", addressInfo.V6));

        var port = 7777;
        // var serverEndpoint = AddressV4.Local.OnPort(2);
        using var server = UnixUdpSocketV6.BindAnyIp(port, true);
        // using var oops = UnixUdpSocketV4.Bind(serverEndpoint);
        using var client = UnixUdpSocketV4.Create();

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
