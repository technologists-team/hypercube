namespace Hypercube.Graphics.Core.Commands;

public enum RenderCommandType : byte
{
    Test,
    Draw,
    Scissor,
    RenderTarget,
    Clear,
    Matrix,
    
    Count
}
