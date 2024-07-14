using System.Diagnostics.CodeAnalysis;

namespace Hypercube.Shared.Resources;

public interface IContentRoot
{
    bool TryGetFile(ResourcePath path, [NotNullWhen(true)] out Stream? stream);
    IEnumerable<ResourcePath> FindFiles(ResourcePath path);
    Stream? CreateFile(ResourcePath path);
}