using ImGuiNET;
using JetBrains.Annotations;

namespace Hypercube.ImGui;

[PublicAPI]
public class GlfwImGuiController : IDisposable
{
    private readonly nint _window;
    
    private readonly bool[] _mousePressed;
    private readonly nint[] _mouseCursors;
    
    private ImGuiIOPtr _io;

    public GlfwImGuiController(nint window)
    {
        _window = window;
        _mousePressed = new bool[(int) ImGuiMouseButton.COUNT];
        _mouseCursors = new nint[(int) ImGuiMouseCursor.COUNT];
        
        ImGuiNET.ImGui.CreateContext();
        ImGuiNET.ImGui.StyleColorsDark();
        
        InitializeIo();
        InitializeGlfw();
    }
    
    public void InitializeIo()
    {
        _io = ImGuiNET.ImGui.GetIO();
        
        _io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
        _io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
        _io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
    }

    public void InitializeGlfw()
    {
        
    }

    public void Dispose()
    {
        
    }
}