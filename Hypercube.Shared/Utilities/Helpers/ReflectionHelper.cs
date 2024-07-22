using System.Collections.Frozen;
using System.Reflection;

namespace Hypercube.Shared.Utilities.Helpers;

/// <summary>
/// Reflection is a very handy tool for automating work.
/// It can be resource-intensive, so try to use it only at the moment of initialization.
/// </summary>
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

    public static FrozenSet<Type> GetAllInstantiableSubclassOf<T>()
    {
        return GetAllInstantiableSubclassOf(typeof(T));
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