using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL;

public sealed class ArrayObject
{
    public const int Null = 0;
    
    public readonly int Handle;

    private bool _binded;
    
    public ArrayObject()
    {
        Handle = GL.GenVertexArray();
    }
    
    public void Bind()
    {
        if (_binded) 
            return;

        _binded = true;
        GL.BindVertexArray(Handle);
    }

    public void Unbind()
    {
        if (!_binded)
            return;

        _binded = false;
        GL.BindVertexArray(Null);
    }

    public void DrawElements(int start, int count)
    {
        DrawElements(BeginMode.Triangles, start, count, DrawElementsType.UnsignedInt);
    }
    
    public void DrawElements(BeginMode mode, int start, int count, DrawElementsType type)
    {
        GL.DrawElements(mode, count, type, start);
    }
}