using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.UnitTests.EventBus;

public static class EventBusRefStructTests
{
    [Test]
    public static void RefStruct()
    {
        var eventBus = new Shared.EventBus.EventBus();
        
        var subscriber1 = new TestRefStructSubscriber1(eventBus);
        var subscriber2 = new TestRefStructSubscriber2(eventBus);
        
        subscriber1.Subscribe();
        subscriber2.Subscribe();
        
        var args = new TestEventStruct();
        eventBus.Raise(ref args);
        
        Assert.That(args.Counter, Is.EqualTo(2));
        Assert.Pass("All subscribers handled correctly");
    }
    
    private sealed class TestRefStructSubscriber1(IEventBus eventBus) : IEventSubscriber
    {
        public void Subscribe()
        {
            eventBus.Subscribe<TestEventStruct>(this, RefMethod);
        }

        private void RefMethod(ref TestEventStruct args)
        {
            args.Counter++;
        }
    }
    
    private sealed class TestRefStructSubscriber2(IEventBus eventBus) : IEventSubscriber
    {
        public void Subscribe()
        {
            eventBus.Subscribe<TestEventStruct>(this, RefMethod);
        }
        
        private void RefMethod(ref TestEventStruct args)
        {
            Assert.That(args.Counter, Is.EqualTo(1));
            args.Counter++;
        }
    }

    private record struct TestEventStruct(int Counter = 0) : IEventArgs;
}