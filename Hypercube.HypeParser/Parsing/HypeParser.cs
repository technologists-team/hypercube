using System.Diagnostics;
using System.Reflection;
using Hypercube.HypeParser.Nodes;

namespace Hypercube.HypeParser.Parsing;

public class HypeParser
{
    public HypeData Data = new(new HypeRootNode());
    private IHypeNode _parentNode;
    private IHypeNode _currentNode;
    private List<Assembly> _assemblies; 
    
    public static void Test()
    {
        var source = @"
settings:
  flipped: true
  parameters:
    TextureMinFilter: Nearest
copyright:
  author: Test
  license: MIT
";
        var st = Stopwatch.StartNew();
        var hype = new HypeParser([Assembly.GetExecutingAssembly()]);
        hype.Parse(source);
        st.Stop();
        Console.WriteLine(st.Elapsed);
        Console.WriteLine(hype.Data);
    }

    public HypeParser(List<Assembly> assemblies)
    {
        _assemblies = assemblies;
    }
    
    public void Parse(string source)
    {
        _parentNode = Data.Root;
        
        FixEOF(ref source);
        var rawData = ParseLines(source);
        
        IHypeNode node;
        foreach (var line in rawData)
        {
            if (line == string.Empty)
                continue;
            
            var type = GetNodeType(line);
            node = ParseNode(type, line);
            Data.Nodes.Add(node);
        }

        foreach (var n in Data.Nodes.Where(n => n.ParentNode is HypeRootNode root))
        {
            Console.WriteLine(n.ToString());
        }
    }

    private NodeType GetNodeType(string line)
    {
        var split = line.Split(':');
        
        if (IsTyped(line))
        {
            return GetTypedNodeType(split);
        }

        return GetNonTypedNodeType(split);
    }

    private bool IsTyped(string line)
    {
        return line.Contains('.');
    }

    private NodeType GetTypedNodeType(string[] kvp)
    {
        
        switch (kvp.Length)
        {
            case 2:
                if (kvp[1] == string.Empty)
                    throw new InvalidOperationException("Typed mapping???");
                return NodeType.ScalarTyped;
            case 1:
                return NodeType.ScalarTyped;
        }

        throw new InvalidOperationException();
    }

    private NodeType GetNonTypedNodeType(string[] kvp)
    {
        switch (kvp.Length)
        {
            case 2:
                if (kvp[1] == string.Empty)
                    return NodeType.Mapping;
                return NodeType.Scalar;
        }

        throw new Exception();
    }

    private IHypeNode ParseNode(NodeType type, string node)
    {
        var split = node.Split(":");
        IHypeNode parsedNode;
        switch (type)
        {
            case NodeType.Mapping:
            {
                var name = split[0];
                TryRemoveWhitespaces(ref name);
                parsedNode = new HypeMappingNode(_parentNode, name, typeof(Dictionary<string, object>));
                if (_parentNode is HypeMappingNode mappingNode)
                    mappingNode.Add(parsedNode);
                
                _parentNode = (HypeMappingNode)parsedNode;
                
                break;
            }
            case NodeType.Scalar:
            {
                var name = split[0];
                var value = split[1];
                TryRemoveWhitespaces(ref name);
                TryRemoveWhitespaces(ref value);
                parsedNode = new HypeScalarNode(_parentNode, name, value, null);
                
                if (_parentNode is HypeMappingNode mappingNode)
                    mappingNode.Add(parsedNode);
                
                break;
            }
            case NodeType.ScalarTyped:
            {
                var name = split[0];
                var valueSplit = split[1].Split(".");
                var value = valueSplit[1];
                var valueType = valueSplit[0];
                
                TryRemoveWhitespaces(ref name);
                TryRemoveWhitespaces(ref value);
                TryRemoveWhitespaces(ref valueType);
                
                var parsedType = GetTypeByName(valueType);
                if (parsedType == null)
                    throw new InvalidOperationException();
                parsedNode = new HypeScalarNode(_parentNode, name, value, parsedType);
                
                if (_parentNode is HypeMappingNode mappingNode)
                    mappingNode.Add(parsedNode);
                
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        return parsedNode;
    }

    private void FixEOF(ref string source)
    {
        source = source
            .Replace("\r\n", "\n")
            .Replace('\r', '\n');
    }

    private string[] ParseLines(string source)
    {
        return source.Split('\n');
    }

    private Type? GetTypeByName(string name)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            // Search for the type in the current assembly without namespace
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == name);
            if (type != null)
            {
                return type;
            }
        }
        return null;
    }

    private void TryRemoveWhitespaces(ref string line)
    {
        if (!line.Contains(' '))
        {
            _parentNode = Data.Root;
            return;
        }
        line = line.Replace(" ", "");
    }

    enum SomeOtherType
    {
        Nearest
    }
}