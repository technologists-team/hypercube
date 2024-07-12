using Hypercube.Shared.EventBus.Events;
using EventArgs = Hypercube.Shared.EventBus.Events.EventArgs;

namespace Hypercube.UnitTests.EventBus;

public class EventBusRefTests : IEventSubscriber
{
    /*
     * Class Ref Tests
     */
    
    [Test]
    public void TestRef()
    {
        var evBus = new Shared.EventBus.EventBus();
        var s1 = new TestRefClassSubscriber1(evBus);
        var s2 = new TestRefClassSubscriber2(evBus);
        s1.Subscribe();
        s2.Subscribe();
        evBus.RaiseEvent(new TestEventClass());
    }
    
    private sealed class TestEventClass() : EventArgs
    {
        public int Counter { get; set; } = 0;
    }

    private class TestRefClassSubscriber1(Shared.EventBus.EventBus bus) : IEventSubscriber
    {
        public void Subscribe()
        {
            bus.SubscribeEvent<TestEventClass>(this, RefFunc2);
        }

        private void RefFunc2(ref TestEventClass args)
        {
            args.Counter++;
        }
    }
    private class TestRefClassSubscriber2(Shared.EventBus.EventBus bus) : IEventSubscriber
    {
        public void Subscribe()
        {
            bus.SubscribeEvent<TestEventClass>(this, RefFunc1);
        }
        private void RefFunc1(ref TestEventClass args)
        {
            Assert.That(args.Counter == 1);
        
            Assert.Pass("Counter increased correctly");
        }
    }
    
    /*
     * Structs Ref Tests
     */
    
    [Test]
    public void TestRefStruct()
    {
        var evBus = new Shared.EventBus.EventBus();
        var s1 = new TestRefStructSubscriber1(evBus);
        var s2 = new TestRefStructSubscriber2(evBus);
        s1.Subscribe();
        s2.Subscribe();
        evBus.RaiseEvent(new TestEventStruct());
    }
    
    
    private class TestRefStructSubscriber1(Shared.EventBus.EventBus bus) : IEventSubscriber
    {
        public void Subscribe()
        {
            bus.SubscribeEvent<TestEventStruct>(this, RefFunc2);
        }

        private void RefFunc2(ref TestEventStruct args)
        {
            args.Counter++;
        }
    }
    private class TestRefStructSubscriber2(Shared.EventBus.EventBus bus) : IEventSubscriber
    {
        public void Subscribe()
        {
            bus.SubscribeEvent<TestEventStruct>(this, RefFunc1);
        }
        private void RefFunc1(ref TestEventStruct args)
        {
            Assert.That(args.Counter == 1);
        
            Assert.Pass("Counter increased correctly");
        }
    }

    private record struct TestEventStruct(int Counter = 0);
}