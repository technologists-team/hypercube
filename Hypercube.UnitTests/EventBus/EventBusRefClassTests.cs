using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using EventArgs = Hypercube.Shared.EventBus.Events.EventArgs;

namespace Hypercube.UnitTests.EventBus;

public sealed class EventBusRefClassTests
{
    [Test]
    public static void RefClass()
    {
        var eventBus = new Shared.EventBus.EventBus();
        
        var subscriber1 = new TestRefClassSubscriber1(eventBus);
        var subscriber2 = new TestRefClassSubscriber2(eventBus);
        
        subscriber1.Subscribe();
        subscriber2.Subscribe();

        var args = new TestEventClass();
        eventBus.Raise(args);
        
        Assert.That(args.Counter, Is.EqualTo(2));
        Assert.Pass("All subscribers handled correctly");
    }
    
    private sealed class TestEventClass : EventArgs
    {
        public int Counter { get; set; }
    }

    private sealed class TestRefClassSubscriber1(IEventBus bus) : IEventSubscriber
    {
        public void Subscribe()
        {
            bus.Subscribe<TestEventClass>(this, RefMethod2);
        }

        private void RefMethod2(ref TestEventClass args)
        {
            args.Counter++;
        }
    }
    
    private sealed class TestRefClassSubscriber2(IEventBus bus) : IEventSubscriber
    {
        public void Subscribe()
        {
            bus.Subscribe<TestEventClass>(this, RefMethod1);
        }
        
        private void RefMethod1(ref TestEventClass args)
        {
            Assert.That(args.Counter, Is.EqualTo(1));
            args.Counter++;
        }
    }
}