using Hypercube.Core.Analyzers;

namespace Hypercube.Core.Ecs.Core.Utilities;

[EngineCore]
public class IntPool
{
    private readonly Stack<int> _released = new();
    private int _counter;

    public int Next => _released.Count > 0 ? _released.Pop() : _counter++;

    public void Release(int value)
    {
        if (value < 0 || value > _counter || _released.Contains(value))
            throw new ArgumentException("Invalid integer to release.");

        _released.Push(value);
    }
}