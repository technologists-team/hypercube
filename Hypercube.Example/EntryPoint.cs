using Hypercube.Shared.EventBus;

namespace Hypercube.Example;

internal class EntryPoint : IEventSubscriber
{
    public static void Main(string[] args)
    {
        var example = new Example();
        Client.EntryPoint.Enter(args, example.Start);
    }
}