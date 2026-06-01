using System.Numerics;

namespace Hypercube.Graphics.Core.Commands.Implementations;

public struct MatrixCommand : IRenderCommand
{
    public RenderCommandType Type => RenderCommandType.Matrix;
    
    public Matrix4x4 Projection;
    public Matrix4x4 View;
}
