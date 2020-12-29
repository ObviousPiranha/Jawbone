using System.Collections.Concurrent;
using System.Threading;

namespace Piranha.Jawbone.Tools
{
    public sealed class ScenePool<T> where T : class, new()
    {
        private readonly ConcurrentBag<T> _pool = new();
        private T? _latest = null;

        public bool Closed { get; set; }

        public T? TakeLatestScene() => Interlocked.Exchange(ref _latest, null);
        public void ReturnScene(T scene) => _pool.Add(scene);
        public T AcquireScene()
        {
            if (!_pool.TryTake(out var scene))
                scene = new();
            
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
                return false;
            }
        }
    }
}
