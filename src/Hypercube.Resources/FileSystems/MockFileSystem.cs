namespace Hypercube.Resources.FileSystems;

public sealed class MockFileSystem : IFileSystem
{
    private readonly Dictionary<ResourcePath, byte[]> _files = new();
    private readonly Dictionary<ResourcePath, string> _textFiles = new();

    public void AddFile(ResourcePath path, byte[] content)
    {
        _files[path.Normalized] = content;
    }

    public void AddTextFile(ResourcePath path, string content)
    {
        _textFiles[path.Normalized] = content;
    }

    public bool Exists(ResourcePath path)
    {
        var normalized = path.Normalized;
        return _files.ContainsKey(normalized) || _textFiles.ContainsKey(normalized);
    }

    public Stream OpenRead(ResourcePath path)
    {
        var normalized = path.Normalized;
        if (_files.TryGetValue(normalized, out var data))
            return new MemoryStream(data);
        
        throw new FileNotFoundException(normalized);
    }
}