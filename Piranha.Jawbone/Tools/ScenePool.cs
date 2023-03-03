using System.Threading;

namespace Piranha.Jawbone.Tools;

public sealed class ScenePool<T> where T : class, new()
{
    private T? _latest = null;
    private T? _pool = null;
    private int _roamCount = 0;

    public bool Closed { get; set; }
    public int CreateCount { get; private set; }
    public int RoamCount => _roamCount;
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
        Interlocked.Exchange(ref _pool, scene);
        Interlocked.Decrement(ref _roamCount);
    }

    public T AcquireScene()
    {
        var scene = Interlocked.Exchange(ref _pool, null);
        if (scene is null)
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
            Interlocked.Exchange(ref _pool, staleScene);
            Interlocked.Decrement(ref _roamCount);
            ++StaleCount;
            return false;
        }
    }
}
