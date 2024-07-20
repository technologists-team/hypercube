using Hypercube.Math.Shapes;
using Hypercube.Math.Vectors;

namespace Hypercube.Shared.Physics;

public interface IBody
{
    BodyType Type { get; set; }
    bool IsStatic { get; }
    
    IShape Shape { get; set; }

    Vector2 Position { get; }
    Vector2 LinearVelocity { get; set; }
    
    float Angle { get; }
    float AngularVelocity { get; }
    
    Vector2 Force { get; }
    
    float Density { get; }
    float Mass { get; }
    float InvMass { get; }
    float Inertia { get; }
    float InvInertia { get; }
    float Restitution { get; }
    float Friction { get; }
    
    Vector2[] GetShapeVerticesTransformed();

    void Update(float deltaTime);
    void Move(Vector2 position);
}