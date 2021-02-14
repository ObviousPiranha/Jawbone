using System;
using System.Numerics;
using Microsoft.Extensions.Logging;
using Piranha.Jawbone.Sdl;
using Piranha.Jawbone.Tools;

namespace Piranha.TestApplication
{
    class GameLoop : IGameLoop
    {
        private const int CycleFrameCount = 240;
        private readonly Random _random = new();
        private readonly ILogger<GameLoop> _logger;
        private readonly ScenePool<PiranhaScene> _scenePool;

        private int _staleCount = 0;
        private int _frameCount = 0;
        private Vector4 _startColor;
        private Vector4 _endColor;

        public bool Running { get; private set; } = true;

        public GameLoop(
            ILogger<GameLoop> logger,
            ScenePool<PiranhaScene> scenePool)
        {
            _logger = logger;
            _scenePool = scenePool;

            _startColor = RandomColor();
            _endColor = RandomColor();
        }

        public void FrameUpdate()
        {
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
            var positions = Quadrilateral.Create(new Vector2(-1F, 0.5F), new Vector2(1F, -0.5F)).Transformed(matrix);
            var textureCoordinates = MyTestHandler.PiranhaSprite.ToTextureCoordinates(new Point32(512, 512));
            var n = 0;
            scene.VertexData[n++] = positions.A.X;
            scene.VertexData[n++] = positions.A.Y;
            scene.VertexData[n++] = textureCoordinates.A.X;
            scene.VertexData[n++] = textureCoordinates.A.Y;

            scene.VertexData[n++] = positions.B.X;
            scene.VertexData[n++] = positions.B.Y;
            scene.VertexData[n++] = textureCoordinates.B.X;
            scene.VertexData[n++] = textureCoordinates.B.Y;

            scene.VertexData[n++] = positions.C.X;
            scene.VertexData[n++] = positions.C.Y;
            scene.VertexData[n++] = textureCoordinates.C.X;
            scene.VertexData[n++] = textureCoordinates.C.Y;

            scene.VertexData[n++] = positions.A.X;
            scene.VertexData[n++] = positions.A.Y;
            scene.VertexData[n++] = textureCoordinates.A.X;
            scene.VertexData[n++] = textureCoordinates.A.Y;

            scene.VertexData[n++] = positions.C.X;
            scene.VertexData[n++] = positions.C.Y;
            scene.VertexData[n++] = textureCoordinates.C.X;
            scene.VertexData[n++] = textureCoordinates.C.Y;

            scene.VertexData[n++] = positions.D.X;
            scene.VertexData[n++] = positions.D.Y;
            scene.VertexData[n++] = textureCoordinates.D.X;
            scene.VertexData[n++] = textureCoordinates.D.Y;
            
            if (!_scenePool.SetLatestScene(scene))
            {
                ++_staleCount;
                _logger.LogWarning("Stale scene ({0})", _staleCount);
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
}
