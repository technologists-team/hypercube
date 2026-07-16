namespace Hypercube.Graphics.Core.Batching;

public readonly struct BatchGroup
{
    public readonly RenderState State;
    public readonly int Depth;
    public readonly BatchFlags Flags;
    
    public int VertexStart { get; init; }
    public int VertexEnd { get; init; }
    public int IndexStart { get; init; }
    public int IndexEnd { get; init; }
    
    public int VertexCount => VertexEnd - VertexStart;
    public int IndexCount => IndexEnd - IndexStart;

    public BatchGroup(RenderState state, int depth = 0, BatchFlags flags = BatchFlags.None)
    {
        State = state;
        Depth = depth;
        Flags = flags;
    }
    
    public BatchGroup(BatchGroup group)
    {
        State = group.State;
        Depth = group.Depth;
        Flags = group.Flags;
        
        VertexStart = group.VertexStart;
        IndexStart = group.IndexStart;
    }
}