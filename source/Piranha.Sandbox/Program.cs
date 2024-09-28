using Piranha.Jawbone;
using Piranha.Jawbone.Net;
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
            var serverEndpoint = AddressV4.Local.OnPort(7777);
            using var server = LinuxUdpSocketV4.Bind(serverEndpoint);
            using var client = LinuxUdpSocketV4.Create();

            client.Send("Hello, IPv4!"u8, serverEndpoint);

            var buffer = new byte[2048];
            var n = server.Receive(buffer, out var origin, TimeSpan.FromSeconds(2));

            if (n == 0)
            {
                Console.WriteLine("Timed out.");
            }
            else
            {
                var text = Encoding.UTF8.GetString(buffer.AsSpan(0, n));
                Console.WriteLine("Received: " + text);
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
