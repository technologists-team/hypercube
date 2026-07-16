namespace Hypercube.Graphics.Core.Batching;

[Flags]
public enum BatchFlags
{
    None           = 1 << 0,
    IgnoreLayering = 1 << 1 
}
