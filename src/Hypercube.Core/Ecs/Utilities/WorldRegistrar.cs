using Hypercube.Core.Ecs.Attributes;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Core.Ecs.Utilities;

public sealed class WorldRegistrar
{
    public List<Type> GetTypes()
    {
        // Get all types with the RegisterEntitySystemAttribute
        var systemTypes = ReflectionHelper.GetAllTypesWithAttribute<RegisterEntitySystemAttribute>();
        var graph = new Dictionary<Type, List<Type>>();
        
        // Initialize the graph for all system types
        foreach (var (type, _) in systemTypes)
        {
            graph[type] = [];
        }
        
        // Build the dependency graph based on Before and After attributes
        foreach (var (type, attribute) in systemTypes)
        {
            foreach (var before in attribute.Before)
            {
                if (!graph.ContainsKey(before))
                    graph[before] = [];
                
                // Add dependency: before -> type
                graph[before].Add(type);
            }
            
            foreach (var after in attribute.After)
            {
                // Add dependency: type -> after
                graph[type].Add(after);
            }
        }
        
        // Perform topological sorting to determine the registration order
        var sortedTypes = new List<Type>();
        var visited = new HashSet<Type>();
        var tempVisited = new HashSet<Type>();

        foreach (var systemType in graph.Keys)
        {
            if (visited.Contains(systemType))
                continue;
            
            if (TopologicalSort(systemType, graph, visited, tempVisited, sortedTypes))
                continue;
            
            throw new InvalidOperationException("Cyclic dependency detected in system registration.");
        }

        return sortedTypes;
    }

    // Recursive method for topological sorting
    private static bool TopologicalSort(Type type, Dictionary<Type, List<Type>> graph, HashSet<Type> visited, HashSet<Type> tempVisited, List<Type> sortedTypes)
    {
        // If the type is temporarily visited, a cycle is detected
        if (tempVisited.Contains(type))
            return false;

        // If the type is already fully processed, skip it
        if (visited.Contains(type))
            return true;

        // Mark the type as temporarily visited
        tempVisited.Add(type);

        // Recursively process all dependencies
        if (graph.TryGetValue(type, out var dependencies))
        {
            foreach (var dependency in dependencies)
            {
                if (!TopologicalSort(dependency, graph, visited, tempVisited, sortedTypes))
                    return false;
            }
        }

        // Remove the type from temporarily visited and add it to the sorted list
        tempVisited.Remove(type);
        visited.Add(type);
        sortedTypes.Add(type);

        return true;
    }
}