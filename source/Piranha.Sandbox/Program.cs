using Piranha.Jawbone;
using Piranha.Jawbone.Net;
using Piranha.Jawbone.Net.Unix;
using Piranha.Jawbone.Sdl2;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
