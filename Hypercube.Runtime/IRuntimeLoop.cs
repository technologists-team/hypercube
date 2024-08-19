using JetBrains.Annotations;

namespace Hypercube.Runtime;

[PublicAPI]
public interface IRuntimeLoop
{
    bool Running { get; }
    void Run();
    void Shutdown();
}