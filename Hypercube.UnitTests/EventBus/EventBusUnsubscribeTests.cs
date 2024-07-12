using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;

namespace Hypercube.UnitTests.EventBus;

public class EventBusUnsubscribeTests
{
    [Test]
    public void Unsubscribing()
    {
        var eventBus = new Shared.EventBus.EventBus();
        var subscriber = new TestEventSubscriber(eventBus);
        
        subscriber.Subscribe();
       
        eventBus.RaiseEvent(new TestUnsubEventClass());
        eventBus.RaiseEvent(new TestUnsubEventStruct());
        
        subscriber.Unsubscribe();
        
        eventBus.RaiseEvent(new TestUnsubEventClass());
        eventBus.RaiseEvent(new TestUnsubEventStruct());
        
        subscriber.AssertPassed();
    }
    
    private class TestEventSubscriber(IEventBus eventBus) : IEventSubscriber
    {
        private bool _classPassed;
        private bool _structPassed;
        
        public void Subscribe()
        {
            eventBus.SubscribeEvent<TestUnsubEventClass>(this, OnClass);
            eventBus.SubscribeEvent<TestUnsubEventStruct>(this, OnStruct);
        }

        public void Unsubscribe()
        {
            eventBus.UnsubscribeEvent<TestUnsubEventClass>(this);
            eventBus.UnsubscribeEvent<TestUnsubEventStruct>(this);
        }

        private void OnClass(ref TestUnsubEventClass args)
        {
            _classPassed = !_classPassed;
        }

        private void OnStruct(ref TestUnsubEventStruct args)
        {
            _structPassed = !_structPassed;
        }

        public void AssertPassed()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_classPassed, Is.True);
                Assert.That(_structPassed, Is.True);
            });
        }
    }

    private record struct TestUnsubEventStruct;

    private sealed class TestUnsubEventClass;
}