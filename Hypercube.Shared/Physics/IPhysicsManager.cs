namespace Hypercube.Shared.Physics;

public interface IPhysicsManager
{
    void AddBody(IBody body);
    void RemoveBody(IBody body);
}