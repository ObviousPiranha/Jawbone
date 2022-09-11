using System;
using System.IO;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;

namespace Piranha.Jawbone.Benchmark;

[MemoryDiagnoser(false)]
public class AesBenchmark
{
    private readonly Aes _aes = Aes.Create();
    private readonly byte[] _originalMessage = new byte[999];
    private readonly byte[] _buffer = new byte[4096];
    private readonly byte[] _encryptedMessage;
    private readonly byte[] _iv;
    private readonly byte[] _key;

    public AesBenchmark()
    {
        _aes.GenerateIV();
        _aes.GenerateKey();
        _iv = _aes.IV;
        _key = _aes.Key;
        _encryptedMessage = _aes.EncryptCbc(_originalMessage, _iv);
    }

    [Benchmark]
    public void EncryptSpan()
    {
        for (int i = 0; i < 4; ++i)
        {
            int length = _aes.EncryptCbc(_originalMessage, _iv, _buffer);

            if (!_buffer.AsSpan(0, length).SequenceEqual(_encryptedMessage))
                throw new Exception("Results do not match");
        }
    }

    [Benchmark]
    public void EncryptStream()
    {
        for (int i = 0; i < 4; ++i)
        {
            using var inputStream = new MemoryStream(_originalMessage);
            using var outputStream = new MemoryStream(_buffer);
            
            using (var encryptionStream = new CryptoStream(inputStream, _aes.CreateEncryptor(_key, _iv), CryptoStreamMode.Read))
                encryptionStream.CopyTo(outputStream);

            if (!_buffer.AsSpan(0, (int)outputStream.Position).SequenceEqual(_encryptedMessage))
                throw new Exception("Results do not match");
        }
    }
}
