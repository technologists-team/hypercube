using Hypercube.Client.Graphics.Texturing;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;

namespace Hypercube.Client.Graphics.Rendering;

public sealed partial class Renderer
{
    public void DrawTexture(ITextureHandle textureHandle, Box2 quad, Box2 uv, Color color)
    {
        var bottomLeft = quad.BottomLeft;
        var bottomRight = quad.BottomRight;
        var topRight = quad.TopRight;
        var topLeft = quad.TopLeft;

        var startIndex = _batchVertexIndex;
        _batchVertices[_batchVertexIndex++] = new Vertex(topRight, uv.TopRight, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(bottomRight, uv.BottomRight, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(bottomLeft, uv.BottomLeft, color);
        _batchVertices[_batchVertexIndex++] = new Vertex(topLeft, uv.TopLeft, color);
        
        _batchIndices[_batchIndexIndex++] = startIndex;
        _batchIndices[_batchIndexIndex++] = startIndex + 1;
        _batchIndices[_batchIndexIndex++] = startIndex + 3;
        _batchIndices[_batchIndexIndex++] = startIndex + 1;
        _batchIndices[_batchIndexIndex++] = startIndex + 2;
        _batchIndices[_batchIndexIndex++] = startIndex + 3;
    }   
}