using Hypercube.Graphics.Core.Commands.Implementations;
using Hypercube.Graphics.Core.Context;

namespace Hypercube.Graphics.Core.Backends;

public interface IRenderBackend
{
    void Initialize(IGraphicsContext context);
    
    void Execute(in TestCommand cmd);
    void Execute(in ScissorCommand cmd);
    void Execute(in ClearCommand cmd);
}