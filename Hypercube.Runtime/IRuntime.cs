using JetBrains.Annotations;

namespace Hypercube.Runtime;

[PublicAPI]
public interface IRuntime
{
    /// <summary>
    /// Start Hypercube.
    /// </summary>
    void Run();
}