using System.Runtime.CompilerServices;
using Hypercube.Math.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Windowing;

[PublicAPI]
public abstract class WindowHandle
{
    public readonly WindowId Id;
    public readonly nint Pointer;
    
    public WindowHandle? Owner;
    
    public bool DisposeOnClose;
    public bool IsDisposed;

    public float Ratio { get; init; }
    public Vector2Int Size { get; set; }
    public Vector2Int FramebufferSize { get; init; }
    
    protected WindowHandle(WindowId id, nint pointer)
    {
        Id = id;
        Pointer = pointer;
    }
    
    public void SetSize(Vector2Int size)
    {
        Size = size;
    }
    
    public void SetSize(int width, int height)
    {
        Size = new Vector2Int(width, height);
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