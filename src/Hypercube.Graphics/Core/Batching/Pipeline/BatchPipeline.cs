using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Core.Batching.Pipeline;

public class BatchPipeline
{
    private readonly List<IBatchPipelineStage> _stages = [];

    public void AddStage(IBatchPipelineStage stage)
    {
        _stages.Add(stage);
    }

    public void Process(ReadOnlySpan<Vertex> vertices, ReadOnlySpan<uint> indices, Span<BatchResult> results)
    {
        var state = new PipelineState
        {
            Vertices = vertices,
            Indices = indices,
            Results = results,
            ResultCount = 0
        };

        foreach (var stage in _stages)
        {
            stage.Execute(ref state);
        }
    }
}

public interface IBatchPipelineStage
{
    void Execute(ref PipelineState state);
}

public ref struct PipelineState
{
    public ReadOnlySpan<Vertex> Vertices;
    public ReadOnlySpan<uint> Indices;
    public Span<BatchResult> Results;
    public int ResultCount;
    
    public void AddResult(BatchResult result)
    {
        if (ResultCount < Results.Length)
            Results[ResultCount++] = result;
    }
}

public struct BatchResult
{
    public int VertexStart;
    public int VertexEnd;
    
    public int IndexStart;
    public int IndexEnd;
    
    public int VertexCount => VertexEnd - VertexStart;
    public int IndexCount => IndexEnd - IndexStart;
}