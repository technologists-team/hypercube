namespace Hypercube.HypeParser.Nodes;

public class HypeScalarNode : IHypeNode
{
    public IHypeNode? ParentNode { get; }
    public string? Name { get; }
    public object Value { get; }
    public Type? ValueType { get; }

    public HypeScalarNode(IHypeNode? parent, string? name, object value, Type? valueType)
    {
        ParentNode = parent;
        Name = name;
        Value = value;
        ValueType = valueType;
    }

    public override string ToString()
    {
        return $"{Name}: {ValueType?.Name} {Value}";
    }
}