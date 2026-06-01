using Hypercube.Mathematics.Shapes;

namespace Hypercube.Graphics.Core.Commands.Implementations;

public struct ScissorCommand : IRenderCommand
{
    public RenderCommandType Type => RenderCommandType.Scissor;

    public bool Enabled;
    public Rect2i Scissor;
}