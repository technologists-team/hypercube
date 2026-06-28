using Hypercube.Graphics.Backend.Realisation.OpenGl.Objects;
using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Types;
using Hypercube.Graphics.Device;
using Hypercube.Mathematics.Matrices;

using Silk.NET.OpenGL;
using SilkPrimitiveType = Silk.NET.OpenGL.PrimitiveType;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend : IBackend
{
    private readonly Vertex[] _batchVertices = new Vertex[2 << 14];
    private readonly uint[] _batchIndices = new uint[2 << 14];
    
    private GL _gl = null!;
    
    private OpenGlArrayObject _vao = null!;
    private OpenGlBufferObject _vbo = null!;
    private OpenGlBufferObject _ebo = null!;

    private int _batchVerticesIndex;
    private int _batchIndicesIndex;
    
    private Matrix4x4 _projection = Matrix4x4.Identity;
    private Matrix4x4 _view = Matrix4x4.Identity;
    
    private SilkPrimitiveType _primitive = SilkPrimitiveType.Quads;
    
    public void Initialize(in GraphicsDeviceSettings settings)
    {
        _gl = GL.GetApi(settings.GetProcAddress.Invoke);
        
        if (_gl.HasErrors())
            return;
        
        // Enable normal debug
        _gl.DebugMessageCallback(DebugProcCallback, in nint.Zero);
        _gl.Enable(EnableCap.DebugOutput);
        _gl.Enable(EnableCap.DebugOutputSynchronous);
        
        _vao = GenArrayObject("Main VAO");
        _vbo = GenBufferObject(BufferTargetARB.ArrayBuffer, "Main VBO");
        _ebo = GenBufferObject(BufferTargetARB.ElementArrayBuffer, "Main EBO");
        
        _vao.Bind();
        _vbo.SetData(_batchVertices);
        _ebo.SetData(_batchIndices);

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

    public void FrameStart()
    {
        Clear();
    }

    public void EndFrame()
    {
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
    }

    private void Draw()
    {
        // _gl.DrawElements(_primitive);
    }
    
    private void Clear()
    {
        _batchVerticesIndex = 0;
        _batchIndicesIndex = 0;
    }
    
    private void DebugProcCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
    {
        
    }
}