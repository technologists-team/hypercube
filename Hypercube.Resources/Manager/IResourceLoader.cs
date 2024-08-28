using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Resources.Manager;

/// <summary>
/// The main mechanism serving for work and management with game resource loading streams.
/// </summary>
public interface IResourceLoader
{
    StreamReader WrapStream(Stream stream);
    void AddRoot(ResourcePath prefix, IContentRoot root);
    void MountContentFolder(string file, ResourcePath? prefix = null);
    Stream? ReadFileContent(ResourcePath path);
    bool TryReadFileContent(ResourcePath path, [NotNullWhen(true)] out Stream? fileStream);
    IEnumerable<ResourcePath> FindContentFiles(ResourcePath? path);
    string ReadFileContentAllText(ResourcePath path);
}