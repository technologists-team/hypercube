using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Math;
using Hypercube.Math.Boxs;
using Hypercube.Math.Matrixs;
using Hypercube.Math.Vectors;
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
        var startIndex = (uint)_batchVertexIndex;
        _batches.Add(new Batch(_batchIndexIndex, 1, null, PrimitiveType.Points, model));
        AddPointBatch(startIndex, vector, color);
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
        var startIndex = (uint)_batchVertexIndex;
        _batches.Add(new Batch(_batchIndexIndex, 2, null, PrimitiveType.Lines, model));
        AddLineBatch(startIndex, pointA, pointB, color);
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
        var startIndex = (uint)_batchVertexIndex;
        _batches.Add(new Batch(_batchIndexIndex, 2, null, PrimitiveType.Lines, model));
        AddLineBatch(startIndex, box, color);
    }

    public void DrawRectangle(Box2 box, Color color)
    {
        DrawRectangle(box, color, Matrix4X4.Identity);
    }

    public void DrawRectangle(Box2 box, Color color, Matrix3X3 model)
    {
        DrawRectangle(box, color, Matrix4X4.CreateIdentity(model));
    }
    
    public void DrawRectangle(Box2 box, Color color, Matrix4X4 model)
    {
        var startIndex = (uint)_batchVertexIndex;
        _batches.Add(new Batch(_batchIndexIndex, 6, null, PrimitiveType.Triangles, model));
        AddQuadBatch(startIndex, box, Box2.UV, color);
    }
    
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color)
    {
        DrawTexture(texture, quad, uv, color, Matrix4X4.Identity);
    }

    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model)
    {
        var startIndex = (uint)_batchVertexIndex;
        
        _batches.Add(new Batch(_batchIndexIndex, 6, texture.Handle, PrimitiveType.Triangles, model));
        AddQuadBatch(startIndex, quad, uv, color);
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
    
    private void AddQuadBatch(uint startIndex, Box2 quad, Box2 uv, Color color)
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