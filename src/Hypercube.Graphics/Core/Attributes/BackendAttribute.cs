using Hypercube.Graphics.Types;

namespace Hypercube.Graphics.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BackendAttribute : Attribute
{
    public readonly BackendType Backend;

    public BackendAttribute(BackendType backend)
    {
        Backend = backend;
    }
}
