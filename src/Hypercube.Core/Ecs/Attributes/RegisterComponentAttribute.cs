using JetBrains.Annotations;

namespace Hypercube.Core.Ecs.Attributes;

[MeansImplicitUse, AttributeUsage(AttributeTargets.Class)]
public class RegisterComponentAttribute : Attribute;