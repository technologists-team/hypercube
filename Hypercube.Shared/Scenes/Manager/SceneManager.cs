using System.Diagnostics.CodeAnalysis;
using Hypercube.Dependencies;
using Hypercube.EventBus;
using Hypercube.Shared.Scenes.Events;

namespace Hypercube.Shared.Scenes.Manager;

public sealed class SceneManager : ISceneManager
{
    [Dependency] private readonly IEventBus _eventBus = default!;

    private readonly Dictionary<SceneId, Scene> _scenes = new();
    
    private SceneId NewSceneId => new(_nextSceneId++);

    private int _nextSceneId;
    
    public SceneId Create()
    {
        var id = NewSceneId;
        var scene = new Scene(id);
        
        _scenes.Add(id, scene);
        _eventBus.Raise(new SceneAdded(scene));
        
        return id;
    }

    public void Delete(SceneId sceneId)
    {
        var scene = _scenes[sceneId];
        _scenes.Remove(sceneId);
        _eventBus.Raise(new SceneDeleted(scene));
    }
    
    public bool HasScene(SceneId sceneId)
    {
        return _scenes.ContainsKey(sceneId);
    }
    
    public bool TryGetScene(SceneId sceneId, [NotNullWhen(true)] out Scene? scene)
    {
        scene = null;
        return _scenes.TryGetValue(sceneId, out scene);
    }
    
    public Scene GetScene(SceneId sceneId)
    {
        return _scenes[sceneId];
    }
}