using Hypercube.Client.Graphics.Viewports;
using Hypercube.Shared.Entities.Realisation.Components;

namespace Hypercube.Example.Camera;

public sealed class CameraComponent : Component
{
    public ICamera Camera = default!;
}