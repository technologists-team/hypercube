using System.Runtime.CompilerServices;
using Hypercube.Core.Ecs.Core;

namespace Hypercube.Core.Ecs;

public readonly struct Entity : IDisposable, IEquatable<Entity>
{
    public readonly int Id;
    public readonly World World;

    public Entity(int id, World world)
    {
        Id = id;
        World = world;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        
    }

    public bool Equals(Entity other)
    {
        return Id == other.Id && World == other.World;
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, World);
    }
    
    public override string ToString()
    {
        return $"Entity {World}:{Id}";
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Entity left, Entity right)
    {
        return left.Equals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Entity left, Entity right)
    {
        return !left.Equals(right);
    }
}