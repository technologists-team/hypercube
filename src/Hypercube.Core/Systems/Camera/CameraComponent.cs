using Hypercube.Core.Viewports;
using Hypercube.Ecs.Components;

namespace Hypercube.Core.Systems.Camera;

public struct CameraComponent() : IComponent
{
    public ICamera? LinkedCamera;
    
    public Vector2i Size;
    public float ZFar;
    public float ZNear;
}
