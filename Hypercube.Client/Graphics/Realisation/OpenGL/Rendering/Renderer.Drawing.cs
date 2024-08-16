using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using OpenToolkit.Graphics.OpenGL4;
namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    public void DrawPoint(Vector2 vector, Color color)
    {
        DrawPoint(vector, color, Matrix4X4.Identity);
    }

    public void DrawPoint(Vector2 vector, Color color, Matrix3X3 model)
    {
        DrawPoint(vector, color, Matrix4X4.CreateIdentity(model));
    }

    public void DrawPoint(Vector2 vector, Color color, Matrix4X4 model)
    {
        BreakCurrentBatch();
        EnsureBatch(PrimitiveType.Points, null, _primitiveShaderProgram.Handle);
        AddPointBatch((uint)_batchVertexIndex, model.Transform(vector), color);
    }

    public void DrawLine(Vector2 pointA, Vector2 pointB, Color color)
    {
        DrawLine(pointA, pointB, color, Matrix4X4.Identity);
    }

    public void DrawLine(Vector2 pointA, Vector2 pointB, Color color, Matrix3X3 model)
    {
        DrawLine(pointA, pointB, color, Matrix4X4.CreateIdentity(model));
    }

    public void DrawLine(Vector2 pointA, Vector2 pointB, Color color, Matrix4X4 model)
    {
        DrawLine(new Box2(pointA, pointB), color, model);
    }

    public void DrawLine(Box2 box, Color color)
    {
        DrawLine(box, color, Matrix4X4.Identity);
    }

    public void DrawLine(Box2 box, Color color, Matrix3X3 model)
    {
        DrawLine(box, color, Matrix4X4.CreateIdentity(model));
    }

    public void DrawLine(Box2 box, Color color, Matrix4X4 model)
    {
        EnsureBatch(PrimitiveType.Lines, null, _primitiveShaderProgram.Handle);
        AddLineBatch((uint)_batchVertexIndex, model.Transform(box), color);
    }

    public void DrawCircle(Circle circle, Color color)
    {
        DrawCircle(circle, color, Matrix4X4.Identity);
    }

    public void DrawCircle(Circle circle, Color color, Matrix3X3 model)
    {
        DrawCircle(circle, color, Matrix4X4.CreateIdentity(model));
    }

    public void DrawCircle(Circle circle, Color color, Matrix4X4 model)
    {
        EnsureBatch(PrimitiveType.Lines, null, _primitiveShaderProgram.Handle);
        AddCircleBatch(_batchVertexIndex, circle, color, 20);
    }

    public void DrawRectangle(Box2 box, Color color, bool outline = false)
    {
        DrawRectangle(box, color, Matrix4X4.Identity, outline);
    }

    public void DrawRectangle(Box2 box, Color color, Matrix3X3 model, bool outline = false)
    {
        DrawRectangle(box, color, Matrix4X4.CreateIdentity(model), outline);
    }
    
    public void DrawRectangle(Box2 box, Color color, Matrix4X4 model, bool outline = false)
    {
        EnsureBatch(outline ? PrimitiveType.Lines : PrimitiveType.Triangles, null, _primitiveShaderProgram.Handle);
        AddQuadTriangleBatch((uint)_batchVertexIndex, model.Transform(box), Box2.UV, color);
    }
    
    public void DrawPolygon(Vector2[] vertices, Color color, bool outline = false)
    {
        DrawPolygon(vertices, color, Matrix4X4.Identity, outline);
    }

    public void DrawPolygon(Vector2[] vertices, Color color, Matrix3X3 model, bool outline = false)
    {
        DrawPolygon(vertices, color, Matrix4X4.CreateIdentity(model), outline);
    }

    public void DrawPolygon(Vector2[] vertices, Color color, Matrix4X4 model, bool outline = false)
    {
        if (outline)
        {
            DrawPolygonOutline(vertices, color, model);
            return;
        }
        
        BreakCurrentBatch();
        EnsureBatch(PrimitiveType.TriangleFan, null, _primitiveShaderProgram.Handle);
        var startIndex = (uint)_batchVertexIndex;
        
        uint i = 0;
        foreach (var vertex in vertices)
        {
            _batchVertices[_batchVertexIndex++] = new Vertex(model.Transform(vertex), Vector2.Zero, color);
            _batchIndices[_batchIndexIndex++] = startIndex + i;
            i++;
        }
    }

    private void DrawPolygonOutline(Vector2[] vertices, Color color, Matrix4X4 model)
    {
        EnsureBatch(PrimitiveType.Lines, null, _primitiveShaderProgram.Handle);
        var startIndex = (uint)_batchVertexIndex;
        
        uint i = 0;
        foreach (var vertex in vertices)
        {
            _batchVertices[_batchVertexIndex++] = new Vertex(model.Transform(vertex), Vector2.Zero, color);
            
            if (i >= vertices.Length - 1)
            {
                _batchIndices[_batchIndexIndex++] = startIndex + i;
                _batchIndices[_batchIndexIndex++] = startIndex;
                continue;
            }
            
            _batchIndices[_batchIndexIndex++] = startIndex + i;
            _batchIndices[_batchIndexIndex++] = startIndex + i + 1;
            
            i++;
        }
    }
    
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color)
    {
        DrawTexture(texture, quad, uv, color, Matrix4X4.Identity);
    }

    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix3X3 model)
    {
        DrawTexture(texture, quad, uv, color, Matrix4X4.CreateIdentity(model));
    }
    
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model)
    {
        EnsureBatch(PrimitiveType.Triangles, texture.Handle, _texturingShaderProgram.Handle);
        AddQuadTriangleBatch(_batchVertexIndex, model.Transform(quad), uv, color);
    }

    private void AddPointBatch(uint startIndex, Vector2 point, Color color)
    {
        _batchVertices[_batchVertexIndex++] = new Vertex(point, Vector2.Zero, color);
        _batchIndices[_batchIndexIndex++] = startIndex;
    }
    
    private void AddLineBatch(uint startIndex, Vector2 pointA, Vector2 pointB, Color color)
    {
        _batchVertices[_batchVertexIndex++] = new Vertex(pointA, Vector2.Zero, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(pointB, Vector2.Zero, color);
        _batchIndices[_batchIndexIndex++] = startIndex;
        _batchIndices[_batchIndexIndex++] = startIndex + 1;
    }
    
    private void AddLineBatch(uint startIndex, Box2 box2, Color color)
    {
        _batchVertices[_batchVertexIndex++] = new Vertex(box2.TopRight, Vector2.Zero, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(box2.BottomLeft, Vector2.Zero, color);
        _batchIndices[_batchIndexIndex++] = startIndex;
        _batchIndices[_batchIndexIndex++] = startIndex + 1;
    }

    private void AddCircleBatch(int startIndex, Circle circle, Color color, int segments)
    {
        AddCircleBatch((uint)startIndex, circle, color, segments);    
    }
    
    private void AddCircleBatch(uint startIndex, Circle circle, Color color, int segments)
    {
        var total = HyperMathF.TwoPI / segments;
        for (var i = 0; i < segments; i++)
        {
            var theta = total * i;
            var x = circle.Position.X + circle.Radius * MathF.Cos(theta);
            var y = circle.Position.Y + circle.Radius * MathF.Sin(theta);
            
            _batchVertices[_batchVertexIndex++] = new Vertex(new Vector2(x, y), Vector2.Zero, color);

            if (i >= segments - 1)
            {
                _batchIndices[_batchIndexIndex++] = startIndex + (uint)i;
                _batchIndices[_batchIndexIndex++] = startIndex;
                continue;
            }
            
            _batchIndices[_batchIndexIndex++] = startIndex + (uint)i;
            _batchIndices[_batchIndexIndex++] = startIndex + (uint)i + 1;
        }
    }

    private void AddQuadTriangleBatch(int startIndex, Box2 quad, Box2 uv, Color color)
    {
        AddQuadTriangleBatch((uint)startIndex, quad, uv, color);
    }
    
    private void AddQuadTriangleBatch(uint startIndex, Box2 quad, Box2 uv, Color color)
    {
        _batchVertices[_batchVertexIndex++] = new Vertex(quad.TopRight, uv.TopRight, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(quad.BottomRight, uv.BottomRight, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(quad.BottomLeft, uv.BottomLeft, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(quad.TopLeft, uv.TopLeft, color);
        
        _batchIndices[_batchIndexIndex++] = startIndex;
        _batchIndices[_batchIndexIndex++] = startIndex + 1;
        _batchIndices[_batchIndexIndex++] = startIndex + 3;
        _batchIndices[_batchIndexIndex++] = startIndex + 1;
        _batchIndices[_batchIndexIndex++] = startIndex + 2;
        _batchIndices[_batchIndexIndex++] = startIndex + 3;
    }
}