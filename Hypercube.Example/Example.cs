using Hypercube.Client.Audio;
using Hypercube.Client.Audio.Resources;
using Hypercube.Client.Entities.Systems.Sprite;
using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Math;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Entities.Systems.Physics;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Physics.Shapes;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Scenes;

namespace Hypercube.Example;

public sealed class Example : IEventSubscriber, IPostInject
{
    [Dependency] private readonly IAudioManager _audioManager = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IEntitiesManager _entitiesManager = default!;
    [Dependency] private readonly IEntitiesComponentManager _entitiesComponentManager = default!;
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly IResourceCacher _resourceCacher = default!;

    private readonly Random _random = new();
    
    public void Start(string[] args, DependenciesContainer root)
    {
        root.Inject(this);
    }
    
    public void PostInject()
    {
        _eventBus.Subscribe<RuntimeStartupEvent>(this, Startup);
    }

    private void Startup(ref RuntimeStartupEvent args)
    {
        for (var i = 0; i < 400; i++)
        {
            var x = _random.NextSingle() * 10 - 5;
            var y = _random.NextSingle() * 10 - 5;

            var coord = new SceneCoordinates(SceneId.Nullspace, new Vector2(x, y));
            CreateEntity(coord);
        }

        var stream = _resourceCacher.GetResource<AudioResource>("/game_boi_3.wav").Stream;
        var source = _audioManager.CreateSource(stream);
            
        // it's too loud :D
        source.Gain = 0.1f;
        source.Start();
    }

    private void CreateEntity(SceneCoordinates coordinates)
    {
        var entityUid = _entitiesManager.Create("Fuck", coordinates);

        _entitiesComponentManager.AddComponent<PhysicsComponent>(entityUid, entity =>
        {
            entity.Component.Shape = new CircleShape(1f);
        });
        
        _entitiesComponentManager.AddComponent<SpriteComponent>(entityUid, entity =>
        {
            entity.Component.TexturePath = new ResourcePath("/Textures/icon.png");
        });
        
        _entitiesComponentManager.AddComponent<ExampleComponent>(entityUid, entity =>
        {
            entity.Component.Offset = _random.Next(0, 1000);
        });
    }
}