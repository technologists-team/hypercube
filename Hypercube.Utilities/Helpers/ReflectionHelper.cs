using System.Collections.Frozen;
using System.Reflection;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Helpers;

[PublicAPI]
public static class ReflectionHelper
{
    public static T? GetAttribute<T>(MethodInfo method)
        where T : Attribute
    {
        foreach (var customAttribute in method.GetCustomAttributes())
        {
            if (customAttribute is not T attribute)
                continue;

            return attribute;
        }

        return null;
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