using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Runtime.Events;
using Hypercube.Shared;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Entities.Systems.MetaData;
using Hypercube.Shared.Entities.Systems.Transform;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;

namespace Hypercube.UnitTests.Entities;

public sealed class QueriesTests
{
    [Dependency] private readonly IEntitiesComponentManager _components = default!;
    [Dependency] private readonly IEntitiesManager _entitiesManager = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    private readonly Random _random = new();

    // Its funny, but SetUpAttribute doesn't have a way to run once before all tests, so I had to do this in constructor
    public QueriesTests()
    {
        var container = DependencyManager.InitThread();
        SharedDependencies.Register(container);
        container.Inject(this);
        
        // Little hack so comp manager could collect components' types
        _eventBus.Raise(new RuntimeInitializationEvent());
        
        // Create 20 entities for testing
        for (var i = 0; i < 20; i++)
        {
            var giveJeffrey = _random.NextSingle() > 0.5f;
            var entity = _entitiesManager.Create("TestEntity", SceneCoordinates.Nullspace);
            
            if (giveJeffrey)
                _components.AddComponent<JeffreyComponent>(entity);
        }
    }
    
    [Test]
    public void TestGetEntitiesT1()
    {
        foreach (var ent in _components.GetEntities<TransformComponent>())
        {
            Assert.Multiple(() =>
            {
                Assert.That(_components.HasComponent<TransformComponent>(ent));
                Assert.That(ent.Component is not null);
            });
        }
        
        Assert.Pass("Successfully got all entities with T1 components");
    }

    [Test]
    public void TestGetEntitiesT1T2()
    {
        foreach (var ent in _components.GetEntities<MetaDataComponent, TransformComponent>())
        {
            Assert.Multiple(() =>
            {
                Assert.That(_components.HasComponent<TransformComponent>(ent) && _components.HasComponent<MetaDataComponent>(ent));
                Assert.That(ent.Component1 is not null && ent.Component2 is not null);
            });
        }
        
        Assert.Pass("Successfully got all entities with T1 and T2 components");
    }

    [Test]
    public void TestGetEntitiesT1T2T3()
    {
        foreach (var ent in _components.GetEntities<MetaDataComponent, TransformComponent, JeffreyComponent>())
        {
            Assert.Multiple(() =>
            {
                Assert.That(_components.HasComponent<TransformComponent>(ent) && 
                            _components.HasComponent<MetaDataComponent>(ent) &&
                            _components.HasComponent<JeffreyComponent>(ent));
                
                Assert.That(ent.Component1 is not null && 
                            ent.Component2 is not null &&
                            ent.Component3 is not null);
            });
            
            Assert.Pass("Successfully got all entities with T1 and T2 and T3 components");
        }
    }
}

/// <summary>
/// Hi, im Jeffrey, im here so my owner could test queries, I won't leave long, but I lived happily
/// </summary>
public sealed class JeffreyComponent : Component;