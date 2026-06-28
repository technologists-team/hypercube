using Hypercube.Mathematics.Shapes;

namespace Hypercube.Graphics.Backend.Commands;

public struct LowCommandScissor
{
    public bool Enabled;
    public Rect2i Box;
}
