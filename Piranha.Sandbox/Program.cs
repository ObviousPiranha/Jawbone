﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Piranha.Jawbone.Collections;
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

        var byteBuffer = new ByteBuffer();
        var endpoints = new Endpoint<Address128>[16];
        var randomAddress = Address128.Create(span => RandomNumberGenerator.Fill(span));
        endpoints.AsSpan().Fill(Endpoint.Create(randomAddress, 200));
        byteBuffer.AddAllAsBytes(endpoints);

        Console.WriteLine(Address128.Map(Address32.Local));
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
        var info1 = AddressInfo.Get("google.com", "443");
        Dump(info1);
        var info2 = AddressInfo.Get("thebuzzsaw.duckdns.org", null);
        Dump(info2);
        var info3 = AddressInfo.Get("192.168.50.1", "8080");
        Dump(info3);

        Console.WriteLine("yo yo yo");
    }

    static void Main(string[] args)
    {
        try
        {
            AddressShenanigans();
            TryOutV6();

            if (1 < args.Length)
            {
                var info = AddressInfo.Get(args[0], args[1]);
                var endpoint = info.V4[0];
                
                using var client = new UdpSocket32(default);
                Console.WriteLine("Client bound on " + client.GetEndpoint().ToString());
                var message = Encoding.UTF8.GetBytes("Greetings!");
                client.Send(message, endpoint);
                Console.WriteLine("Message sent to " + endpoint.ToString());
            }
            else if (args.Length == 1)
            {
                var port = int.Parse(args[0]);
                var endpoint = Endpoint.Create(Address32.Any, port);
                using var server = new UdpSocket32(endpoint);
                Console.WriteLine("Listening on " + endpoint.ToString());
                var buffer = new byte[4096];

                using var cancellationTokenSource = new CancellationTokenSource();

                Console.CancelKeyPress += (sender, e) =>
                    {
                        e.Cancel = true;
                        Console.WriteLine("Exiting...");
                        cancellationTokenSource.Cancel();
                    };

                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var n = server.Receive(buffer, out var origin, TimeSpan.FromSeconds(0.5));

                    if (!origin.IsDefault)
                    {
                        var message = Encoding.UTF8.GetString(buffer.AsSpan(0, n));
                        Console.WriteLine(origin.ToString() + " >>> " + message);
                    }
                }

                Console.WriteLine("Done.");
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

    static void TryOutV6()
    {
        // var clientInfo = socketProvider.GetAddressInfo(null, null);
        // Dump(clientInfo);
        
        var serverInfo = AddressInfo.Get(null, "12345");
        Dump(serverInfo);

        using var myServer = new UdpSocket128(serverInfo.V6[0]);
        // using var myServer = socketProvider.CreateAndBindUdpSocket128(new Endpoint<Address128>(Address128.Create(span => span.Fill((byte)0xff)), 1));
        Console.WriteLine("Server bound!");
        var serverEndpoint = myServer.GetEndpoint();
        Console.WriteLine(serverEndpoint);

        using var myClient = new UdpSocket128(default);
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

