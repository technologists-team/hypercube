namespace Hypercube.Graphics.Core;

public readonly record struct VertexAttribute(
    string SemanticName,
    uint Location,
    int ComponentCount,
    VertexAttributeType Type,
    bool Normalized = false)
{
    public int GetSize() => GetSize(Type);

    private static int GetSize(VertexAttributeType type) => type switch
    {
        VertexAttributeType.Float => sizeof(float),
        VertexAttributeType.Int   => sizeof(int),
        VertexAttributeType.Byte  => sizeof(byte),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };
}
