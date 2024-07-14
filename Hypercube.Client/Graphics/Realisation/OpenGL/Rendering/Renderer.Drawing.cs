using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Math;
using Hypercube.Math.Box;
using Hypercube.Math.Matrix;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color)
    {
        DrawTexture(texture, quad, uv, color, Matrix4X4.Identity);
    }

    public void DrawTexture(ITextureHandle texture, Box2 quad, Box2 uv, Color color, Matrix4X4 model)
    {
        var startIndex = (uint)_batchVertexIndex;
        _batches.Add(new Batch(_batchIndexIndex, 6, texture.Handle, PrimitiveType.Triangles, model));
        
        var bottomLeft = quad.BottomLeft;
        var bottomRight = quad.BottomRight;
        var topRight = quad.TopRight;
        var topLeft = quad.TopLeft;
        
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