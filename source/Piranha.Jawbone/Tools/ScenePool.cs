using System.Collections.Concurrent;
using System.Threading;

namespace Piranha.Jawbone;

public sealed class ScenePool<T> where T : class, new()
{
    private readonly ConcurrentQueue<T> _pool = new();
    private T? _latest = null;
    private int _roamCount = 0;

    public bool Closed { get; set; }
    public int CreateCount { get; private set; }
    public int RoamCount => _roamCount;
    public bool HasNewScene => _latest is not null;
    public int StaleCount { get; private set; }

    public T? TakeLatestScene() => Interlocked.Exchange(ref _latest, null);

    public T? GetLatestScene(T? currentScene)
    {
        var latestScene = TakeLatestScene();

        if (latestScene is not null)
        {
            if (currentScene is not null)
                ReturnScene(currentScene);

            return latestScene;
        }
        else
        {
            return currentScene;
        }
    }

    public void ReturnScene(T scene)
    {
        _pool.Enqueue(scene);
        Interlocked.Decrement(ref _roamCount);
    }

    public T AcquireScene()
    {
        if (!_pool.TryDequeue(out var scene))
        {
            scene = new();
            ++CreateCount;
        }

        Interlocked.Increment(ref _roamCount);

        return scene;
    }

    public bool SetLatestScene(T? scene)
    {
        var staleScene = Interlocked.Exchange(ref _latest, scene);

        if (staleScene is null)
        {
            return true;
        }
        else
        {
            _pool.Enqueue(staleScene);
            Interlocked.Decrement(ref _roamCount);
            ++StaleCount;
            return false;
        }
    }
}
