using Hypercube.Shared.EventBus.Events;

namespace Hypercube.Shared.Scenes.Events;

public readonly record struct SceneAdded(Scene Scene) : IEventArgs;