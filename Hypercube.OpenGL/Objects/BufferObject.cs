using System.Runtime.InteropServices;
using Hypercube.OpenGL.Utilities.Helpers;
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
    
    public void SetData(int size, nint data, BufferUsageHint hint = BufferUsageHint.StaticDraw)
    {
        Bind();
        GL.BufferData(BufferTarget, size, data, hint);
    }

    public void SetData<T>(T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw) where T : struct
    {
        Bind();
        GL.BufferData(BufferTarget, data.Length * Marshal.SizeOf(default(T)), data, hint);
    }

    public void SetSubData(int size, nint data)
    {
        Bind();
        GL.BufferSubData(BufferTarget, nint.Zero, size, data);
    }
    
    public void SetSubData<T>(T[] data) where T : struct
    {
        Bind();
        GL.BufferSubData(BufferTarget, nint.Zero, data.Length * Marshal.SizeOf<T>(), data);
    }
    
    public void Label(string name)
    {
        Bind();
        GLHelper.LabelObject(ObjectLabelIdentifier.Buffer, Handle, name);    
    }

    public void Dispose()
    {
        Delete();
    }
}