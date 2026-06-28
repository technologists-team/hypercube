using System.Runtime.InteropServices;
using Hypercube.Graphics.Backend;
using Hypercube.Graphics.Backend.Commands;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Core.Device;

public unsafe class GraphicDeviceResources
{
    private readonly Dictionary<TextureId, Texture> _textures = new();
    private readonly List<nint> _allocations = new();
    
    private readonly GraphicDevice _device;

    public GraphicDeviceResources(GraphicDevice device)
    {
        _device = device;
    }
    
    private int NextTextureId => field++;
    private int NextTextureHandle => field++;

    public TextureId CreateTexture(Stream stream, TextureCreationSettings settings)
    {
        var id = new TextureId(NextTextureId);
        var handle = new TextureBackendHandle(NextTextureHandle);
        
        var texture = new Texture
        {
            Id = id,
            Handle = handle,
            Metadata = new TextureMetadata
            {
                Size = settings.Size,
                Depth = settings.Depth,
                Type = settings.Type,
                Format = settings.Format,
                MipmapLevels = -1
            }
        };

        _textures[id] = texture;

        _device.RaiseCommand(new LowCommandCreateTexture
        {
            Handle = handle,
            Data = Allocate(stream, out var size),
            DataSize = size,
        }, LowCommandType.CreateTexture);

        return id;
    }

    public bool TryGetTextureMetadata(TextureId id, out TextureMetadata metadata)
    {
        metadata = default;
        
        if (!_textures.TryGetValue(id, out var texture))
            return false;
        
        metadata = texture.Metadata;
        return true;
    }

    public TextureMetadata GetTextureMetadata(TextureId id)
    {
        return _textures[id].Metadata;
    }

    public void FreeAllocations()
    {
        foreach (var allocation in _allocations)
            NativeMemory.Free((void*) allocation);
        
        _allocations.Clear();
    }

    private void* Allocate(Stream stream, out int size)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Allocate(memoryStream, out size);
    }

    private void* Allocate(MemoryStream stream, out int size)
    {
        return Allocate(stream.TryGetBuffer(out var buffer)
            ? buffer.AsSpan()
            : stream.ToArray(),
        out size);
    }

    private void* Allocate(ReadOnlySpan<byte> data, out int size)
    {
        size = data.Length;
        
        var pointer = NativeMemory.Alloc((nuint) size);
        fixed (byte* src = data)
        {
            Buffer.MemoryCopy(src, pointer, size, size);
        }
        
        _allocations.Add((nint) pointer);
        return pointer;
    }

    public static int CalculateMipmapCount(Vector2i size)
    {
        return CalculateMipLevels(size.X, size.Y);
    }
    
    public static int CalculateMipLevels(int width, int height)
    {
        var levels = 1;
        while (width > 1 || height > 1)
        {
            width = int.Max(1, width / 2);
            height = int.Max(1, height / 2);
            levels++;
        }
        return levels;
    }
}