using JetBrains.Annotations;

namespace Hypercube.Graphics;

[PublicAPI]
public enum FailStrategy
{
    Log,
    Ignore,
    Crash,
}
