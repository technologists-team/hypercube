using System.Text;

namespace Hypercube.HypeParser.Nodes;

public class HypeMappingNode : IHypeNode
{
    public IHypeNode? ParentNode { get; }
    public string? Name { get; }
    public object Value => _nodes;
    public Type? ValueType { get; }

    private List<IHypeNode> _nodes = new();

    public HypeMappingNode(IHypeNode? parent, string? name, Type? valueType = null)
    {
        ParentNode = parent;
        Name = name;
        ValueType = valueType;
    }

    public void Add(IHypeNode node)
    {
        _nodes.Add(node);
    }

    public void Clear()
    {
        _nodes.Clear();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append(Name);
        builder.AppendLine(":");
        foreach (var node in _nodes)
        {
            builder.AppendLine($"   {node.ToString()}");
        }

        return builder.ToString();

    }
}