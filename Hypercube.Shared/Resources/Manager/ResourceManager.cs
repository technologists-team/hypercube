using System.Diagnostics.CodeAnalysis;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources.DirRoot;
using Hypercube.Shared.Utilities.Helpers;

namespace Hypercube.Shared.Resources.Manager;

public sealed class ResourceManager : IResourceManager
{
    private readonly Logger _logger = LoggingManager.GetLogger("resources");
    private (ResourcePath prefix, IContentRoot root)[] _roots = Array.Empty<(ResourcePath, IContentRoot)>();
    private readonly object _rootLock = new();
    
    public void AddRoot(ResourcePath prefix, IContentRoot root)
    {
        lock (_rootLock)
        {
            var copy = _roots;
            Array.Resize(ref copy, copy.Length + 1);
            copy[^1] = (prefix, root);
            _roots = copy;
        }
    }

    public void MountContentFolder(string file, ResourcePath? prefix = null)
    {
        prefix = ValidatePrefix(prefix);
        
        if (!Path.IsPathRooted(file))
            file = PathHelpers.GetExecRelativeFile(file);

        var dirInfo = new DirectoryInfo(file);
        
        if (!dirInfo.Exists)
            throw new DirectoryNotFoundException($"Willing directory is not found. {dirInfo.FullName}");
        
        var dirRoot = new DirContentRoot(dirInfo, LoggingManager.GetLogger("dirRoot"));
        AddRoot(prefix.Value, dirRoot);
    }
    
    public bool TryReadFileContent(ResourcePath path, [NotNullWhen(true)] out Stream? fileStream)
    {
        if (!path.Rooted)
            throw new ArgumentException($"Path must be rooted: {path}");

        if (path.Path.EndsWith(ResourcePath.Separator))
        {
            fileStream = null;
            return false;
        }

        try
        {
            foreach (var (prefix, root) in _roots)
            {
                if (!path.TryRelativeTo(prefix, out var relative))
                    continue;

                if (root.TryGetFile(relative.Value, out var stream))
                {
                    fileStream = stream;
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }

        fileStream = null;
        return false;
    }

    public IEnumerable<ResourcePath> FindContentFiles(ResourcePath? path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));
        
        if (!path.Value.Rooted)
            throw new ArgumentException("Path is not rooted", nameof(path));
        
        
        var returned = new HashSet<ResourcePath>();
        
        foreach (var (prefix, root) in _roots)
        {
            if (!path.Value.TryRelativeTo(prefix, out var relative))
                continue;
            
            foreach (var filename in root.FindFiles(relative.Value))
            {
                var newPath = prefix + filename;
                
                if (returned.Add(newPath))
                    yield return newPath;
            }
        }
    }

    public string ReadFileContentAllText(ResourcePath path)
    {
        var stream = ReadFileContent(path);
        if (stream is null)
            throw new ArgumentException($"File not found: {path.Path}");

        return WrapStream(stream).ReadToEnd();
    }

    public Stream? ReadFileContent(ResourcePath path)
    {
        if (!TryReadFileContent(path, out var stream))
            return null;

        return stream;
    }

    private ResourcePath ValidatePrefix(ResourcePath? prefix)
    {
        if (prefix.HasValue && !prefix.Value.Rooted)
            throw new ArgumentException("Prefix must be rooted", nameof(prefix));

        return prefix ?? "/";
    }

    public StreamReader WrapStream(Stream stream)
    {
        return new StreamReader(stream);
    }
}