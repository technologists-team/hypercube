using Hypercube.Graphics.Core.Batching;
using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Core.Device.Modules;

public class GraphicDeviceBatcher : GraphicDeviceModule
{
    private readonly List<Batch> _items = new (1024);
    private readonly List<BatchGroup> _groups = new(64);

    private List<Vertex> _vertices = new(4096);
    private List<uint> _indices = new(8192);
    
    public RenderState State;
    
    public GraphicDeviceBatcher(GraphicDevice device) : base(device)
    {
    }

    public void AddGeometry(Vertex[] vertices, uint[] indices, int depth = 0, BatchFlags flags = BatchFlags.None)
    {
        AddGeometry(State, vertices, indices, depth, flags);
    }
    
    public void AddGeometry(RenderState state, Vertex[] vertices, uint[] indices, int depth = 0, BatchFlags flags = BatchFlags.None)
    {
        if (vertices.Length == 0 || indices.Length == 0)
            return;

        _items.Add(new Batch
        {
            State =  state,
            Vertices = vertices,
            Indices = indices,
            Depth = depth,
            Flags = flags
        });
    }

    public BatchFlushResult Flush()
    {
        _groups.Clear();
        _vertices.Clear();
        _indices.Clear();

        if (_items.Count == 0)
            return BatchFlushResult.Empty;

        var layered = new List<Batch>();
        var unlayered = new List<Batch>();

        foreach (var item in _items)
        {
            if ((item.Flags & BatchFlags.IgnoreLayering) != 0)
            {
                unlayered.Add(item);
                continue;
            }

            layered.Add(item);
        }
        
        layered.Sort((a, b) => a.Depth.CompareTo(b.Depth));

        Merge(unlayered, static (group, batch) => group.State == batch.State);
        Merge(layered, static (group, batch) => group.State == batch.State && group.Depth == batch.Depth);


        return new BatchFlushResult
        {
            Vertices = _vertices.ToArray(),
            Indices = _indices.ToArray(),
            Groups = _groups.ToArray()
        };
    }
    
    private void Merge(List<Batch> items, Func<BatchGroup, Batch, bool> predicate)
    {
        BatchGroup? group = null;
        foreach (var item in items)
        {
            group ??= new BatchGroup(item.State);
            
            var vStart = _vertices.Count;
            var iStart = _indices.Count;
            
            Append(item.Vertices, item.Indices);
            
            var vEnd = _vertices.Count;
            var iEnd = _indices.Count;
            
            var mergeable = predicate(group.Value, item);
            if (mergeable)
            {
                group = new BatchGroup(group.Value)
                {
                    VertexEnd = vEnd,
                    IndexEnd = iEnd,
                };
                
                continue;
            }

            group = new BatchGroup(group.Value)
            {
                VertexStart = vStart,
                VertexEnd = vEnd,
                IndexStart = iStart,
                IndexEnd = iEnd
            };
            
            _groups.Add(group.Value);
        }
        
        if (group is null)
            return;
        
        _groups.Add(group.Value);
    }
    
    private void Append(Vertex[] vertices, uint[] indices)
    {
        var baseVertex = (uint) _vertices.Count;
        _vertices.AddRange(vertices);
        
        for (var i = 0u; i < indices.Length; i++)
        {
            _indices.Add(indices[i] + baseVertex);
        }
    }
}
