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

public readonly struct Entity<T1, T2>(EntityUid owner, T1 component1, T2 component2)
    where T1 : IComponent? where T2 : IComponent?
{
    public readonly EntityUid Owner = owner;
    public readonly T1 Component1 = component1;
    public readonly T2 Component2 = component2;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1?, T2?>(EntityUid entityUid)
    {
        return new Entity<T1?, T2?>(entityUid, default, default);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1>(Entity<T1, T2> entity)
    {
        return new Entity<T1>(entity.Owner, entity.Component1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (EntityUid entityUid, T1 component, T2 component2)(Entity<T1, T2> entity)
    {
        return (entity.Owner, entity.Component1, entity.Component2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1, T2>((EntityUid entityUid, T1 component, T2 component2) tuple)
    {
        return new Entity<T1, T2>(tuple.entityUid, tuple.component, tuple.component2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator EntityUid(Entity<T1, T2> entity)
    {
        return entity.Owner;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator T1(Entity<T1, T2> entity)
    {
        return entity.Component1;
    }
    
    public override int GetHashCode()
    {
        return Owner.GetHashCode();
    }
}

public readonly struct Entity<T1, T2, T3>(EntityUid owner, T1 component1, T2 component2, T3 component3)
    where T1 : IComponent? where T2 : IComponent? where T3 : IComponent?
{
    public readonly EntityUid Owner = owner;
    public readonly T1 Component1 = component1;
    public readonly T2 Component2 = component2;
    public readonly T3 Component3 = component3;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1?, T2?, T3?>(EntityUid entityUid)
    {
        return new Entity<T1?, T2?, T3?>(entityUid, default, default, default);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1>(Entity<T1, T2, T3> entity)
    {
        return new Entity<T1>(entity.Owner, entity.Component1);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1, T2>(Entity<T1, T2, T3> entity)
    {
        return new Entity<T1, T2>(entity.Owner, entity.Component1, entity.Component2);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator (EntityUid entityUid, T1 component, T2 component2, T3 component3)(Entity<T1, T2, T3> entity)
    {
        return (entity.Owner, entity.Component1, entity.Component2, entity.Component3);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Entity<T1, T2, T3>((EntityUid entityUid, T1 component, T2 component2, T3 component3) tuple)
    {
        return new Entity<T1, T2, T3>(tuple.entityUid, tuple.component, tuple.component2, tuple.component3);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator EntityUid(Entity<T1, T2, T3> entity)
    {
        return entity.Owner;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator T1(Entity<T1, T2, T3> entity)
    {
        return entity.Component1;
    }
    
    public override int GetHashCode()
    {
        return Owner.GetHashCode();
    }
}