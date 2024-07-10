using System.Runtime.CompilerServices;
using Hypercube.Shared.Entities.Realisation.Components;

namespace Hypercube.Shared.Entities.Realisation;

public readonly struct Entity<T>(EntityUid owner, T component) where T : IComponent?
{
    public readonly EntityUid Owner = owner;
    public readonly T Component = component;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T?>(EntityUid entityUid)
    {
        return new Entity<T?>(entityUid, default);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (EntityUid entityUid, T component)(Entity<T> entity)
    {
        return (entity.Owner, entity.Component);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T>((EntityUid entityUid, T component) tuple)
    {
        return new Entity<T>(tuple.entityUid, tuple.component);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator EntityUid(Entity<T> entity)
    {
        return entity.Owner;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator T(Entity<T> entity)
    {
        return entity.Component;
    }
    
    public override int GetHashCode()
    {
        return Owner.GetHashCode();
    }
}