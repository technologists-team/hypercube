using System.Runtime.InteropServices;
using JetBrains.Annotations;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.OpenGL.Objects;

[PublicAPI]
public class BufferObject : IDisposable
{
    public const int Null = 0;
    
    public readonly int Handle;
    public readonly BufferTarget BufferTarget;

    private bool _bound;
    
    public BufferObject(BufferTarget target)
    {
        Handle = GL.GenBuffer();
        BufferTarget = target;
    }

    public void Bind()
    {
        if (_bound) 
            return;

        _bound = true;
        GL.BindBuffer(BufferTarget, Handle);
    }

    public void Unbind()
    {
        if (!_bound)
            return;

        _bound = false;
        GL.BindBuffer(BufferTarget, Null);
    }

    public void Delete()
    {
        GL.DeleteBuffer(Handle);
    }
    
    public void SetData<T>(T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw) where T : struct
    {
        Bind();
        GL.BufferData(BufferTarget, data.Length * Marshal.SizeOf(default(T)), data, hint);
    }

    public void Dispose()
    {
        Delete();
    }
}