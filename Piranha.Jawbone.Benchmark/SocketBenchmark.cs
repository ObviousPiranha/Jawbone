using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Piranha.Jawbone.Net;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class SocketBenchmark
{
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(1);
    private readonly byte[] _sendBuffer = new byte[713];
    private readonly byte[] _receiveBuffer = new byte[2048];

    [GlobalSetup]
    public void GlobalSetup()
    {
        RandomNumberGenerator.Fill(_sendBuffer);
    }

    private void Validate(int n)
    {
        if (n != _sendBuffer.Length)
            Throw();

        static void Throw() => throw new Exception("Didn't receive correct number of bytes.");
    }

    [Benchmark]
    public void SendUdpClient()
    {
        var bindTarget = new IPEndPoint(IPAddress.Any, 0);
        using var serverUdp = new UdpClient(bindTarget);
        var serverEndpoint = (IPEndPoint)serverUdp.Client.LocalEndPoint!;

        {
            using var clientUdp = new UdpClient();
            var destination = new IPEndPoint(IPAddress.Loopback, serverEndpoint.Port);
            clientUdp.Send(_sendBuffer, destination);
        }

        var ep = default(IPEndPoint);
        var received = serverUdp.Receive(ref ep);
        Validate(received.Length);
    }

    [Benchmark]
    public void SendSocket()
    {
        using var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        var bindTarget = new IPEndPoint(IPAddress.Any, 0);
        serverSocket.Bind(bindTarget);
        var serverEndpoint = (IPEndPoint)serverSocket.LocalEndPoint!;

        {
            using var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clientSocket.SendTo(_sendBuffer, serverEndpoint);
        }

        EndPoint ep = serverEndpoint;
        var n = serverSocket.ReceiveFrom(_receiveBuffer, ref ep);
        Validate(n);
    }

    [Benchmark(Baseline = true)]
    public void SendJawbone()
    {
        using var serverSocket = UdpSocketV4.BindAnyIp();
        var serverEndpoint = serverSocket.GetEndpoint();

        {
            using var clientSocket = UdpSocketV4.CreateWithoutBinding();
            clientSocket.Send(_sendBuffer, AddressV4.Local.OnPort(serverEndpoint.Port));
        }

        var n = serverSocket.Receive(_receiveBuffer, out _, _timeout);
        Validate(n);
    }
}
