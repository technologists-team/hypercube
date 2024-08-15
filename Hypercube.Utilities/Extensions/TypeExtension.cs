using System.Reflection;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

[PublicAPI]
public static class TypeExtension
{
    /// <summary>
    /// Returns absolutely all fields, privates, readonlies, and ones from parents.
    /// </summary>
    public static IEnumerable<FieldInfo> GetAllFields(this Type type)
    {
        foreach (var p in GetClassHierarchy(type))
        {
            foreach (var field in p.GetFields(
                         BindingFlags.NonPublic |
                         BindingFlags.Instance |
                         BindingFlags.DeclaredOnly |
                         BindingFlags.Public))
            {
                yield return field;
            }
        }
    }
    
    public static IEnumerable<Type> GetClassHierarchy(this Type t)
    {
        yield return t;

        while (t.BaseType != null)
        {
            t = t.BaseType;
            yield return t;
        }
    }
}