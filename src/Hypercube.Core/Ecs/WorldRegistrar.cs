using Hypercube.Core.Ecs.Attributes;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.Ecs;

public sealed class WorldRegistrar
{
    private readonly IWorld _world;
    
    public WorldRegistrar(IWorld world)
    {
        _world = world;
    }
    
    public void Register()
    {
        var systemTypes = ReflectionHelper.GetAllTypesWithAttribute<RegisterEntitySystemAttribute>();
        var graph = new Dictionary<Type, List<Type>>();
        
        foreach (var (type, attribute) in systemTypes)
        {
            graph[type] = [];
            
            foreach (var before in attribute.Before)
            {
                graph[type].Add(before);
            }
            
            foreach (var after in attribute.After)
            {
                graph[after].Add(type);
            }
        }
        
        var sortedTypes = new List<Type>();
        var visited = new HashSet<Type>();

        foreach (var (systemType, _) in systemTypes)
        {
            Visit(systemType, graph, visited, sortedTypes);
        }
        
        foreach (var systemType in sortedTypes)
        {
            _world.AddSystem(systemType);
        }
    }

    private static void Visit(Type type, Dictionary<Type, List<Type>> graph, HashSet<Type> visited, List<Type> sortedTypes)
    {
        if (!visited.Add(type))
            return;

        if (graph.TryGetValue(type, out var value))
        {
            foreach (var dependency in value)
            {
                Visit(dependency, graph, visited, sortedTypes);
            }
        }
        
        sortedTypes.Add(type);
    }
}