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
    
    public GL Gl { get; private set; } = null!;

    private OpenGlArrayObject _vao = null!;
    private OpenGlBufferObject _vbo = null!;
    private OpenGlBufferObject _ebo = null!;

    public void Initialize(in GraphicsDeviceSettings settings)
    {
        // TODO: context
        // Local setup (no api needed)
        // _context = settings.Context ?? nint.Zero;
        // _contextFeature = settings.Context is not null;
        
        // Api setup
        Gl = GL.GetApi(settings.GetProcAddress.Invoke);
        
        if (Gl.HasErrors())
            return;
        
        // Enable normal debug
        Gl.DebugMessageCallback(DebugProcCallback, in nint.Zero);
        Gl.Enable(EnableCap.DebugOutput);
        Gl.Enable(EnableCap.DebugOutputSynchronous);
        
        _vao = GenArrayObject($"Main VAO ({settings.Context ?? nint.Zero})");
        _vbo = GenBufferObject(BufferTargetARB.ArrayBuffer);
        _ebo = GenBufferObject(BufferTargetARB.ElementArrayBuffer);
        
        _vao.Bind();
        _vbo.Bind();
        _ebo.Bind();
        
        SetupVertexLayout(Constants.ShaderAttribLocations);
        
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
    }

    public void Terminate()
    {
        _vao.Delete();
        _vbo.Delete();
        _ebo.Delete();
    }

    public IBackendFence CreateFence() => new OpenGlBackendFence(this);

    private void DebugProcCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint messagePtr, nint userParam)
    {
        var message = Marshal.PtrToStringAnsi(messagePtr);
    }
}
