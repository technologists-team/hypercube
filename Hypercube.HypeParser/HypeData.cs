using Hypercube.HypeParser.Nodes;

namespace Hypercube.HypeParser;

public class HypeData(HypeRootNode root)
{
    public HypeRootNode Root { get; } = root;
    public List<IHypeNode> Nodes { get; } = new();

    public void Clear()
    {
        Root.Clear();
        Nodes.Clear();
    }
}