using System.Collections.Frozen;
using System.Diagnostics;
using System.Text;
using Hypercube.Client.Graphics.Texturing.Events;
using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using Hypercube.Shared.Runtimes.Event;
using StbImageSharp;

namespace Hypercube.Client.Graphics.Texturing;

public sealed class TextureManager : ITextureManager, IEventSubscriber, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private FrozenDictionary<ResourcePath, ITextureHandle> _cachedHandles = FrozenDictionary<ResourcePath, ITextureHandle>.Empty;
    private FrozenDictionary<ResourcePath, ITexture> _cachedTextures = FrozenDictionary<ResourcePath, ITexture>.Empty;
    
    private readonly Logger _logger = LoggingManager.GetLogger("texturing");

    public TextureManager()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeInitializationEvent>(this, OnInitialization);
    }

    private void OnInitialization(ref RuntimeInitializationEvent args)
    {
        _logger.EngineInfo("Caching textures...");
        var st = Stopwatch.StartNew();
        var ev = new TexturesPreloadEvent(new HashSet<ResourcePath>());
        var textures = new Dictionary<ResourcePath, ITexture>();
        _eventBus.Raise(ref ev);
        
        foreach (var texturePath in ev.Textures)
        {
            if (textures.ContainsKey(texturePath))
                continue;
            
            var texture = CreateTexture(texturePath);
            textures.Add(texturePath, texture);
        }
        
        CacheTextures(textures);
        st.Stop();
        _logger.EngineInfo($"Cached {_cachedTextures.Count} textures in {st.Elapsed}");
    }

    public void CacheHandles()
    {
        _logger.EngineInfo("Caching handles...");
        var st = Stopwatch.StartNew();
        
        var ev = new HandlesPreloadEvent(new HashSet<ResourcePath>());
        _eventBus.Raise(ref ev);
        
        var handles = new Dictionary<ResourcePath, ITextureHandle>();
        
        foreach (var handlePath in ev.Handles)
        {
            if (handles.ContainsKey(handlePath))
                continue;
            
            ITextureHandle handle;
            
            if (_cachedTextures.TryGetValue(handlePath, out var texture))
            {
                handle = CreateTextureHandle(texture, new Texture2DCreationSettings());
                handles.Add(handlePath, handle);
                continue;
            }
            
            handle = CreateTextureHandle(handlePath, new Texture2DCreationSettings());
            handles.Add(handlePath, handle);
        }

        CacheHandles(handles);
        st.Stop();
        
        _logger.EngineInfo($"Cached {_cachedHandles.Count} handles in {st.Elapsed}");
    }

    #region PublicAPI
    
    public ITextureHandle GetTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        return GetTextureHandleInternal(path, settings);
    }
    
    public ITextureHandle GetTextureHandle(ResourcePath path)
    {
        return GetTextureHandleInternal(path, new Texture2DCreationSettings());
    }
    
    public ITexture GetTexture(ResourcePath path)
    {
        return GetTextureInternal(path);
    }

    public ITextureHandle GetTextureHandle(ITexture texture)
    {
        return GetTextureHandleInternal(texture.Path, new Texture2DCreationSettings());
    }

    public ITextureHandle GetTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return GetTextureHandleInternal(texture.Path, settings);
    }

    #endregion

    #region Internal

    internal ITexture GetTextureInternal(ResourcePath path)
    {
        if (_cachedTextures.TryGetValue(path, out var value))
            return value;
        
        // fallback to low performance method
        var texture = CreateTexture(path);
        CacheTexture(texture);
        
        return texture;
    }

    internal ITexture CreateTexture(ResourcePath path)
    {
        using var stream = _resourceManager.ReadFileContent(path);
        
        var result = ImageResult.FromStream(stream);
        var texture = new Texture(path, (result.Width, result.Height), result.Data);
        
        return texture;
    }

    internal ITextureHandle GetTextureHandleInternal(ResourcePath path, ITextureCreationSettings settings)
    {
        if (_cachedHandles.TryGetValue(path, out var value))
            return value;

        ITextureHandle handle;
        
        if (_cachedTextures.TryGetValue(path, out var texture))
        {
            // low performance method fallback
            handle = CreateTextureHandle(texture, settings);
            CacheHandle(handle);    
            
            return handle;
        }

        // fallback to low performance method
        handle = CreateTextureHandle(path, settings);
        
        CacheHandle(handle);

        return handle;
    }

    internal ITextureHandle CreateTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        var texture = GetTexture(path);
        var handle = new TextureHandle(texture, settings);

        return handle;
    }
    
    internal ITextureHandle CreateTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return new TextureHandle(texture, settings);
    }

    /// <summary>
    /// Extremely high impact on performance, use when you know what you're doing
    /// </summary>
    internal void CacheTexture(ITexture texture)
    {
        var cached = _cachedTextures.ToDictionary();
        cached.Add(texture.Path, texture);
        _cachedTextures = cached.ToFrozenDictionary();
        _logger.Warning($"Cached texture with path {texture.Path} in runtime");
    }
    
    /// <summary>
    /// Extremely high impact on performance, use when you know what you're doing
    /// </summary>
    internal void CacheHandle(ITextureHandle texture)
    {
        var cached = _cachedHandles.ToDictionary();
        cached.Add(texture.Texture.Path, texture);
        _cachedHandles = cached.ToFrozenDictionary();
        _logger.Warning($"Cached handle with path {texture.Texture.Path} in runtime");
    }
    
    #endregion

    #region Preloading

    internal void CacheTextures(Dictionary<ResourcePath, ITexture> textures)
    {
        _cachedTextures = textures.ToFrozenDictionary();
    }
    
    internal void CacheHandles(Dictionary<ResourcePath, ITextureHandle> handles)
    {
        _cachedHandles = handles.ToFrozenDictionary();
    }

    #endregion

}