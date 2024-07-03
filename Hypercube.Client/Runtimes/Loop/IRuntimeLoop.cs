namespace Hypercube.Client.Runtimes.Loop;

public interface IRuntimeLoop
{
    bool Running { get; }
    void Run();
    void Shutdown();
}