using System.Diagnostics.CodeAnalysis;
using Hypercube.Resources.Root;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;
using Hypercube.Utilities.Helpers;
using JetBrains.Annotations;

namespace Hypercube.Resources.Loader;

[PublicAPI]
public class ResourceLoader : IResourceLoader
{
    [Dependency] private readonly ILogger _logger = default!;

    private readonly Dictionary<ResourcePath, string> _cachedContent = new();
    private readonly object _rootLock = new();
    
    private (ResourcePath prefix, IContentRoot root)[] _roots = [];
    
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
            file = PathHelper.GetExecutionRelativeFile(file);

        var dirInfo = new DirectoryInfo(file);
        
        if (!dirInfo.Exists)
            throw new DirectoryNotFoundException($"Willing directory is not found. {dirInfo.FullName}");
        
        var dirRoot = new DirContentRoot(dirInfo, _logger);
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

                if (!root.TryGetFile(relative.Value, out var stream)) continue;
                fileStream = stream;
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.Critical(ex.Message);
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
        if (_cachedContent.TryGetValue(path, out var result))
            return result;
        
        using var stream = ReadFileContent(path);
        if (stream is null)
            throw new ArgumentException($"File not found: {path.Path}");

        using var warped = WrapStream(stream);
        
        var content = warped.ReadToEnd();
        _cachedContent[path] = content;

        return content;
    }

    public bool TryReadFileContentAll(ResourcePath path, [NotNullWhen(true)] out string? content)
    {
        content = null;
        
        if (!path.Rooted)
            throw new ArgumentException($"Path must be rooted: {path}");

        if (path.Path.EndsWith(ResourcePath.Separator))
            return false;

        try
        {
            foreach (var (prefix, root) in _roots)
            {
                if (!path.TryRelativeTo(prefix, out var relative))
                    continue;

                if (!root.TryGetFile(relative.Value, out var stream))
                    continue;

                using var warped = WrapStream(stream);
                content = warped.ReadToEnd();
                _cachedContent[path] = content;
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.Critical(ex.Message);
        }
        
        return false;
    }

    public Stream? ReadFileContent(ResourcePath path)
    {
        return !TryReadFileContent(path, out var stream) ? null : stream;
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