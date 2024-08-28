using JetBrains.Annotations;

namespace Hypercube.Dependencies;

[PublicAPI]
public interface IPostInject
{
    void PostInject();
}