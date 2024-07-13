using Hypercube.Shared.Entities.Systems.Transform.Coordinates;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public interface IEntitiesManager
{
    EntityUid Create(string name = "New Entity");
    EntityUid Create(string name, SceneCoordinates coordinates);
    void Delete(EntityUid entityUid);
}