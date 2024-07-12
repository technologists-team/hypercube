using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.EventBus.Events.Events;

namespace Hypercube.UnitTests.EventBus;

public static class EventBusRaiseTests
{
    [Test]
    public static void Raising()
    {
        var eventBus = new Shared.EventBus.EventBus();
        
        var subscriber = new TestEventSubscriber(eventBus);
        subscriber.Subscribe();
        
        eventBus.Raise(new TestSubEventClass());
        eventBus.Raise(new TestSubEventStruct());
        
        subscriber.AssertPassed();
    }

    private sealed class TestEventSubscriber(IEventBus eventBus) : IEventSubscriber
    {
        private bool _classPassed;
        private bool _structPassed;
        
        public void Subscribe()
        {
            eventBus.Subscribe<TestSubEventClass>(this, OnClass);
            eventBus.Subscribe<TestSubEventStruct>(this, OnStruct);
        }

        private void OnClass(ref TestSubEventClass args)
        {
            _classPassed = true;
        }

        private void OnStruct(ref TestSubEventStruct args)
        {
            _structPassed = true;
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

    private readonly record struct TestSubEventStruct : IEventArgs;

    private sealed class TestSubEventClass : IEventArgs;
}