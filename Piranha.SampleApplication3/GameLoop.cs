using Microsoft.Extensions.Logging;
using Piranha.Jawbone;
using Piranha.Jawbone.Sdl3;
using System;
using System.Numerics;

namespace Piranha.SampleApplication3;

class GameLoop : IGameLoop
{
    private const int CycleFrameCount = 240;
    private readonly Random _random = new();
    private readonly ILogger<GameLoop> _logger;
    private readonly ScenePool<PiranhaScene> _scenePool;
    private readonly IAudioManager _audioManager;
    private int _staleCount = 0;
    private int _frameCount = 0;
    private Vector4 _startColor;
    private Vector4 _endColor;

    public bool Running { get; private set; } = true;

    public GameLoop(
        ILogger<GameLoop> logger,
        ScenePool<PiranhaScene> scenePool,
        IAudioManager audioManager)
    {
        _logger = logger;
        _scenePool = scenePool;
        _audioManager = audioManager;

        _startColor = RandomColor();
        _endColor = RandomColor();
    }

    public void FrameUpdate()
    {
        _audioManager.PumpAudio();
        if (CycleFrameCount <= ++_frameCount)
        {
            _startColor = _endColor;
            _endColor = RandomColor();
            _frameCount = 0;
        }

        if (_scenePool.Closed)
            Running = false;
    }

    public void PrepareScene()
    {
        var scene = _scenePool.AcquireScene();

        var t = _frameCount / (float)CycleFrameCount;
        scene.Color = Vector4.Lerp(_startColor, _endColor, t);

        var radians = t * 2f * (float)Math.PI;
        var matrix = Matrix4x4.CreateRotationZ(radians);
        var positions = Quad.Create(new Vector2(-1F, 0.5F), new Vector2(1F, -0.5F)).Transformed(matrix);
        var textureCoordinates = SampleHandler.PiranhaSprite.ToTextureCoordinates(new Point32(512, 512));
        scene.VertexData.Clear();
        scene.VertexData.Add(positions, textureCoordinates);

        if (!_scenePool.SetLatestScene(scene))
        {
            ++_staleCount;
            // _logger.LogWarning("Stale scene ({staleCount})", _staleCount);
        }
    }

    private Vector4 RandomColor()
    {
        return new Vector4(
            (float)_random.NextDouble(),
            (float)_random.NextDouble(),
            (float)_random.NextDouble(),
            1.0f);
    }

    public void Close()
    {
    }
}
