using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;
using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Attributes;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Resources.Shaders;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Graphics.Types;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

[Backend(BackendType.OpenGl)]
public sealed partial class OpenGlBackend : IBackend
{
    private readonly Dictionary<TextureBackendHandle, uint> _textures = new();
    private readonly Dictionary<ShaderBackendHandle, uint> _shaders = new();
    private readonly Dictionary<nint, OpenGlContextState> _contexts = new();
    
    private GL _gl = null!;

    private OpenGlArrayObject _vao = null!;
    private OpenGlBufferObject _vbo = null!;
    private OpenGlBufferObject _ebo = null!;
    
    private nint _context = nint.Zero;
    private bool _contextFeature;

    public void Initialize(in GraphicsDeviceSettings settings)
    {
        // Local setup (no api needed)
        _context = settings.Context ?? nint.Zero;
        _contextFeature = settings.Context is not null;
        
        // Api setup
        _gl = GL.GetApi(settings.GetProcAddress.Invoke);
        
        if (_gl.HasErrors())
            return;
        
        // Enable normal debug
        _gl.DebugMessageCallback(DebugProcCallback, in nint.Zero);
        _gl.Enable(EnableCap.DebugOutput);
        _gl.Enable(EnableCap.DebugOutputSynchronous);
        
        _vao = GenArrayObject($"Main VAO ({_context})");
        _vbo = GenBufferObject(BufferTargetARB.ArrayBuffer);
        _ebo = GenBufferObject(BufferTargetARB.ElementArrayBuffer);
        
        _vao.Bind();
        _vbo.Bind();
        _ebo.Bind();
        
        SetupVertexLayout(Constants.ShaderAttribLocations);
        
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();

        if (_contextFeature)
        {
            _contexts[_context] = new OpenGlContextState
            {
                Vao = _vao,
                Initialized = true,
            };
        }
    }

    public void Terminate()
    {
        _vao.Delete();
        _vbo.Delete();
        _ebo.Delete();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SwitchContext(nint context)
    {
        if (!_contextFeature || _context == context)
            return;
        
        ref var state = ref CollectionsMarshal.GetValueRefOrAddDefault(_contexts, context, out var exists);
        if (!exists || !state.Initialized)
            InitializeContext(ref state, context);

        _context = context;
        _vao = state.Vao;
    }

    private void InitializeContext(ref OpenGlContextState state, nint context)
    {
        var vao = GenArrayObject($"Main VAO ({context})");
        
        vao.Bind();
        
        _vbo.Bind();
        _ebo.Bind();
        
        SetupVertexLayout(Constants.ShaderAttribLocations);

        vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind(); 
        
        state.Vao = vao;
        state.Initialized = true;
        
        _vao.Bind(); 
    }
    
    private void DebugProcCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint messagePtr, nint userParam)
    {
        var message = Marshal.PtrToStringAnsi(messagePtr);
    }
}