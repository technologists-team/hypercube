using Hypercube.Graphics.Core.Types;
using Hypercube.Mathematics;

namespace Hypercube.Graphics.Backend.Commands;

public struct LowCommandClearSettings
{
    public Color Color;
    public ClearBufferMask Mask;
}