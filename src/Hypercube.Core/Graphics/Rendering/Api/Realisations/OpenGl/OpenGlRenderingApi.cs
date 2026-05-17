using System.Runtime.InteropServices;
using System.Text;
using Hypercube.Core.Graphics.Rendering.Api.Handlers;
using Hypercube.Core.Graphics.Rendering.Api.Realisations.OpenGl.Objects;
using Hypercube.Core.Graphics.Rendering.Api.Settings;
using Hypercube.Core.Graphics.Rendering.Batching;
using Hypercube.Core.Graphics.Rendering.Shaders;
using Hypercube.Core.Graphics.Utilities.Extensions;
using Hypercube.Core.Resources;
using Hypercube.Core.Viewports;
using Hypercube.Core.Windowing.Api;
using Hypercube.Core.Windowing.Windows;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Shapes;
using Hypercube.Utilities.Dependencies;
using Silk.NET.OpenGL;

using Shader = Hypercube.Core.Graphics.Resources.Shader;
using ShaderType = Hypercube.Core.Graphics.Rendering.Shaders.ShaderType;
using TextureHandle = Hypercube.Core.Graphics.Objects.Texturing.TextureHandle;

namespace Hypercube.Core.Graphics.Rendering.Api.Realisations.OpenGl;

[EngineInternal]
public sealed partial class OpenGlRenderingApi : BaseRenderingApi, IOpenGlRenderingApi
{
    public override RenderingApi Type => RenderingApi.OpenGl;
    
    [Dependency] private readonly ICameraManager _cameraManager = null!;
    [Dependency] private readonly IResourceManager _resource = null!;

    public override event DrawHandler? OnDraw;
    public override event DebugInfoHandler? OnDebugInfo;

    public event Action? OnBeforeBufferSwap;
    
    private ArrayObject _vao = null!;
    private BufferObject _vbo = null!;
    private BufferObject _ebo = null!;

    public GL Gl { get; private set; } = null!;

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

    public OpenGlRenderingApi(RenderingApiSettings settings, IWindowingApi windowingApi) : base(settings, windowingApi)
    {
    }

    public override unsafe TextureHandle CreateTexture(int width, int height, int channels, byte[] data)
    {
        var handle = Gl.GenTexture();
        Gl.BindTexture(TextureTarget.Texture2D, handle);

        var internalFormat = channels switch
        {
            1 => InternalFormat.R8,
            2 => InternalFormat.RG8,
            3 => InternalFormat.Rgb8,
            4 => InternalFormat.Rgba8,
            _ => throw new ArgumentException($"Unsupported channel count: {channels}")
        };

        var format = channels switch
        {
            1 => PixelFormat.Red,
            2 => PixelFormat.RG,
            3 => PixelFormat.Rgb,
            4 => PixelFormat.Rgba,
            _ => throw new ArgumentException($"Unsupported channel count: {channels}")
        };
        
        fixed (byte* dataPointer = data)
            Gl.TexImage2D(TextureTarget.Texture2D, 0, (int) internalFormat, (uint) width, (uint) height, 0, format, PixelType.UnsignedByte, dataPointer);
        
        Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
        Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
        Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
        Gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
        
        Gl.BindTexture(TextureTarget.Texture2D, 0);
        
        return new TextureHandle(handle);
    }

    public override void DeleteTexture(TextureHandle handle)
    {
        if (!handle.HasValue)
            return;
        
        Gl.DeleteTexture(handle);
    }

    public override void Scissor(bool value)
    {
        Gl.SetScissor(value);
    }

    public override void SetScissorRect(Rect2i rect)
    {
        Gl.Scissor(rect.Left, rect.Bottom, (uint) rect.Width, (uint) rect.Height);
    }

    protected override bool InternalInit(IContextInfoProvider contextInfoProvider)
    {
        Gl = GL.GetApi(contextInfoProvider.GetProcAddress);

        if (Gl.HasErrors())
            return false;

        Gl.DebugMessageCallback(DebugProcCallback, in nint.Zero);
        
        Gl.Enable(EnableCap.DebugOutput);
        Gl.Enable(EnableCap.DebugOutputSynchronous);
        
        _vao = GenArrayObject("Main VAO");
        _vbo = GenBufferObject(BufferTargetARB.ArrayBuffer, "Main VBO");
        _ebo = GenBufferObject(BufferTargetARB.ElementArrayBuffer, "Main EBO");
                
        _vao.Bind();
        _vbo.SetData(BatchVertices);
        _ebo.SetData(BatchIndices);

        var pointer = nint.Zero;
        
        // aPos
        Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size, pointer);
        Gl.EnableVertexAttribArray(0);
        
        // aPos offset
        pointer += 3 * sizeof(float);

        // aColor
        Gl.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size, pointer);
        Gl.EnableVertexAttribArray(1);
        
        // aColor offset
        pointer += 4 * sizeof(float);

        // aTexCoords
        Gl.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size, pointer);
        Gl.EnableVertexAttribArray(2);
        
        // aTexCoords offset
        pointer += 2 * sizeof(float);
        
        // aNormal
        Gl.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, Vertex.Size, pointer);
        Gl.EnableVertexAttribArray(3);
        
        // aNormal offset
        pointer += 3 * sizeof(float);
        
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
        
        return true;
    }

    protected override void InternalLoad()
    {
        PrimitiveShaderProgram = _resource.Load<Shader>("/shaders/base_primitive.shd");
        TexturingShaderProgram = _resource.Load<Shader>("/shaders/base_texturing.shd");
    }

    protected override void InternalTerminate()
    {
        _vao.Delete();
        _vbo.Delete();
        _ebo.Delete();
    }

    public override void Render(IWindow window)
    {
        Clear();

        Gl.Viewport(window);
        Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        SetRenderState(_cameraManager);
        
        OnDraw?.Invoke(new DrawPayload(window, _cameraManager.MainCamera));

        BreakCurrentBatch();
        UpdateBatchCount();

        Gl.Enable(EnableCap.Blend);
        Gl.Disable(EnableCap.ScissorTest);
        
        Gl.BlendEquation(BlendEquationModeEXT.FuncAdd);
        Gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        Gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);
        Gl.ClearColor(ClearColor);
        
        _vao.Bind();
        _vbo.SetData(BatchVertices);
        _ebo.SetData(BatchIndices);
        
        foreach (var batch in Batches)
            Render(batch);
        
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
        
        OnBeforeBufferSwap?.Invoke();
        
        window.SwapBuffers();
    }

    private void Render(Batch batch)
    {
        var renderState = GetRenderState(batch.RenderStateId);
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
        shader.SetUniform("model", Matrix4x4.Identity, transpose: true);
        shader.SetUniform("view", renderState.View, transpose: true);
        shader.SetUniform("projection", renderState.Projection, transpose: true);

        Gl.DrawElements(batch.PrimitiveTopology, batch.Size, DrawElementsType.UnsignedInt, batch.Start * sizeof(uint));

        shader.Stop();
        
        Gl.BindTexture(TextureTarget.Texture2D, 0);
    }

    protected override IShader InternalCreateShader(string source, ShaderType type)
    {
        var handle = Gl.CreateShader(type);
        return new OpenGlShader(Gl, handle, type, source);
    }

    protected override IShaderProgram InternalCreateShaderProgram(IEnumerable<IShader> shaders)
    {
        var handle = Gl.CreateProgram();
        return new OpenGlShaderProgram(Gl, handle, shaders);
    }

    protected override IShaderProgram InternalCreateShaderProgram(List<IShader> shaders)
    {
        var handle = Gl.CreateProgram();
        return new OpenGlShaderProgram(Gl, handle, shaders);
    }

    private void DebugProcCallback(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam)
    {
        var messageString = Marshal.PtrToStringAnsi(message) ?? "Unknown message";
        OnDebugInfo?.Invoke($"[OpenGL] Source: {source}, Type: {type}, ID: {id}, Severity: {severity}, Message: {messageString}");
    }
}