using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Shared.Entities.Realisation.Systems;

public interface IEntitySystem
{
    void Initialize();
    void Shutdown(RuntimeShutdownEvent args);
    void FrameUpdate(UpdateFrameEvent args);
}