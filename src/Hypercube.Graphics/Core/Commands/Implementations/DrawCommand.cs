namespace Hypercube.Graphics.Core.Commands.Implementations;

public struct DrawCommand : IRenderCommand
{
    public RenderCommandType Type => RenderCommandType.Draw;
    
    public uint TextureId;
    public int StartIndex;
    public int Count;
    public bool Indexed;
    public PrimitiveTopology PrimitiveType;
}
