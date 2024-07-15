using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.UnitTests.EventBus;

public class EventBusDirectRaiseTests
{
    [Test]
    public static void DirectRaising()
    {
        var eventBus = new Shared.EventBus.EventBus();
        
        var subscriber1 = new TestSubscriber(eventBus);
        var subscriber2 = new TestSubscriber(eventBus);
        var subscriber3 = new TestSubscriber(eventBus);
        
        subscriber1.Subscribe();
        subscriber2.Subscribe();
        subscriber3.Subscribe();

        var args = new TestEvent();
        eventBus.Raise(subscriber1, ref args);
        
        Assert.That(args.Counter, Is.EqualTo(1));
    }
    
    private sealed class TestSubscriber(IEventBus eventBus) : IEventSubscriber
    {
        public void Subscribe()
        {
            eventBus.Subscribe<TestEvent>(this, RefMethod);
        }

        private void RefMethod(ref TestEvent args)
        {
            args.Counter++;
        }
    }

    private record struct TestEvent(int Counter = 0) : IEventArgs;
}