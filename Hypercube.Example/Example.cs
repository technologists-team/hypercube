using Hypercube.Client.Audio;
using Hypercube.Client.Entities.Systems.Sprite;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Math.Vector;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Scenes;

namespace Hypercube.Example;

public sealed class Example : IEventSubscriber, IPostInject
{
    [Dependency] private readonly IAudioManager _audioManager = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IEntitiesManager _entitiesManager = default!;
    [Dependency] private readonly IEntitiesComponentManager _entitiesComponentManager = default!;

    private readonly Random _random = new();
    
    public void Start(DependenciesContainer root)
    {
        root.Inject(this);
    }
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeStartupEvent>(this, Startup);
    }

    private void Startup(ref RuntimeStartupEvent args)
    {
        for (var i = 0; i < 300; i++)
        {
            var x = _random.NextSingle() * 800 - 400;
            var y = _random.NextSingle() * 800 - 400;

            var coord = new SceneCoordinates(SceneId.Nullspace, new Vector2(x, y));
            CreateEntity(coord);
        }

        var source = _audioManager.CreateSource("/game_boi_3.wav", new AudioSettings());
        source.Start();
    }

    private void CreateEntity(SceneCoordinates coordinates)
    {
        var entity = _entitiesManager.Create("Fuck", coordinates);
        var sprite = _entitiesComponentManager.AddComponent<SpriteComponent>(entity);
        var example = _entitiesComponentManager.AddComponent<ExampleComponent>(entity);
        
        sprite.TexturePath = new ResourcePath("/icon.png");
        example.Offset = _random.Next(0, 1000); 
    }
}