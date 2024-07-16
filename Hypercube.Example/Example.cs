using Hypercube.Client.Audio;
using Hypercube.Client.Audio.Resources;
using Hypercube.Client.Entities.Systems.Sprite;
using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Math;
using Hypercube.Math.Boxs;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.Realisation.Manager;
using Hypercube.Shared.Entities.Systems.Transform.Coordinates;
using Hypercube.Shared.EventBus;
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
        _eventBus.Subscribe<RenderDrawingEvent>(this, OnRenderDrawing);
    }

    private void OnRenderDrawing(ref RenderDrawingEvent ev)
    {
        //_renderer.DrawRectangle(new Box2(100, 100, 0, 0), Color.White);
        //_renderer.DrawLine(new Box2(100, 100, 0, 0), Color.Red);
        _renderer.DrawPoint(new Vector2(100, 100), Color.Green);
        _renderer.DrawPoint(new Vector2(0, 0), Color.Green);
    }

    private void Startup(ref RuntimeStartupEvent args)
    {
        for (var i = 0; i < 100; i++)
        {
            var x = _random.NextSingle() * 800 - 400;
            var y = _random.NextSingle() * 800 - 400;

            var coord = new SceneCoordinates(SceneId.Nullspace, new Vector2(x, y));
            CreateEntity(coord);
        }

        var stream = _resourceCacher.GetResource<AudioResource>("/game_boi_3.wav").Stream;
        var source = _audioManager.CreateSource(stream);
            
        // it's too loud :D
        source.Gain = 0.1f;
        source.Start();
        // var source = _audioManager.CreateSource("/game_boi_3.wav", new AudioSettings());
        // source.Start();
    }

    private void CreateEntity(SceneCoordinates coordinates)
    {
        var entityUid = _entitiesManager.Create("Fuck", coordinates);
        
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