using Hypercube.Dependencies;
using Hypercube.Shared;
using Hypercube.Utilities;

namespace Hypercube.Client;

public sealed class EntryPoint : SharedEntryPoint
{
    protected override void Enter(ArgumentParser parser, DependenciesContainer rootContainer)
    {
        Dependencies.Register(rootContainer);
        MountFolders.Mount(rootContainer);
    }
}