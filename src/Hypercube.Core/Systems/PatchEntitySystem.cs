using Hypercube.Core.Ecs;
using Hypercube.Graphics.Patching;
using Hypercube.Graphics.Rendering.Context;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Systems;

public abstract class PatchEntitySystem : EntitySystem, IPatch, IPostInject
{
    [Dependency] protected readonly IPatchManager PatchManager = default!;
    
    public abstract void Draw(IRenderContext renderer);

    public void PostInject()
    {
        PatchManager.AddPatch(this);
    }
}