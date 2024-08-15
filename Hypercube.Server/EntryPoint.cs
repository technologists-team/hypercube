using Hypercube.Dependencies;
using Hypercube.Shared;
using Hypercube.Shared.Utilities.ArgumentsParser;

namespace Hypercube.Server;

public sealed class EntryPoint : SharedEntryPoint
{
    protected override void Enter(ArgumentParser parser, DependenciesContainer rootContainer)
    {
        Dependencies.Register(rootContainer);
    }
}