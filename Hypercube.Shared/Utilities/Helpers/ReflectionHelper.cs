using System.Collections.Frozen;

namespace Hypercube.Shared.Utilities.Helpers;

public static class ReflectionHelper
{
    public static FrozenSet<Type> GetAllSubclassOf(Type parent)
    {
        var types = new HashSet<Type>();
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsSubclassOf(parent))
                    continue;

                types.Add(type);
            }
        }

        return types.ToFrozenSet();
    }
    
    public static FrozenSet<Type> GetAllInstantiableSubclassOf(Type parent)
    {
        var types = new HashSet<Type>();
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAssignableTo(parent) || type.IsAbstract || type.IsInterface)
                    continue;

                types.Add(type);
            }
        }

        return types.ToFrozenSet();
    }
}