using Hypercube.Client.Audio;
using Hypercube.Client.Audio.Resources;
using Hypercube.Client.Entities.Systems.Sprite;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Example.Controls;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Entities.Systems.Physics;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.Physics;
using Hypercube.Shared.Physics.Shapes;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Container;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Scenes;

namespace Hypercube.Example;

public sealed class Example : IEventSubscriber, IPostInject
{
    [Dependency] private readonly IAudioManager _audioManager = default!;
    [Dependency] private readonly ICameraManager _cameraManager = default!;
    [Dependency] private readonly IEventBus _eventBus = default!;
    [Dependency] private readonly IEntitiesManager _entitiesManager = default!;
    [Dependency] private readonly IEntitiesComponentManager _entitiesComponentManager = default!;
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly IResourceContainer _resourceContainer = default!;

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
        for (var i = 0; i < 10; i++)
        {
            var x = _random.NextSingle() * 10 - 5;
            var y = _random.NextSingle() * 10 - 5;

            var coord = new SceneCoordinates(SceneId.Nullspace, new Vector2(x, y));
            CreateEntity(coord, new RectangleShape(Vector2.One * 2f));
            CreateEntity(coord, new CircleShape(1f));
        }

        CreateEntity(new SceneCoordinates(SceneId.Nullspace, new Vector2(0, 11)),
            new RectangleShape(new Vector2(40, 1)), BodyType.Static);
        CreateEntity(new SceneCoordinates(SceneId.Nullspace, new Vector2(0, -11)),
            new RectangleShape(new Vector2(40, 1)), BodyType.Static);
        CreateEntity(new SceneCoordinates(SceneId.Nullspace, new Vector2(-20, 0)),
            new RectangleShape(new Vector2(1, 21)), BodyType.Static);
        CreateEntity(new SceneCoordinates(SceneId.Nullspace, new Vector2(20, 0)),
            new RectangleShape(new Vector2(1, 21)), BodyType.Static);
        
        CreatePlayer();

        var stream = _resourceContainer.GetResource<AudioResource>("/game_boi_3.wav").Stream;
        var source = _audioManager.CreateSource(stream);
            
        // it's too loud :D
        source.Gain = 0.1f;
        source.Start();
        
        var camera = _cameraManager.CreateCamera2D(_renderer.MainWindow.Size);
        _cameraManager.SetMainCamera(camera);
    }

    private void CreateEntity(SceneCoordinates coordinates, IShape shape, BodyType type = BodyType.Dynamic)
    {
        var entityUid = _entitiesManager.Create("Fuck", coordinates);

        _entitiesComponentManager.AddComponent<PhysicsComponent>(entityUid, entity =>
        {
            entity.Component.Type = type;
            entity.Component.Shape = shape;
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

    private void CreatePlayer()
    {
        var entityUid = _entitiesManager.Create("Fuck", new SceneCoordinates(SceneId.Nullspace, new Vector2(0, 0)));
        
        _entitiesComponentManager.AddComponent<ControlsComponent>(entityUid);
        _entitiesComponentManager.AddComponent<PhysicsComponent>(entityUid, entity =>
        {
            entity.Component.Shape = new CircleShape(1f);
            entity.Component.Mass = _random.NextSingle() * 2.0f + 0.5f;
        });
        
        _entitiesComponentManager.AddComponent<SpriteComponent>(entityUid, entity =>
        {
            entity.Component.TexturePath = new ResourcePath("/Textures/icon.png");
        });
    }
}