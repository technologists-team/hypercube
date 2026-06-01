namespace Hypercube.Graphics.Core.Commands.Implementations;

public struct TestCommand : IRenderCommand
{
    public RenderCommandType Type => RenderCommandType.Test;
}
