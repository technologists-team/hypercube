using Hypercube.Core.Analyzers;
using Hypercube.Graphics.Rendering.Api;
using Hypercube.Graphics.Rendering.Batching;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Rendering.Context;

[EngineInternal]
[UsedImplicitly]
public sealed class RenderContext : IRenderContext
{
    private IRenderingApi _renderingApi = default!;
    
    public void Init(IRenderingApi api)
    {
        _renderingApi = api;
    }

    public void DrawTexture(Texture texture, Vector2 position, Color color)
    {
        if (_renderingApi.PrimitiveShaderProgram is null)
            throw new Exception();

        var box = new Box2(position, texture.Size);
        AddQuadTriangleBatch(_renderingApi.BatchVerticesIndex, Matrix4x4.Identity.Transform(box), Box2.UV, color);
    }
    
    public void DrawRectangle(Box2 box, Color color, bool outline = false)
    {
        if (_renderingApi.PrimitiveShaderProgram is null)
            throw new Exception();
        
        _renderingApi.EnsureBatch(outline ? PrimitiveTopology.LineList : PrimitiveTopology.TriangleList, _renderingApi.PrimitiveShaderProgram.Handle, null);
        AddQuadTriangleBatch(_renderingApi.BatchVerticesIndex, Matrix4x4.Identity.Transform(box), Box2.UV, color);
    }
    
    private void AddQuadTriangleBatch(int start, Box2 quad, Box2 uv, Color color)
    {
        _renderingApi.PushVertex(new Vertex(quad.TopRight, uv.TopRight, color));
        _renderingApi.PushVertex(new Vertex(quad.BottomRight, uv.BottomRight, color));
        _renderingApi.PushVertex(new Vertex(quad.BottomLeft, uv.BottomLeft, color));
        _renderingApi.PushVertex(new Vertex(quad.TopLeft, uv.TopLeft, color));
        
        _renderingApi.PushIndex(start, 0);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 3);
        _renderingApi.PushIndex(start, 1);
        _renderingApi.PushIndex(start, 2);
        _renderingApi.PushIndex(start, 3);
    }

    private void AddLineBatch(int start, Box2 box2, Color color)
    {
        _renderingApi.PushVertex(new Vertex(box2.TopRight, Vector2.Zero, color));
        _renderingApi.PushVertex(new Vertex(box2.BottomLeft, Vector2.Zero, color));
        _renderingApi.PushIndex(start, 0);
        _renderingApi.PushIndex(start, 1);
    }
    
    private void AddPointBatch(int start, Vector2 point, Color color)
    {
        _renderingApi.PushVertex(new Vertex(point, Vector2.Zero, color));
        _renderingApi.PushIndex(start, 0);
    }
}