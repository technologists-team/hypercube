namespace Hypercube.Graphics.Core.Context;

public interface IGraphicsContext
{
    IContextInfoProvider Info { get; }
    IContextResourceProvider Resource { get; }
    
    IContextListener Listener { get; }
}
