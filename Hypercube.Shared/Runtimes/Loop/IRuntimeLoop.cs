namespace Hypercube.Shared.Runtimes.Loop;

public interface IRuntimeLoop
{
    bool Running { get; }
    void Run();
    void Shutdown();
}