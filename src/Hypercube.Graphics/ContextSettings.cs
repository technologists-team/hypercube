using Hypercube.Graphics.Core.Backends;
using JetBrains.Annotations;

namespace Hypercube.Graphics;

[PublicAPI]
public readonly struct ContextSettings
{
    public static readonly ContextSettings Default = new()
    {
        Backend = RenderBackend.Auto,
        FailStrategy = FailStrategy.Crash
    };
    
    public RenderBackend Backend { get; init; }
    public FailStrategy FailStrategy { get; init; }
}
