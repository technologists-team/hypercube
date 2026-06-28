using JetBrains.Annotations;

namespace Hypercube.Graphics.Core.Types;

[PublicAPI]
public enum FailStrategy
{
    Ignore,
    Log,
    Crash
}
