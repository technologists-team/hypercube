namespace Hypercube.Resources.FileSystems;

public interface IFileSystem
{
    void Mount(Dictionary<ResourcePath, ResourcePath> mountFolders);
    void Mount(Dictionary<string, string> mountFolders);
    void Mount(ResourcePath physicalPath, ResourcePath relativePath);
    void Unmount(ResourcePath relativePath);
    bool Exists(ResourcePath path);
    FileStream OpenRead(ResourcePath relativePath);
    List<ResourcePath> GetFiles(ResourcePath path);
}
