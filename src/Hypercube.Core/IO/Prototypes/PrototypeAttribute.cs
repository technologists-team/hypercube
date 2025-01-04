namespace Hypercube.Core.IO.Prototypes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class PrototypeAttribute : Attribute
{
    public readonly string Id;

    public PrototypeAttribute(string id)
    {
        Id = id;
    }
}