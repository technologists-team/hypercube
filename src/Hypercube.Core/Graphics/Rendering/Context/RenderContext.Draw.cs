using Hypercube.Core.Graphics.Rendering.Batching;
using Hypercube.Core.Graphics.Resources;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Quaternions;
using Hypercube.Mathematics.Shapes;

namespace Hypercube.Core.Graphics.Rendering.Context;

public sealed partial class RenderContext
{
    public void DrawModel(Model model, Vector3 position, Quaternion rotation, Vector3 scale, Color color, Texture? texture)
    {
        var shader = texture is null ? _renderingApi.PrimitiveShaderProgram : _renderingApi.TexturingShaderProgram;
        if (shader is null)
            throw new Exception("Model shader program is not initialized.");

        if (texture is not null && texture.Gpu is null)
            texture.GpuBind(_renderingApi);
        
        _renderingApi.EnsureBatch(PrimitiveTopology.TriangleList, shader.Handle, texture?.Gpu?.Handle);

        var start = _renderingApi.BatchVerticesIndex;
        var matrix = Matrix4x4.CreateTransformSRT(position, rotation, scale);
        
        foreach (var (v, vt, vn) in model.Indices)
        {
            var vertexPosition = model.Vertices[v] * matrix;
            
            var uv = vt >= 0 && vt < model.UVs.Length ? model.UVs[vt] : Vector2.Zero;
            var normal = vn >= 0 && vn < model.Normals.Length
                ? (model.Normals[vn] * matrix).Normalized
                : Vector3.UnitZ;
    
            _renderingApi.PushVertex(new Vertex(vertexPosition, uv, color, normal));
        }
        
        for (var i = 0; i < model.Indices.Length; i += 3)
        {
            _renderingApi.PushIndex(start, i + 0);
            _renderingApi.PushIndex(start, i + 1);
            _renderingApi.PushIndex(start, i + 2);
        }
    }

    public void DrawTexture(Texture texture, Vector2 position)
    {
        DrawTexture(texture, position, Angle.Zero);
    }

    public void DrawTexture(Texture texture, Vector2 position, Angle rotation)
    {
        DrawTexture(texture, position, rotation, Vector2.One);
    }
    
    public void DrawTexture(Texture texture, Vector2 position, Angle rotation, Vector2 scale) 
    {
        DrawTexture(texture, position, rotation, scale, Color.White, Rect2.UV);
    }
    
    public void DrawTexture(Texture texture, Vector2 position, Angle rotation, Vector2 scale, Color color) 
    {
        DrawTexture(texture, position, rotation, scale, color, Rect2.UV);
    }

    public void DrawTexture(Texture texture, Vector2 position, Angle rotation, Vector2 scale, Color color, Rect2 uv)
    {
        if (_renderingApi.TexturingShaderProgram is null)
            throw new Exception();

        if (texture.Gpu is null)
            texture.GpuBind(_renderingApi);
        
        var halfSize = texture.Size / 2;
        var rect = new Rect4(
            new Vector2(-halfSize.X, -halfSize.Y),
            new Vector2(halfSize.X, -halfSize.Y),
            new Vector2(halfSize.X, halfSize.Y),
            new Vector2(-halfSize.X, halfSize.Y)
        );
        
        var matrix = Matrix4x4.CreateTransformSRT(
            (Vector3) position,
            Quaternion.FromEulerZ((float) rotation),
            (Vector3) scale
        );
        
        _renderingApi.EnsureBatch(PrimitiveTopology.TriangleList, _renderingApi.TexturingShaderProgram.Handle, texture.Gpu?.Handle);
        AddQuadTriangleBatch(_renderingApi.BatchVerticesIndex, rect * matrix, uv, color);
    }
    
    public void DrawRectangle(Rect2 box, Color color, bool outline = false)
    {
        if (_renderingApi.PrimitiveShaderProgram is null)
            throw new Exception();
        
        _renderingApi.EnsureBatch(outline ? PrimitiveTopology.LineList : PrimitiveTopology.TriangleList, _renderingApi.PrimitiveShaderProgram.Handle, null);
        
        if (outline)
        {
            AddQuadTriangleLineBatch(_renderingApi.BatchVerticesIndex, box, Rect2.UV, color);
            return;    
        }
        
        AddQuadTriangleBatch(_renderingApi.BatchVerticesIndex, box, Rect2.UV, color);
    }
    
    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        if (_renderingApi.PrimitiveShaderProgram is null)
            throw new InvalidOperationException("Primitive shader program is not initialized");

        var direction = (end - start).Normalized;
        var normal = new Vector2(-direction.Y, direction.X) * thickness / 2f;

        _renderingApi.EnsureBatch(PrimitiveTopology.TriangleList, _renderingApi.PrimitiveShaderProgram.Handle, null);

        var startIndex = _renderingApi.BatchVerticesIndex;
        
        _renderingApi.PushVertex(new Vertex(start - normal, Vector2.Zero, color));
        _renderingApi.PushVertex(new Vertex(start + normal, Vector2.Zero, color));
        _renderingApi.PushVertex(new Vertex(end - normal, Vector2.Zero, color));
        _renderingApi.PushVertex(new Vertex(end + normal, Vector2.Zero, color));
        
        _renderingApi.PushIndex(startIndex, 0);
        _renderingApi.PushIndex(startIndex, 1);
        _renderingApi.PushIndex(startIndex, 2);
        _renderingApi.PushIndex(startIndex, 1);
        _renderingApi.PushIndex(startIndex, 3);
        _renderingApi.PushIndex(startIndex, 2);
    }

    public void DrawCircle(Vector2 center, float radius, Color color, int segments = 32, bool outline = false)
    {
        if (_renderingApi.PrimitiveShaderProgram is null)
            throw new InvalidOperationException("Primitive shader program is not initialized");
        
        _renderingApi.EnsureBatch(outline ? PrimitiveTopology.LineList : PrimitiveTopology.TriangleList, _renderingApi.PrimitiveShaderProgram.Handle, null);
        
        var startIndex = _renderingApi.BatchVerticesIndex;
        
        if (outline)
        {
            for (var i = 0; i <= segments; i++)
            {
                var angle = (float) i / segments * float.Pi * 2;
                var point = center + new Vector2(float.Cos(angle), float.Sin(angle)) * radius;
                _renderingApi.PushVertex(new Vertex(point, Vector2.Zero, color));
            }

            for (var i = 0; i < segments; i++)
            {
                _renderingApi.PushIndex(startIndex, i);
                _renderingApi.PushIndex(startIndex, i + 1);
            }
            return;
        }
        
        _renderingApi.PushVertex(new Vertex(center, Vector2.Zero, color));
        
        for (var i = 0; i <= segments; i++)
        {
            var angle = (float) i / segments * float.Pi * 2;
            var point = center + new Vector2(float.Cos(angle), float.Sin(angle)) * radius;
            _renderingApi.PushVertex(new Vertex(point, Vector2.Zero, color));
        }
        
        for (var i = 1; i <= segments; i++)
        {
            _renderingApi.PushIndex(startIndex, 0);
            _renderingApi.PushIndex(startIndex, i); 
            _renderingApi.PushIndex(startIndex, i % segments + 1);
        }
    }
    
    private void AddQuadTriangleBatch(int start, Rect4 rect, Rect2 uv, Color color)
    {
        _renderingApi.PushVertex(new Vertex(rect.Point0, uv.TopLeft, color));
        _renderingApi.PushVertex(new Vertex(rect.Point1, uv.TopRight, color));
        _renderingApi.PushVertex(new Vertex(rect.Point2, uv.BottomRight, color));
        _renderingApi.PushVertex(new Vertex(rect.Point3, uv.BottomLeft, color));
        
        _renderingApi.PushIndex(start, 0);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 3);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 2);
        _renderingApi.PushIndex(start, 3);
    }

    private void AddQuadTriangleBatch(int start, Rect2 rect, Rect2 uv, Color color)
    {
        _renderingApi.PushVertex(new Vertex(rect.TopRight, uv.TopRight, color));
        _renderingApi.PushVertex(new Vertex(rect.BottomRight, uv.BottomRight, color));
        _renderingApi.PushVertex(new Vertex(rect.BottomLeft, uv.BottomLeft, color));
        _renderingApi.PushVertex(new Vertex(rect.TopLeft, uv.TopLeft, color));
        
        _renderingApi.PushIndex(start, 0);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 3);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 2);
        _renderingApi.PushIndex(start, 3);
    }
    
    private void AddQuadTriangleLineBatch(int start, Rect2 rect, Rect2 uv, Color color)
    {
        _renderingApi.PushVertex(new Vertex(rect.TopRight, uv.TopRight, color));
        _renderingApi.PushVertex(new Vertex(rect.BottomRight, uv.BottomRight, color));
        _renderingApi.PushVertex(new Vertex(rect.BottomLeft, uv.BottomLeft, color));
        _renderingApi.PushVertex(new Vertex(rect.TopLeft, uv.TopLeft, color));
        
        _renderingApi.PushIndex(start, 0);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 2);
        _renderingApi.PushIndex(start, 2);
        _renderingApi.PushIndex(start, 3);
        _renderingApi.PushIndex(start, 3);
        _renderingApi.PushIndex(start, 0);
    }  
}