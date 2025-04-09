using Hypercube.Mathematics;

namespace Hypercube.Graphics.Rendering.Api.Settings;

public readonly struct RenderingApiSettings
{
    public static readonly RenderingApiSettings DefaultOpenGL = new()
    {
        Api = RenderingApi.OpenGl,
        IndicesPerVertex = 6,
        MaxVertices = 65532,
        ClearColor = Color.Black
    };
    
    public RenderingApi Api { get; init; }
    public int MaxVertices { get; init; }
    public int IndicesPerVertex { get; init; }
    public Color ClearColor { get; init; }

    public int MaxIndices => MaxVertices * IndicesPerVertex;
}