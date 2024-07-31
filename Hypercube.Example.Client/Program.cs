using Hypercube.Shared.EventBus;

namespace Hypercube.Example.Client;

internal class Program : IEventSubscriber
{
    public static void Main(string[] args)
    {
        var example = new Example();
        var entry = new Hypercube.Client.EntryPoint();
        entry.Enter(args, example.Start);
    }
}