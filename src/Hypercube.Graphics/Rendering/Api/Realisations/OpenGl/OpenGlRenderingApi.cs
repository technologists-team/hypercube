using System.Runtime.InteropServices;
using System.Text;
using Hypercube.Core.Analyzers;
using Hypercube.Graphics.Rendering.Api.Handlers;
using Hypercube.Graphics.Rendering.Batching;
using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Graphics.Utilities.Extensions;
using Hypercube.Graphics.Viewports;
using Hypercube.Graphics.Windowing;
using Hypercube.Resources;
using Hypercube.Utilities.Dependencies;
using Silk.NET.OpenGL;
using ShaderType = Hypercube.Graphics.Rendering.Shaders.ShaderType;

namespace Hypercube.Graphics.Rendering.Api.Realisations.OpenGl;

[EngineInternal]
public sealed partial class OpenGlRenderingApi : BaseRenderingApi
{
    [Dependency] private readonly ICameraManager _cameraManager = default!;
    [Dependency] private readonly IResourceManager _resource = default!;
    
    public override event DrawHandler? OnDraw;
    public override event DebugInfoHandler? OnDebugInfo;

    private GL? _gl;
    private ArrayObject? _vao;
    private BufferObject? _vbo;
    private BufferObject? _ebo;

    protected override string InternalInfo
    {
        get
        {
            var vendor = Gl.GetStringExt(StringName.Vendor);
            var renderer = Gl.GetStringExt(StringName.Renderer);
            var version = Gl.GetStringExt(StringName.Version);
            var shading = Gl.GetStringExt(StringName.ShadingLanguageVersion);

            var result = new StringBuilder();

            result.AppendLine($"Version: {version}");
            result.AppendLine($"Shading: {shading}");
            result.AppendLine($"Vendor: {vendor}");
            result.AppendLine($"Renderer: {renderer}");
            result.Append($"Thread: {Thread.CurrentThread.Name ?? "unnamed"} ({Environment.CurrentManagedThreadId})");
            // result.AppendLine($"Swap interval: {SwapInterval}");

            return result.ToString();
        }
    }

    private GL Gl
    {
        get => _gl ?? throw new NullReferenceException();
        set => _gl = value;
    }

    protected override bool InternalInit(IContextInfo contextInfo)
    {
        Gl = GL.GetApi(contextInfo.GetProcAddress);

        if (Gl.HasErrors())
            return false;

        Gl.DebugMessageCallback(DebugProcCallback, nint.Zero);
        
        Gl.Enable(EnableCap.DebugOutput);
        Gl.Enable(EnableCap.DebugOutputSynchronous);
        
        _vao = GenArrayObject("Main VAO");
        _vbo = GenBufferObject(BufferTargetARB.ArrayBuffer, "Main VBO");
        _ebo = GenBufferObject(BufferTargetARB.ElementArrayBuffer, "Main EBO");
                
        _vao.Bind();
        _vbo.SetData(BatchVertices);
        _ebo.SetData(BatchIndices);
        
        // aPos
        Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 0);
        Gl.EnableVertexAttribArray(0);

        // aColor
        Gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 3 * sizeof(float));
        Gl.EnableVertexAttribArray(1);

        // aTexCoords
        Gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 7 * sizeof(float));
        Gl.EnableVertexAttribArray(2);
        
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
        
        return true;
    }

    protected override void InternalLoad()
    {
        PrimitiveShaderProgram = _resource.Get<Shader>("/shaders/base_primitive.shd");
        TexturingShaderProgram = _resource.Get<Shader>("/shaders/base_texturing.shd");
    }

    protected override void InternalTerminate()
    {
        _vao?.Delete();
        _vbo?.Delete();
        _ebo?.Delete();
    }

    public override void Render(IWindow window)
    {
        Clear();

        Gl.Viewport(0, 0, (uint) window.Size.X, (uint) window.Size.Y);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        OnDraw?.Invoke();

        BreakCurrentBatch();

        Gl.Enable(EnableCap.Blend);
        Gl.Disable(EnableCap.ScissorTest);

        Gl.BlendEquation(BlendEquationModeEXT.FuncAdd);
        Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        Gl.ClearColor(ClearColor);
        
        _vao?.Bind();
        _vbo?.SetData(BatchVertices);
        _ebo?.SetData(BatchIndices);
        
        foreach (var batch in Batches)
        {
            Render(batch);
        }

        _vao?.Unbind();
        _vbo?.Unbind();
        _ebo?.Unbind();
        
        window.SwapBuffers();
    }

    private void Render(Batch batch)
    {
        var shader = PrimitiveShaderProgram;
        
        if (batch.TextureHandle is not null)
        {
            Gl.ActiveTexture(TextureUnit.Texture0);
            Gl.BindTexture(TextureTarget.Texture2D, batch.TextureHandle.Value);
            shader = TexturingShaderProgram;
        }
        
        if (shader is null)
            throw new Exception();
        
        shader.Use();
        shader.SetUniform("model", batch.Model);
        shader.SetUniform("view", _cameraManager.MainCamera.View);
        shader.SetUniform("projection", _cameraManager.MainCamera.Projection);

        Gl.DrawElements(batch.PrimitiveTopology, batch.Size, DrawElementsType.UnsignedInt, batch.Start * sizeof(uint));

        shader.Stop();
        
        Gl.BindTexture(TextureTarget.Texture2D, 0);
    }

    protected override IShader InternalCreateShader(string source, ShaderType type)
    {
        var handle = Gl.CreateShader(type);
        return new GlShader(Gl, handle, type, source);
    }

    protected override IShaderProgram InternalCreateShaderProgram(IEnumerable<IShader> shaders)
    {
        var handle = Gl.CreateProgram();
        return new ShaderProgram(Gl, handle, shaders);
    }

    protected override IShaderProgram InternalCreateShaderProgram(List<IShader> shaders)
    {
        var handle = Gl.CreateProgram();
        return new ShaderProgram(Gl, handle, shaders);
    }

    private void DebugProcCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
    {
        var messageString = Marshal.PtrToStringAnsi(message) ?? "Unknown message";
        OnDebugInfo?.Invoke($"[OpenGL] Source: {source}, Type: {type}, ID: {id}, Severity: {severity}, Message: {messageString}");
    }
}