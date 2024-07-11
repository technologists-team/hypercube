using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Shared.Resources.Manager;

public interface IResourceManager
{
    StreamReader WrapStream(Stream stream);
    void AddRoot(ResourcePath prefix, IContentRoot root);
    void MountContentFolder(string file, ResourcePath? prefix = null);
    Stream? ReadFileContent(ResourcePath path);
    bool TryReadFileContent(ResourcePath path, [NotNullWhen(true)] out Stream? fileStream);
    IEnumerable<ResourcePath> FindContentFiles(ResourcePath? path);
    string ReadFileContentAllText(ResourcePath path);
}