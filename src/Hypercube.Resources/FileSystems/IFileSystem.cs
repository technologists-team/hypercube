namespace Hypercube.Resources.FileSystems;

public interface IFileSystem
{
    void Mount(Dictionary<ResourcePath, ResourcePath> mountFolders);
    void Mount(Dictionary<string, string> mountFolders);
    void Mount(ResourcePath path, ResourcePath physicalPath);
    void Unmount(ResourcePath path);
    bool Exists(ResourcePath path);
    Stream OpenRead(ResourcePath path);
    List<ResourcePath> GetFiles(ResourcePath path);
}
