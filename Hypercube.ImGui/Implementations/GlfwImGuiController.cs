using System.Diagnostics;
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
    
    private MouseButtonCallback? _mouseButtonCallback;
    private ScrollCallback? _scrollCallback;
    private KeyCallback? _keyCallback;
    private CharCallback? _charCallback;
    
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
        InitializeGlfw();
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

    public unsafe void InitializeGlfw()
    {
        _mouseButtonCallback = OnMouseButton;
        _scrollCallback = OnScroll;
        _keyCallback = OnKey;
        _charCallback = OnChar;
        
        CheckErrors("Glfw initialized");
    }

    public void InitializeShaders()
    {
        _shader = new ShaderProgram(ShaderSource.VertexShader, ShaderSource.FragmentShader);
        _shader.Label("ImGui");
        
        _vao = new ArrayObject();
        _vao.Label("ImGui");
        
        _vbo = new BufferObject(BufferTarget.ArrayBuffer);
        _vbo.Label("ImGui");
        
        _ebo = new BufferObject(BufferTarget.ElementArrayBuffer);
        _ebo.Label("ImGui");
        
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
        
        GLFWHelper.GetWindowSize(_window, out var size);
        GLFWHelper.GetFramebufferSize(_window, out var framebufferSize);

        _io.DisplaySize = size;
        if (size > 0)
            _io.DisplayFramebufferScale = framebufferSize / size;
        
        _io.DeltaTime = deltaTime;

        UpdateMousePosition();
        UpdateMouseButtons();
        UpdateMouseCursor();
        
        ImGuiNET.ImGui.NewFrame();
    }

    public void Begin(string name)
    {
        ImGuiNET.ImGui.Begin(name);
    }

    public void End()
    {
        ImGuiNET.ImGui.End();
    }
    
    public void Dispose()
    {
        
    }
    
    private unsafe void OnMouseButton(Window* window, MouseButton button, InputAction action, KeyModifiers mods)
    {

    }
    
    private unsafe void OnScroll(Window* window, double offsetx, double offsety)
    {

    }
    
    private unsafe void OnKey(Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
    {

    }
    
    private unsafe void OnChar(Window* window, uint codepoint)
    {

    }
    
    private void CreateFontsTexture()
    {
        _io.Fonts.GetTexDataAsRGBA32(out nint pixels, out var width, out var height);

        GL.ActiveTexture(TextureUnit.Texture0);
        
        var texture = GL.GenTexture();        
        GL.BindTexture(TextureTarget.Texture2D, texture);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);

        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        
        _io.Fonts.SetTexID(texture);
        _io.Fonts.ClearTexData();
    }
    
    private void CheckErrors(string title)
    {
        var errorString = GLHelper.CheckErrors($"{nameof(GlfwImGuiController)} {title}");
        if (errorString == string.Empty)
            return;
        
        OnErrorHandled?.Invoke(errorString);
    }
}