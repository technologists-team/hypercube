namespace Hypercube.Graphics.Core.Commands.Implementations;

public struct RenderTargetCommand : IRenderCommand
{
    public RenderCommandType Type => RenderCommandType.RenderTarget;
    
    public long Handle;
}
