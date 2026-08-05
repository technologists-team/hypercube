namespace Hypercube.Graphics;

public readonly struct Version
{
    public uint Major { get; init; }
    public uint Minor { get; init; }
    public uint Patch { get; init; }

    public Version(uint major, uint minor, uint patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }
    
    public Version(int major, int minor, int patch)
    {
        Major = (uint) major;
        Minor = (uint) minor;
        Patch = (uint) patch;
    }
}