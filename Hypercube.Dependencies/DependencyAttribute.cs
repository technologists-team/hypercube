using JetBrains.Annotations;

namespace Hypercube.Dependencies;

[PublicAPI, AttributeUsage(AttributeTargets.Field)]
public class DependencyAttribute : Attribute;