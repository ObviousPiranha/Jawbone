using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Piranha.Jawbone.Net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class SocketSendReceiveBenchmark : IDisposable
{
    private static readonly TimeSpan s_timeout = TimeSpan.FromSeconds(1);

    private readonly byte[] _message = new byte[512];
    private readonly byte[] _buffer = new byte[2048];
    private readonly Socket _clientSocket;
    private readonly Socket _serverSocket;
    private readonly IPEndPoint _serverSocketDestination;
    private readonly IUdpSocket<AddressV6> _clientJawbone;
    private readonly IUdpSocket<AddressV6> _serverJawbone;
    private readonly Endpoint<AddressV6> _serverJawboneDestination;

    public SocketSendReceiveBenchmark()
    {
        RandomNumberGenerator.Fill(_message);

        var bindEndpoint = new IPEndPoint(IPAddress.IPv6Loopback, 0);
        _clientSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        _serverSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        _serverSocket.Bind(bindEndpoint);
        _serverSocketDestination = (IPEndPoint)_serverSocket.LocalEndPoint!;

        _clientJawbone = UdpSocketV6.Create();
        _serverJawbone = UdpSocketV6.BindLocalIp();
        _serverJawboneDestination = _serverJawbone.GetSocketName();
    }

    [GlobalSetup]
    public void GlobalSetup()
    {

    }

    public void Dispose()
    {
        _serverJawbone.Dispose();
        _clientJawbone.Dispose();
        _serverSocket.Dispose();
        _clientSocket.Dispose();
    }

    private static void Throw()
    {
        throw new InvalidDataException();
    }

    private void Validate(int n)
    {
        if (n != _message.Length)
            Throw();
    }

    [Benchmark]
    public void RunSocket()
    {
        _clientSocket.SendTo(_message, _serverSocketDestination);
        EndPoint ep = _serverSocketDestination;
        var n = _serverSocket.ReceiveFrom(_buffer, ref ep);
        Validate(n);
    }

    [Benchmark(Baseline = true)]
    public void RunJawbone()
    {
        _clientJawbone.Send(_message, _serverJawboneDestination);
        _serverJawbone.Receive(_buffer, s_timeout, out var result);
        result.ThrowOnErrorOrTimeout();
        Validate(result.ReceivedByteCount);
    }
}
