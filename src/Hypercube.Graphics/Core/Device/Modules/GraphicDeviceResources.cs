using System.Text;
using Hypercube.Graphics.Backend;
using Hypercube.Graphics.Backend.Commands;
using Hypercube.Graphics.Core.Resources;
using Hypercube.Graphics.Resources.Shaders;
using Hypercube.Graphics.Resources.Shaders.Data;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Graphics.Resources.Textures.Data;

namespace Hypercube.Graphics.Core.Device.Modules;

// Yes, it is heavily overloaded with repetitive code,
// and yes, it is not the best solution;
// this module will need to be updated in the future.
public unsafe class GraphicDeviceResources
{
    private readonly Dictionary<TextureId, Texture> _textures = new();
    private readonly Dictionary<ShaderId, Shader> _shaders = new();
    
    private readonly GraphicDevice _device;
    private readonly ResourceAllocator _allocator = new();
    
    private TextureId NextTextureId => field++;
    private TextureBackendHandle NextTextureHandle => field++;
    
    private ShaderId NextShaderId => field++;
    private ShaderBackendHandle NextShaderHandle => field++;
    
    public GraphicDeviceResources(GraphicDevice device)
    {
        _device = device;
    }

    #region Textures

    public TextureId CreateTexture(Stream stream, TextureCreationSettings settings)
    {
        var id = NextTextureId;
        var handle = NextTextureHandle;
        
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
                MipmapLevels = -1 // TODO
            }
        };

        _textures[id] = texture;

        _device.Backend.CommandPush(new LowCommandCreateTexture
        {
            Handle = handle,
            Type = settings.Type,
            Format = settings.Format,
            Data = _allocator.Allocate(stream, out var size),
            DataSize = size,
            Size = settings.Size
        }, LowCommandType.CreateTexture);

        return id;
    }

    public TextureBackendHandle TranslateTexture(TextureId id)
    {
        return _textures[id].Handle;
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

    #endregion
    
    #region Shaders

    public void BindShader(ShaderId id)
    {
        _device.Backend.CommandPush(new LowCommandBindShader
        {
            Handle = _shaders[id].Handle,
        }, LowCommandType.BindShader);
    }
    
    public ShaderId CreateShader(params ShaderPart[] parts)
    {
        var id = NextShaderId;
        var handle = NextShaderHandle;
        
        var totalSize = sizeof(byte);
        for (var i = 0; i < parts.Length; i++)
        {
            totalSize += sizeof(byte);
            totalSize += sizeof(int);
            totalSize += Encoding.UTF8.GetByteCount(parts[i].Code);
        }

        var data = (byte*) _allocator.Allocate(totalSize);
        var pointer = data;
        
        *pointer++ = (byte) parts.Length;
        
        for (var i = parts.Length - 1; i >= 0; i--)
        {
            var part = parts[i];

            *pointer++ = (byte) part.Type;

            var byteCount = Encoding.UTF8.GetByteCount(part.Code);

            *(int*) pointer = byteCount;
            pointer += sizeof(int);

            fixed (char* src = part.Code)
                Encoding.UTF8.GetBytes(src, part.Code.Length, pointer, byteCount);

            pointer += byteCount;
        }

        var shader = new Shader
        {
            Id = id,
            Handle = handle,
            Metadata = new ShaderMetadata(),
        };

        _shaders[id] = shader;

        _device.Backend.CommandPush(new LowCommandCreateShader
        {
            Handle = handle,
            Data = data,
            DataSize = totalSize,
        }, LowCommandType.CreateShader);

        return id;
    }
    
    public bool TryGetShaderMetadata(ShaderId id, out ShaderMetadata metadata)
    {
        metadata = default;
        
        if (!_shaders.TryGetValue(id, out var shader))
            return false;
        
        metadata = shader.Metadata;
        return true;
    }

    public ShaderMetadata GetShaderMetadata(ShaderId id)
    {
        return _shaders[id].Metadata;
    }
    
    #endregion
    
    public void FreeAllocations()
    {
        _allocator.FreeAll();
    }
}