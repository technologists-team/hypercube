using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Math;
using Hypercube.Math.Matrices;
using Hypercube.Math.Shapes;
using OpenTK.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using Box2 = Hypercube.Math.Shapes.Box2;
using Vector2 = Hypercube.Math.Vectors.Vector2;
using Vector4 = System.Numerics.Vector4;

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
        EnsureBatchState(null, PrimitiveType.Points, _primitiveShaderProgram.Handle);
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
        EnsureBatchState(null, PrimitiveType.Lines, _primitiveShaderProgram.Handle);
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
        BreakCurrentBatch();
        EnsureBatchState(null, PrimitiveType.LineLoop, _primitiveShaderProgram.Handle);
        AddCircleBatch((uint)_batchVertexIndex, circle, color, 20);
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
        BreakCurrentBatch();
        EnsureBatchState(null, outline ? PrimitiveType.LineLoop : PrimitiveType.Triangles, _primitiveShaderProgram.Handle);
        AddQuadBatch((uint)_batchVertexIndex, model.Transform(box), Box2.UV, color);
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
        BreakCurrentBatch();
        EnsureBatchState(null, outline ? PrimitiveType.LineLoop : PrimitiveType.TriangleFan, _primitiveShaderProgram.Handle);
        
        var startIndex = (uint)_batchVertexIndex;
        
        uint i = 0;
        foreach (var vertex in vertices)
        {
            _batchVertices[_batchVertexIndex++] = new Vertex(model.Transform(vertex), Vector2.Zero, color);
            _batchIndices[_batchIndexIndex++] = startIndex + i;
            i++;
        }
    }
    
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color)
    {
        DrawTexture(texture, quad, uv, color, Matrix4X4.Identity);
    }

    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model)
    {
        EnsureBatchState(texture.Handle, PrimitiveType.Triangles, _texturingShaderProgram.Handle);
        
        var startIndex = (uint)_batchVertexIndex;
        //_batches.Add(new Batch(_batchIndexIndex, 6, texture.Handle, PrimitiveType.Triangles, model));
        AddQuadBatch(startIndex, model.Transform(quad), uv, color);
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
    
    private void AddCircleBatch(uint startIndex, Circle circle, Color color, int segments)
    {
        var total = HyperMathF.TwoPI / segments;
        for (var i = 0; i < segments; i++)
        {
            var theta = total * i;
            var x = circle.Position.X + circle.Radius * MathF.Cos(theta);
            var y = circle.Position.Y + circle.Radius * MathF.Sin(theta);
            
            _batchVertices[_batchVertexIndex++] = new Vertex(new Vector2(x, y), Vector2.Zero, color);
            _batchIndices[_batchIndexIndex++] = startIndex + (uint)i;
        }
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

    private void EnsureBatchState(int? texHandle, PrimitiveType primitiveType, int shaderInstance)
    {
        if (_currentBatchData is not null)
        {
            var data = _currentBatchData.Value;
            if (data.PrimitiveType == primitiveType &&
                data.TextureHandle == texHandle &&
                data.ShaderHandle == shaderInstance)
            {
                return;
            }
            FinalizeCurrentBatch();
        }
        
        _currentBatchData = new BatchData(texHandle, shaderInstance, primitiveType, _batchIndexIndex);
    }
}