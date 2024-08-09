using JetBrains.Annotations;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.OpenGL.Objects;

[PublicAPI]
public class ArrayObject
{
    public const int Null = 0;
    
    public readonly int Handle;

    private bool _bound;
    
    public ArrayObject()
    {
        Handle = GL.GenVertexArray();
    }
    
    public void Bind()
    {
        if (_bound) 
            return;

        _bound = true;
        GL.BindVertexArray(Handle);
    }

    public void Unbind()
    {
        if (!_bound)
            return;

        _bound = false;
        GL.BindVertexArray(Null);
    }

    public static void DrawElements(int start, int count)
    {
        DrawElements(BeginMode.Triangles, start, count, DrawElementsType.UnsignedInt);
    }
    
    public static void DrawElements(BeginMode mode, int start, int count, DrawElementsType type)
    {
        GL.DrawElements(mode, count, type, start);
    }
}