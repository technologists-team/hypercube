using System.Runtime.CompilerServices;
using Hypercube.Graphics.Shaders;
using Hypercube.Graphics.Windowing;
using Hypercube.OpenGL.Objects;
using Hypercube.OpenGL.Shaders;
using Hypercube.OpenGL.Utilities.Helpers;
using ImGuiNET;
using JetBrains.Annotations;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenToolkit.Graphics.OpenGL4;
using static OpenTK.Windowing.GraphicsLibraryFramework.GLFWCallbacks;

namespace Hypercube.ImGui.Implementations;

[PublicAPI]
public partial class GlfwImGuiController : IImGuiController, IDisposable
{
    private const int Framerate = 60;
    
    public event Action<string>? OnErrorHandled;
    
    private readonly WindowHandle _window;
    
    private readonly bool[] _mousePressed;
    private readonly nint[] _mouseCursors;
    
    private ImGuiIOPtr _io;
    private IShaderProgram _shader = default!;
    
    private ArrayObject _vao = default!;
    private BufferObject _vbo = default!;
    private BufferObject _ebo = default!;
    
    private int _vertexBufferSize;
    private int _indexBufferSize;
    private bool _frameBegun;
    
    public GlfwImGuiController(WindowHandle window)
    {
        _window = window;
        _mousePressed = new bool[(int) ImGuiMouseButton.COUNT];
        _mouseCursors = new nint[(int) ImGuiMouseCursor.COUNT];
    }
    
    public void Initialize()
    {
        var context = ImGuiNET.ImGui.CreateContext();
        ImGuiNET.ImGui.SetCurrentContext(context);
        
        ImGuiNET.ImGui.StyleColorsDark();
        
        InitializeIO();
        InitializeShaders();
    }
    
    public void InitializeIO()
    {
        _io = ImGuiNET.ImGui.GetIO();
        
        _io.Fonts.AddFontDefault();
        
        _io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
        _io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
        _io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;

        _io.ClipboardUserData = _window;
    }
    
    public void InitializeShaders()
    {
        _shader = new ShaderProgram(ShaderSource.VertexShader, ShaderSource.FragmentShader);
        _shader.Label("ImGui shader");
        
        _vao = new ArrayObject();
        _vao.Label("ImGui vao");
        
        _vbo = new BufferObject(BufferTarget.ArrayBuffer);
        _vbo.Label("ImGui vbo");
        
        _ebo = new BufferObject(BufferTarget.ElementArrayBuffer);
        _ebo.Label("ImGui ebo");
        
        var stride = Unsafe.SizeOf<ImDrawVert>();
        
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.UnsignedByte, true, stride, 16);
        GL.EnableVertexAttribArray(1);
        
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, stride, 8);
        GL.EnableVertexAttribArray(2);

        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
        
        CreateFontsTexture();
        
        CheckErrors("OpenGL objects initialized");
    }
    
    public void Update(float deltaTime)
    {
        if (!_io.Fonts.IsBuilt())
            throw new Exception("Unable to update state, without font atlas built");

        if (_frameBegun)
        {
            ImGuiNET.ImGui.Render();
        }
        
        GLFWHelper.GetWindowSize(_window, out var size);
        GLFWHelper.GetFramebufferSize(_window, out var framebufferSize);

        _io.DisplaySize = size;
        if (size > 0)
            _io.DisplayFramebufferScale = framebufferSize / size;
        
        _io.DeltaTime = deltaTime;

        _frameBegun = true;
        ImGuiNET.ImGui.NewFrame();
    }

    public void Begin(string name)
    {
        ImGuiNET.ImGui.Begin(name);
    }

    public void Text(string label)
    {
        ImGuiNET.ImGui.Text(label);
    }

    public bool Button(string label)
    {
        return ImGuiNET.ImGui.Button(label);
    }
    
    public void End()
    {
        ImGuiNET.ImGui.End();
    }

    public void DockSpaceOverViewport()
    {
        ImGuiNET.ImGui.DockSpaceOverViewport();
    }

    public void ShowDemoWindow()
    {
       ImGuiNET.ImGui.ShowDemoWindow();
    }

    public void ShowDebugInput()
    {
        Begin("ImGui input");
        
        Text($"Mouse LBM: {_io.MouseDown[0]}");
        Text($"Mouse RBM: {_io.MouseDown[1]}");
        Text($"Mouse position: {_io.MousePos}");
        Text($"Mouse wheel: <{_io.MouseWheel}, {_io.MouseWheelH}>");
        
        End();
    }

    public void Dispose()
    {
        
    }
    
    private void CreateFontsTexture()
    {
        _io.Fonts.GetTexDataAsRGBA32(out nint pixels, out var width, out var height, out var bytesPerPixel);
        var mips = (int)Math.Floor(Math.Log(Math.Max(width, height), 2));
        
        GL.ActiveTexture(TextureUnit.Texture0);
        
        var texture = GL.GenTexture();

        GL.BindTexture(TextureTarget.Texture2D, texture);
        GLHelper.LabelObject(ObjectLabelIdentifier.Texture, texture, "ImGui font texture");

        GL.TexStorage2D(TextureTarget2d.Texture2D, mips, SizedInternalFormat.Rgba8, width, height);
        GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, mips - 1);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        

        _io.Fonts.SetTexID(texture);
        _io.Fonts.ClearTexData();
        
        GLHelper.UnbindTexture(TextureTarget.Texture2D);
    }
    
    private void CheckErrors(string title)
    {
        var errorString = GLHelper.CheckErrors($"{nameof(GlfwImGuiController)} {title}");
        if (errorString == string.Empty)
            return;
        
        OnErrorHandled?.Invoke(errorString);
    }
}