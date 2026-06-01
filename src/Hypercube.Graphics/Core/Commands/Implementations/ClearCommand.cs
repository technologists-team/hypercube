using Hypercube.Mathematics;

namespace Hypercube.Graphics.Core.Commands.Implementations;

public struct ClearCommand : IRenderCommand
{
    public RenderCommandType Type => RenderCommandType.Clear;
    
    public Color Color;
}
