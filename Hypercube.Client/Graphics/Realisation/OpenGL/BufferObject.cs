using Hypercube.Shared.Utilities.Helpers;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL;

public sealed class BufferObject : IDisposable
{
    public const int Null = 0;
    
    public readonly int Handle;
    public readonly BufferTarget BufferTarget;

    private bool _binded;
    
    public BufferObject(BufferTarget target)
    {
        Handle = GL.GenBuffer();
        BufferTarget = target;
    }

    public void Bind()
    {
        if (_binded) 
            return;

        _binded = true;
        GL.BindBuffer(BufferTarget, Handle);
    }

    public void Unbind()
    {
        if (!_binded)
            return;

        _binded = false;
        GL.BindBuffer(BufferTarget, Null);
    }

    public void Delete()
    {
        GL.DeleteBuffer(Handle);
    }
    
    public void SetData<T>(T[] data, BufferUsageHint hint = BufferUsageHint.StaticDraw) where T : struct
    {
        Bind();
        GL.BufferData(BufferTarget, data.Length * MarshalHelper.SizeOf<T>(), data, hint);
    }

    public void Dispose()
    {
        Delete();
    }
}