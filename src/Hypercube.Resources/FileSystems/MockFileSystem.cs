namespace Hypercube.Resources.FileSystems;

public sealed class MockFileSystem : IFileSystem
{
    private readonly Dictionary<ResourcePath, byte[]> _files = new();

    public void AddFile(ResourcePath path, byte[] content)
    {
        _files[path.Normalized] = content;
    }

    public bool Exists(ResourcePath path)
    {
        return _files.ContainsKey(path.Normalized);
    }

    public Stream OpenRead(ResourcePath path)
    {
        var normalized = path.Normalized;
        if (_files.TryGetValue(normalized, out var data))
            return new MemoryStream(data);
        
        throw new FileNotFoundException(normalized);
    }

    public List<ResourcePath> GetFiles(ResourcePath path)
    {
        return [];
    }
}