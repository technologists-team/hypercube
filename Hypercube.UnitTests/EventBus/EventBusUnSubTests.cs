using Hypercube.Shared.EventBus.Events;

namespace Hypercube.UnitTests.EventBus;

public class EventBusUnSubTests
{
    [Test]
    public void TestUnsubscribing()
    {
        var evBus = new Shared.EventBus.EventBus();
        var s1 = new TestEventSubscriber(evBus);
        s1.Subscribe();
        evBus.RaiseEvent(new TestUnsubEventClass());
        evBus.RaiseEvent(new TestUnsubEventStruct());
        s1.Unsubscribe();
        evBus.RaiseEvent(new TestUnsubEventClass());
        evBus.RaiseEvent(new TestUnsubEventStruct());
        s1.AssertPassed();
    }
    
    private class TestEventSubscriber(Shared.EventBus.EventBus eventBus) : IEventSubscriber
    {
        private bool _classPassed;
        private bool _structPassed;
        
        public void Subscribe()
        {
            eventBus.SubscribeEvent<TestUnsubEventClass>(this,OnClassSub);
            eventBus.SubscribeEvent<TestUnsubEventStruct>(this,OnStructSub);
        }

        public void Unsubscribe()
        {
            eventBus.UnsubscribeEvent<TestUnsubEventClass>(this);
            eventBus.UnsubscribeEvent<TestUnsubEventStruct>(this);
        }

        private void OnClassSub(ref TestUnsubEventClass args)
        {
            _classPassed = !_classPassed;
        }

        private void OnStructSub(ref TestUnsubEventStruct args)
        {
            _structPassed = !_structPassed;
        }

        public void AssertPassed()
        {
            if (_classPassed && _structPassed)
            {
                Assert.Pass("Both passed");
                return;
            }
            
            if (_classPassed)
            {
                Assert.Fail("Struct failed");
                return;
            }
            
            if (_structPassed)
                Assert.Fail("Class failed");
        }
    }

    private record struct TestUnsubEventStruct;

    private sealed class TestUnsubEventClass
    {
    }
}