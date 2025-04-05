namespace Hypercube.Resources.FileSystems;

public sealed class PhysicalFileSystem : IFileSystem
{
    public bool Exists(ResourcePath path)
    {
        return File.Exists(path);
    }

    public Stream OpenRead(ResourcePath path)
    {
        return File.OpenRead(path);
    }
}
