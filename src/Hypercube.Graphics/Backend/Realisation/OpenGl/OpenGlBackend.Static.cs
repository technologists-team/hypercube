using System.Diagnostics;
using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    static OpenGlBackend()
    {
        Debug.Assert(GetVertexStride(Constants.ShaderAttribLocations) == Vertex.Size);
    }
}
