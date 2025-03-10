using JetBrains.Annotations;

namespace Hypercube.Core.Ecs.Attributes;

[MeansImplicitUse, AttributeUsage(AttributeTargets.Class)]
public sealed class RegisterEntitySystemAttribute : Attribute
{
    public readonly Type[] Before;
    public readonly Type[] After;

    public RegisterEntitySystemAttribute(Type[]? before = null, Type[]? after = null)
    {
        Before = before ?? [];
        After = after ?? [];
    }
}