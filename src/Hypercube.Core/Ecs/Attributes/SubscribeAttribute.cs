using JetBrains.Annotations;

namespace Hypercube.Core.Ecs.Attributes;

[MeansImplicitUse,  AttributeUsage(AttributeTargets.Method)]
public sealed class SubscribeAttribute : Attribute;