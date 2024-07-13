using Hypercube.Shared.EventBus.Events;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing.Events;

public record struct TexturesPreloadEvent(HashSet<ResourcePath> Textures) : IEventArgs;