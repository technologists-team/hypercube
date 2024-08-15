using Hypercube.EventBus;
using Hypercube.EventBus.Events;

namespace Hypercube.UnitTests.EventBus;

public class EventBusUnsubscribeTests
{
    [Test]
    public void Unsubscribing()
    {
        var eventBus = new Hypercube.EventBus.EventBus();
        var subscriber = new TestEventSubscriber(eventBus);
        
        subscriber.Subscribe();
       
        eventBus.Raise(new TestUnsubEventClass());
        eventBus.Raise(new TestUnsubEventStruct());
        
        subscriber.Unsubscribe();
        
        eventBus.Raise(new TestUnsubEventClass());
        eventBus.Raise(new TestUnsubEventStruct());
        
        subscriber.AssertPassed();
    }
    
    private class TestEventSubscriber(IEventBus eventBus) : IEventSubscriber
    {
        private bool _classPassed;
        private bool _structPassed;
        
        public void Subscribe()
        {
            eventBus.Subscribe<TestUnsubEventClass>(this, OnClass);
            eventBus.Subscribe<TestUnsubEventStruct>(this, OnStruct);
        }

        public void Unsubscribe()
        {
            eventBus.Unsubscribe<TestUnsubEventClass>(this);
            eventBus.Unsubscribe<TestUnsubEventStruct>(this);
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

    private record struct TestUnsubEventStruct : IEventArgs;

    private sealed class TestUnsubEventClass : IEventArgs;
}