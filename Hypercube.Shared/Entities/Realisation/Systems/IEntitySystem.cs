using Hypercube.Runtime.Events;

namespace Hypercube.Shared.Entities.Realisation.Systems;

public interface IEntitySystem
{
    void Initialize();
    void Shutdown(RuntimeShutdownEvent args);
    void FrameUpdate(UpdateFrameEvent args);
}