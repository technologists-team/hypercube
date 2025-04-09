namespace Hypercube.Resources.FileSystems;

public sealed class PhysicalFileSystem : IFileSystem
{
    private readonly Dictionary<ResourcePath, ResourcePath> _mounting = new();
    private readonly object _mountLock = new();
    
    public void Mount(Dictionary<ResourcePath, ResourcePath> mountFolders)
    {
        throw new NotImplementedException();
    }

    public void Mount(Dictionary<string, string> mountFolders)
    {
        throw new NotImplementedException();
    }

    public void Mount(ResourcePath path, ResourcePath physicalPath)
    {
        throw new NotImplementedException();
    }

    public void Unmount(ResourcePath path)
    {
        throw new NotImplementedException();
    }

    public bool Exists(ResourcePath path)
    {
        return File.Exists(path);
    }

    public Stream OpenRead(ResourcePath path)
    {
        return File.OpenRead(path);
    }

    public List<ResourcePath> GetFiles(ResourcePath path)
    {
        return Directory.GetFiles(path).Select(str => new ResourcePath(str)).ToList();
    }
}
