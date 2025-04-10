using System.Diagnostics.CodeAnalysis;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Resources.FileSystems;

public sealed class PhysicalFileSystem : IFileSystem
{
    private readonly List<Root> _mountings = [];
    private readonly object _mountLock = new();

    public void Mount(Dictionary<ResourcePath, ResourcePath> mountFolders)
    {
        lock (_mountLock)
        {
            foreach (var (physicalPath, relativePath) in mountFolders)
            {
                Mount(physicalPath, relativePath);
            }
        }
    }

    public void Mount(Dictionary<string, string> mountFolders)
    {
        lock (_mountLock)
        {
            foreach (var (physicalPath, relativePath) in mountFolders)
            {
                Mount(physicalPath, relativePath);
            }
        }
    }

    public void Mount(ResourcePath physicalPath, ResourcePath relativePath)
    {
        if (!physicalPath.Rooted)
            physicalPath = PathHelper.GetExecutionRelativeFile(physicalPath);

        if (!relativePath.Rooted)
            throw new ArgumentException($"Relative path \"{relativePath}\" must be rooted", nameof(relativePath));

        lock (_mountLock)
        {
            _mountings.Add(new Root(relativePath, physicalPath));
        }
    }

    public void Unmount(ResourcePath relativePath)
    {
        lock (_mountLock)
        {
            _mountings.RemoveAll(root => root.RelativePath.Equals(relativePath));
        }
    }

    public bool Exists(ResourcePath path)
    {
        if (!path.Rooted)
            return false;

        // Check mounted paths first
        lock (_mountLock)
        {
            foreach (var root in _mountings)
            {
                if (path.TryRelativeTo(root.RelativePath, out var relativePath))
                    return root.Exists(relativePath.Value);
            }
        }

        // Fall back to direct file system check
        return File.Exists(path);
    }

    public FileStream OpenRead(ResourcePath path)
    {
        if (!path.Rooted)
            throw new ArgumentException($"Path must be rooted: {path}");

        if (path.Value.EndsWith(ResourcePath.Separator))
            throw new ArgumentException("Cannot open a directory as a file", nameof(path));

        lock (_mountLock)
        {
            foreach (var root in _mountings)
            {
                if (!path.TryRelativeTo(root.RelativePath, out var relativePath))
                    continue;
                
                if (root.TryGet(relativePath.Value, out var stream))
                    return stream;
            }
        }

        // Fall back to direct file system access
        return File.OpenRead(path);
    }

    public List<ResourcePath> GetFiles(ResourcePath path)
    {
        if (!path.Rooted)
            throw new ArgumentException($"Path \"{path}\" must be rooted", nameof(path));
        
        lock (_mountLock)
        {
            foreach (var root in _mountings)
            {
                if (path != root.RelativePath && !path.IsChildOf(root.RelativePath))
                    continue;
                
                if (path.TryRelativeTo(root.RelativePath, out var relativePath))
                    return root.GetFiles(relativePath.Value)
                        .Select(p => root.RelativePath + p)
                        .ToList();
            }
        }

        // Fall back to direct file system access
        return Directory.GetFiles(path)
            .Select(str => new ResourcePath(str))
            .ToList();
    }
    
    private class Root
    {
        private readonly DirectoryInfo _directory;

        public ResourcePath PhysicalPath { get; }
        public ResourcePath RelativePath { get; }

        public Root(ResourcePath relativePath, ResourcePath physicalPath)
        {
            RelativePath = relativePath;
            PhysicalPath = physicalPath;
            _directory = new DirectoryInfo(physicalPath);
            
            if (!_directory.Exists)
                throw new DirectoryNotFoundException($"Physical path does not exist: {physicalPath}");
        }
        
        public bool Exists(ResourcePath relativePath)
        {
            return File.Exists(GetPhysicalPath(relativePath));
        }

        public FileStream Get(ResourcePath relativePath)
        {
            return File.Open(GetPhysicalPath(relativePath), FileMode.Open, FileAccess.Read);
        }
        
        public bool TryGet(ResourcePath relativePath, [NotNullWhen(true)] out FileStream? stream)
        {
            stream = null;
            
            if (!Exists(relativePath))
                return false;
        
            try
            {
                stream = File.Open(GetPhysicalPath(relativePath), FileMode.Open, FileAccess.Read);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ResourcePath> GetFiles(ResourcePath relativePath)
        {
            var fullPath = GetPhysicalPath(relativePath);
            if (!Directory.Exists(fullPath))
                return [];

            return Directory.GetFiles(fullPath)
                .Select(file => new ResourcePath(Path.GetRelativePath(_directory.FullName, file)))
                .ToList();
        }

        private string GetPhysicalPath(ResourcePath path)
        {
            return Path.GetFullPath(Path.Combine(_directory.FullName, path));
        }
    }
}