namespace Hypercube.Resources.FileSystems;

public interface IFileSystem
{
    bool Exists(ResourcePath path);
    Stream OpenRead(ResourcePath path);
}
