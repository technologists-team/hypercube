using Hypercube.Shared.EventBus;

namespace Hypercube.Example.Server;

internal class Program : IEventSubscriber
{
    public static void Main(string[] args)
    {
        var entry = new Hypercube.Server.EntryPoint();
        entry.Enter(args);
    }
}