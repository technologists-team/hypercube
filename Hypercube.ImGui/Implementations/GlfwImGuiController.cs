using Hypercube.Graphics.Shaders;
using Hypercube.Math.Vectors;
using Hypercube.OpenGL.Shaders;
using Hypercube.OpenGL.Utilities.Helpers;
using ImGuiNET;
using JetBrains.Annotations;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTK.Windowing.GraphicsLibraryFramework.GLFWCallbacks;

namespace Hypercube.ImGui.Implementations;

[PublicAPI]
public partial class GlfwImGuiController : IImGuiController, IDisposable
{
    private const int Framerate = 60;
    
    private readonly nint _window;
    
    private readonly bool[] _mousePressed;
    private readonly nint[] _mouseCursors;
    
    private ImGuiIOPtr _io;
    private IShaderProgram _shader = default!;
    private double _time;
    
    private MouseButtonCallback? _mouseButtonCallback;
    private ScrollCallback? _scrollCallback;
    private KeyCallback? _keyCallback;
    private CharCallback? _charCallback;
    
    public GlfwImGuiController(nint window)
    {
        _window = window;
        _mousePressed = new bool[(int) ImGuiMouseButton.COUNT];
        _mouseCursors = new nint[(int) ImGuiMouseCursor.COUNT];
        
        ImGuiNET.ImGui.CreateContext();
        ImGuiNET.ImGui.StyleColorsDark();
    }
    
    public void Initialize()
    {
        InitializeIO();
        InitializeGlfw();
        InitializeShaders();
    }
    
    public void InitializeIO()
    {
        _io = ImGuiNET.ImGui.GetIO();
        
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
    }

    public void InitializeShaders()
    {
        _shader = new ShaderProgram(ShaderSource.VertexShader, ShaderSource.FragmentShader);
    }
    
    public unsafe void Update()
    {
        if (!_io.Fonts.IsBuilt())
            throw new Exception("Unable to update state, without font atlas built");
        
        GLFWHelper.GetWindowSize(_window, out var size);
        GLFWHelper.GetFramebufferSize(_window, out var framebufferSize);

        _io.DisplaySize = size;

        if (size > 0)
            _io.DisplayFramebufferScale = framebufferSize / size;

        var time = GLFW.GetTime();
        var deltaTime = _time > 0 ? time - _time : 1 / (float)Framerate;

        _io.DeltaTime = (float)deltaTime;
        _time = time;
        
        UpdateMousePosition();
        UpdateMouseButtons();
        UpdateMouseCursor();
        
        ImGuiNET.ImGui.NewFrame();
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
}