using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Backends;
using Hypercube.Graphics.Core.Context;
using Hypercube.Utilities;

namespace Hypercube.Graphics;

public sealed class GraphicContext
{
    public static readonly IContextInfoProvider NullInfo = new NullInfoProvider();
    
    public event Action<string>? OnFail;
    public event Action<string>? OnWarn;

    private readonly RenderBackend _backend;
    private readonly FailStrategy _failStrategy;
    
    private GraphicContext(ContextSettings settings)
    {
        _backend = ResolveBackend(settings.Backend);
        _failStrategy = settings.FailStrategy;
    }

    public GraphicDevice CreateDevice(IContextInfoProvider provider)
    {
        var device = new GraphicDevice();
        return device;
    }
    
    public static GraphicContext Create()
    {
        return new GraphicContext(ContextSettings.Default);
    }
    
    public static GraphicContext Create(RenderBackend backend)
    {
        return new GraphicContext(ContextSettings.Default with { Backend = backend });
    }
    
    public static GraphicContext Create(ContextSettings settings)
    {
        return new GraphicContext(settings);
    }
    
    private RenderBackend ResolveBackend(RenderBackend backend = RenderBackend.Auto, bool forced = false)
    {
        var os = HyperOS.Current;
        
        // Something terrible going on
        if (!Constants.OSBackendMatrix.TryGetValue(os, out var info))
        {
            WarnResolve($"Unsupported OS type: {os}");
            return RenderBackend.None;
        }
        
        if (backend != RenderBackend.Auto)
        {
            if (info.Supported.Contains(backend))
            {
                if (TryChoose(backend))
                    return backend;
            }
            
            if (forced)
            {
                FailResolve($"Forced unsupported os {os} backend {backend}");
                return RenderBackend.None;
            }
            
            WarnResolve($"Unsupported os {os} backend {backend}, no force, switch to auto");
        }
        
        if (TryChoose(info.Best))
            return info.Best;
            
        foreach (var supportedBackend in info.Supported)
        {
            if (TryChoose(supportedBackend))
                return supportedBackend;
        }

        FailResolve($"Os {os} doesn't support any backends");
        return RenderBackend.None;
        
        bool TryChoose(RenderBackend back) => Constants.ImplementedBackends.Contains(back);
    }
    
    public void WarnResolve(string message)
    {
    }

    public void FailResolve(string message)
    {
        switch (_failStrategy)
        {
            case FailStrategy.Ignore:
                // Just ignore
                break;
            
            case FailStrategy.Log:
                OnFail?.Invoke(message);
                break;
            
            case FailStrategy.Crash:
                throw new Exception(message);
                break;
        }
    }

    private class NullInfoProvider : IContextInfoProvider
    {
        public nint GetProcAddress(string name) => nint.Zero;
    }
}