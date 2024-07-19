using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Hypercube.Shared.Resources;

public class VirtualContentRoot : IContentRoot
{
    private readonly VirtualFileSystem _vfs;
    
    public VirtualContentRoot()
    {
        _vfs = new VirtualFileSystem();
    }

    public bool TryGetFile(ResourcePath path, [NotNullWhen(true)] out Stream? stream)
    {
        if (path.Path[0] != '/')
            path = path.Path.Insert(0, "/");
        
        if (!_vfs.FileExists(path))
            throw new FileNotFoundException("No such file");
        
        return _vfs.TryGetFile(path, out stream);
    }

    public IEnumerable<ResourcePath> FindFiles(ResourcePath path)
    {
        return _vfs.FindFiles(path);
    }

    private sealed class VirtualFolder : IVirtualItem
    {
        public string Name { get; }
        public ResourcePath Path { get; }
        public HashSet<IVirtualItem> Children = new();

        public VirtualFolder(ResourcePath path, string name)
        {
            Path = path;
            Name = name;
        }
        
        public void AddItem(IVirtualItem item)
        {
            Children.Add(item);
        } 
    }

    private sealed class VirtualFile : IVirtualItem
    {
        public ResourcePath Path { get; private set; }
        public string Filename;
        public string DotExtension;
        public string Content;

        public VirtualFile(string filename, string dotExtension, string? content, ResourcePath path)
        {
            Filename = filename;
            DotExtension = dotExtension;
            Content = content ?? string.Empty;
            Path = path;
            
            if (DotExtension[0] != '.')
                DotExtension = DotExtension.Insert(0, ".");
        }

        public void SetContent(string content)
        {
            Content = content;
        }
    }
    
    private interface IVirtualItem
    {
        ResourcePath Path { get; }
    }

    private sealed class VirtualFileSystem
    {
        private Dictionary<ResourcePath, IVirtualItem> _vfsFiles = new();
        
        public VirtualFileSystem()
        {
            SetupVFS();
        }

        public void CreateFile(VirtualFile file, bool createDirectories = false)
        {
            var path = file.Path;
            
            if (!_vfsFiles.TryGetValue(path.ParentDirectory, out var folder))
            {
                if (!createDirectories)
                    throw new DirectoryNotFoundException("No such directory");
                
                CreateDirectory(path.ParentDirectory, new VirtualFolder(path.ParentDirectory, path.ParentDirectory.Filename));
            }

            var vFolder = folder as VirtualFolder;
            vFolder!.AddItem(file);
            _vfsFiles.Add(file.Path, file);
        }

        public void CreateDirectory(ResourcePath path, VirtualFolder folder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResourcePath> FindFiles(ResourcePath path)
        {
            return _vfsFiles.Where(p => p.Key.ParentDirectory == path && p.Key != path).Select(kvp => kvp.Key).ToList();
        }
        
        // ReSharper disable once InconsistentNaming
        private void SetupVFS()
        {
            // add root folder
            _vfsFiles.Add("/", new VirtualFolder("/", "/"));
            var folder = _vfsFiles["/"] as VirtualFolder;
            var file = new VirtualFile("testFile", ".txt", "hey", "/testFile.txt");
            folder!.AddItem(file);
            _vfsFiles.Add("/testFile.txt", file);
        }

        public bool TryGetFile(ResourcePath path, [NotNullWhen(true)] out Stream? stream)
        {
            stream = null;
            
            var file = GetOneItem(path);
            if (file is null)
                return false;

            if (file is not VirtualFile vFile)
                throw new InvalidOperationException("Tried to get folder stream???");

            stream = GetStringMemStream(vFile.Content);
            return true;
        }

        public bool FileExists(ResourcePath path)
        {
            return GetOneItem(path) is not null;
        }

        private IVirtualItem? GetOneItem(ResourcePath path)
        {
            var found = _vfsFiles.Where(p => p.Key == path.Path).ToList();

            return found.Count switch
            {
                1 => found.First().Value,
                > 1 => throw new InvalidOperationException("There was a problem creating VFS"),
                _ => null
            };
        }

        private Stream GetStringMemStream(string content)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            return new MemoryStream(bytes);
        }
    }
}