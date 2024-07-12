using Hypercube.Shared.EventBus.Events;

namespace Hypercube.UnitTests.EventBus;

public class EventBusSubRaiseTests
{
    [Test]
    public void SubNRaiseEventTest()
    {
        var evBus = new Shared.EventBus.EventBus();
        var s1 = new TestEventSubscriber(evBus);
        s1.Subscribe();
        evBus.RaiseEvent(new TestSubEventClass());
        evBus.RaiseEvent(new TestSubEventStruct());
        s1.AssertPassed();
    }

    private class TestEventSubscriber(Shared.EventBus.EventBus eventBus) : IEventSubscriber
    {
        private bool _classPassed;
        private bool _structPassed;
        
        public void Subscribe()
        {
            eventBus.SubscribeEvent<TestSubEventClass>(this,OnClassSub);
            eventBus.SubscribeEvent<TestSubEventStruct>(this,OnStructSub);
        }

        private void OnClassSub(ref TestSubEventClass args)
        {
            _classPassed = true;
        }

        private void OnStructSub(ref TestSubEventStruct args)
        {
            _structPassed = true;
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

    private record struct TestSubEventStruct;

    private sealed class TestSubEventClass
    {
    }
}