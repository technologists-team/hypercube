namespace Hypercube.Windowing.Types;

public readonly struct WindowContextVersion(int major, int minor)
{
    public int Major { get; init; } = major;
    public int Minor { get; init; } = minor;
}
