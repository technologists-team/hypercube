using Hypercube.Core.Ecs.Attributes;
using Hypercube.Graphics.Rendering.Context;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Shapes;

namespace Hypercube.Core.Systems.Rendering;

[RegisterEntitySystem]
public sealed class SpriteSystem : PatchEntitySystem
{
    public override void Draw(IRenderContext renderer)
    {
        renderer.DrawRectangle(new Box2(1, 2, 2, 1), Color.Red);
    }
}