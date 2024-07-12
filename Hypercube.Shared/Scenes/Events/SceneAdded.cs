using Hypercube.Shared.EventBus.Events.Events;

namespace Hypercube.Shared.Scenes.Events;

public readonly record struct SceneAdded(Scene Scene) : IEventArgs;