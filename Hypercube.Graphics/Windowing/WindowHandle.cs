using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

[PublicAPI]
public abstract class WindowHandle
{
    public WindowId Id { get; }
    public nint Pointer { get; }
    public WindowHandle? Owner { get; }
    public bool DisposeOnClose { get; }
    public bool IsDisposed { get; }
    public float Ratio { get; init; }
    public Vector2i Size { get; set; }
    public Vector2i FramebufferSize { get; init; }
    
    protected WindowHandle(WindowId id, nint pointer)
    {
        Id = id;
        Pointer = pointer;
    }
    
    public void SetSize(Vector2i size)
    {
        Size = size;
    }
    
    public void SetSize(int width, int height)
    {
        Size = new Vector2i(width, height);
    }

    public override string ToString()
    {
        return $"{Id}[{Pointer}]";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator WindowId(WindowHandle handle)
    {
        return handle.Id;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator nint(WindowHandle handle)
    {
        return handle.Pointer;
    }
} 