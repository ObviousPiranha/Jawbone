using System;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Net;

namespace Piranha.Jawbone.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // var serviceCollection = new ServiceCollection();

            // var options = new ServiceProviderOptions
            // {
            //     ValidateOnBuild = true,
            //     ValidateScopes = true
            // };

            // serviceCollection.AddNetworking();

            // using var serviceProvider = serviceCollection.BuildServiceProvider(options);
            // var networkProvider = serviceProvider.GetRequiredService<NetworkProvider>();

            // Console.WriteLine("Endpoint<Address32> size: " + Unsafe.SizeOf<Endpoint<Address32>>());

            // using var serverSocket = networkProvider.CreateAndBindUdpV4Socket(new Endpoint<Address32>(default, 9999));
            // using var clientSocket = networkProvider.CreateAndBindUdpV4Socket(default);

            // Console.WriteLine("Server endpoint: " + serverSocket.GetEndpoint());
            // Console.WriteLine("Client endpoint: " + clientSocket.GetEndpoint());

            // var message = Encoding.UTF8.GetBytes("Brian is bad.");
            // var destination = new Endpoint<Address32>(Address32.Local, 9999);
            // clientSocket.Send(message, destination);

            // var buffer = new byte[2048];
            // var length = serverSocket.Receive(buffer, out var origin);
            // var receivedMessage = Encoding.UTF8.GetString(buffer.AsSpan(0, length));
            // Console.WriteLine("Received message from " + origin);
            // Console.WriteLine(receivedMessage);
            var summary = BenchmarkRunner.Run<SocketBenchmark>();
        }
    }
}
