using Hypercube.Client;
using Hypercube.Client.Entities.Systems.Sprite;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Components;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Entities.Systems.MetaData;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Runtimes.Event;

namespace Hypercube.UnitTests.Entities;

public class QueriesTests
{
    private IEntitiesComponentManager _components = default!;
    private IEntitiesManager _entitiesManager = default!;
    private IEventBus _eventBus = default!;
    private Random _random = new();

    // its funny, but SetUpAttribute doesn't have a way to run once before all tests, so I had to do this in constructor
    public QueriesTests()
    {
        var container = DependencyManager.InitThread();
        Dependencies.Register(container);

        _components = container.Resolve<IEntitiesComponentManager>();
        _entitiesManager = container.Resolve<IEntitiesManager>();
        _eventBus = container.Resolve<IEventBus>();
        
        // little hack so comp manager could collect components' types
        _eventBus.Raise(new RuntimeInitializationEvent());
        
        // create 20 entities for testing
        for (var i = 0; i < 20; i++)
        {
            var giveSprite = _random.NextSingle() > 0.4f;
            var giveJeffrey = _random.NextSingle() > 0.5f;
            var entity = _entitiesManager.Create("TestEntity", SceneCoordinates.Nullspace);
            
            if (giveSprite)
                _components.AddComponent<SpriteComponent>(entity);

            if (giveJeffrey)
                _components.AddComponent<JeffreyComponent>(entity);
        }
    }
    
    [Test]
    public void TestGetEntitiesT1()
    {
        foreach (var ent in _components.GetEntities<SpriteComponent>())
        {
            Assert.Multiple(() =>
            {
                Assert.That(_components.HasComponent<SpriteComponent>(ent));
                Assert.That(ent.Component is not null);
            });
        }
        
        Assert.Pass("Successfully got all entities with T1 components");
    }

    [Test]
    public void TestGetEntitiesT1T2()
    {
        foreach (var ent in _components.GetEntities<MetaDataComponent, SpriteComponent>())
        {
            Assert.Multiple(() =>
            {
                Assert.That(_components.HasComponent<SpriteComponent>(ent) && _components.HasComponent<MetaDataComponent>(ent));
                Assert.That(ent.Component1 is not null && ent.Component2 is not null);
            });
        }
        
        Assert.Pass("Successfully got all entities with T1 and T2 components");
    }

    [Test]
    public void TestGetEntitiesT1T2T3()
    {
        foreach (var ent in _components.GetEntities<MetaDataComponent, SpriteComponent, JeffreyComponent>())
        {
            Assert.Multiple(() =>
            {
                Assert.That(_components.HasComponent<SpriteComponent>(ent) && 
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
/// Hi, im Jeffrey, im here so my owner could test queries, I won't leave long but I lived happily
/// </summary>
public sealed class JeffreyComponent : Component
{
}