using Hypercube.Mathematics.Shapes;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Shared.Physics;

public interface IBody
{
    BodyType Type { get; set; }
    bool IsStatic { get; }
    
    IShape Shape { get; set; }

    Vector2 Position { get; }
    Vector2 LinearVelocity { get; set; }
    Vector2 LinearDamping { get; set; }
    
    float Angle { get; }
    float AngularVelocity { get; }
    float AngularDamping { get; }
    
    Vector2 Force { get; }
    
    float Density { get; }
    float Mass { get; }
    float InvMass { get; }
    float Inertia { get; }
    float InvInertia { get; }
    float Restitution { get; }
    float Friction { get; }
    
    Vector2[] GetShapeVerticesTransformed();
    Box2 ComputeAABB();

    void Update(float deltaTime, Vector2 gravity);
    void Move(Vector2 position);
}