namespace Hypercube.HypeParser.Nodes;

public interface IHypeNode
{
    public IHypeNode? ParentNode { get; }
    public string? Name { get; }
    public object Value { get; }
    public Type? ValueType { get; }
}