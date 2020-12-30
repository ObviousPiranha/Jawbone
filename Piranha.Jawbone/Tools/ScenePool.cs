using System.Collections.Concurrent;
using System.Threading;

namespace Piranha.Jawbone.Tools
{
    public sealed class ScenePool<T> where T : class, new()
    {
        private readonly ConcurrentBag<T> _pool = new();
        private T? _latest = null;
        private int _roamCount = 0;

        public bool Closed { get; set; }
        public int CreateCount { get; private set; }
        public int RoamCount => _roamCount;
        public int StaleCount { get; private set; }

        public T? TakeLatestScene() => Interlocked.Exchange(ref _latest, null);
        public void ReturnScene(T scene)
        {
            _pool.Add(scene);
            Interlocked.Decrement(ref _roamCount);
        }

        public T AcquireScene()
        {
            if (!_pool.TryTake(out var scene))
            {
                scene = new();
                ++CreateCount;
            }

            Interlocked.Increment(ref _roamCount);
            
            return scene;
        }

        public bool SetLatestScene(T scene)
        {
            var staleScene = Interlocked.Exchange(ref _latest, scene);

            if (staleScene is null)
            {
                return true;
            }
            else
            {
                _pool.Add(staleScene);
                Interlocked.Decrement(ref _roamCount);
                ++StaleCount;
                return false;
            }
        }
    }
}
