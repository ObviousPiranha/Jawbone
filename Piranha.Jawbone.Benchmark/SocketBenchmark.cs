using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Jawbone.Net;

namespace Piranha.Jawbone.Benchmark
{
    [MemoryDiagnoser(false)]
    public class SocketBenchmark
    {
        private const int Port = 11111;

        private static readonly Endpoint<Address32> JawboneDestination = new Endpoint<Address32>(Address32.Local, Port);
        private static readonly IPEndPoint DotNetDestination = new IPEndPoint(IPAddress.Loopback, Port);

        private readonly Random _random = new Random();
        private readonly byte[] _sendBuffer = new byte[713];
        private readonly byte[] _receiveBuffer = new byte[2048];

        private ServiceProvider _serviceProvider;
        private UdpSocket _serverSocket;
        private UdpSocket _jawboneClientSocket;
        private Socket _dotNetClientSocket;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var serviceCollection = new ServiceCollection();

            var options = new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            };

            serviceCollection.AddSocketProvider();

            _serviceProvider = serviceCollection.BuildServiceProvider(options);
            var socketProvider = _serviceProvider.GetRequiredService<SocketProvider>();

            _serverSocket = socketProvider.CreateAndBindUdpV4Socket(new Endpoint<Address32>(default, Port));
            _jawboneClientSocket = socketProvider.CreateAndBindUdpV4Socket(default);
            _dotNetClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _dotNetClientSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            _random.NextBytes(_sendBuffer);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _dotNetClientSocket.Dispose();
            _jawboneClientSocket.Dispose();
            _serverSocket.Dispose();
            _serviceProvider.Dispose();
        }

        // [IterationCleanup]
        public void IterationCleanup()
        {
            int n = _serverSocket.Receive(_receiveBuffer, out var origin);
            if (n != _sendBuffer.Length)
                throw new Exception($"Hey, I didn't get the right number of bytes. Expected {_sendBuffer.Length} Actual {n}");
            
            if (!_receiveBuffer.AsSpan(0, n).SequenceEqual(_sendBuffer))
                throw new Exception("They didn't match.");
        }

        [Benchmark(Baseline = true)]
        public void SendDotNet()
        {
            int n = _dotNetClientSocket.SendTo(_sendBuffer, DotNetDestination);
            if (n != _sendBuffer.Length)
                throw new Exception("You lied to me.");
            
            IterationCleanup();
        }

        [Benchmark]
        public void SendJawbone()
        {
            int n = _jawboneClientSocket.Send(_sendBuffer, JawboneDestination);
            if (n != _sendBuffer.Length)
                throw new Exception("You lied to me.");
            
            IterationCleanup();
        }
    }
}
