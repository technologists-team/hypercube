using Hypercube.Graphics.Core.Types;

namespace Hypercube.Graphics.Backend.Commands.Uploading;

public unsafe struct LowCommandUploadVertices
{
    public Vertex* Data;
    public int Count;
}