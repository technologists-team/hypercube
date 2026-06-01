using Hypercube.Graphics.Core.Backends;

namespace Hypercube.Graphics;

public class Test
{
    public static void Test2()
    {
        var ctx = GraphicContext.Create(RenderBackend.Auto);
        var device = ctx.CreateDevice(GraphicContext.NullInfo);
    }
}