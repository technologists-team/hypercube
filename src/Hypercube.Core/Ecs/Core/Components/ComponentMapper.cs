using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Hypercube.Core.Analyzers;

namespace Hypercube.Core.Ecs.Core.Components;

[EngineCore]
public class ComponentMapper<TComponent> where TComponent : IComponent
{
    private const int DefaultEntity = -1;
    private const int DefaultIndex = -1;
    private const int GrowthFactor = 2;

    private TComponent[] _components = [];
    private int[] _mapping = [];

    private int _lastComponentIndex = DefaultIndex;

    public bool Empty => _lastComponentIndex == DefaultIndex;
    public int Count => _lastComponentIndex + 1;
    
    public TComponent this[int entity]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get(entity);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Set(entity, in value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Has(int entity)
    {
        return entity < _mapping.Length && _mapping[entity] != DefaultEntity;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Set(int entity, in TComponent component)
    {
        Resize(ref _mapping, entity, DefaultEntity);
        
        var isNew = true;
        ref var componentIndex = ref _mapping[entity];
        
        if (componentIndex != DefaultIndex)
        {
            Remove(entity);
            isNew = false;
        }
        
        componentIndex = ++_lastComponentIndex;

        Resize(ref _components, _lastComponentIndex);

        _components[_lastComponentIndex] = component;

        return isNew;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove(int entity)
    {
        if (entity >= _mapping.Length)
            return false;

        ref var index = ref _mapping[entity];
        if (index == DefaultIndex)
            return false;
        
        index = DefaultIndex;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref TComponent Get(int entity)
    {
        return ref _components[_mapping[entity]];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(int entity, [NotNullWhen(true)] ref TComponent? component)
    {
        if (!Has(entity))
            return false;

        component = Get(entity);
        return true;
    }

    private void Resize<TArg>(ref TArg[] array, int index, TArg? defaultValue = default)
    {
        var length = _components.Length;
        var size = Math.Max(index + 1, length * GrowthFactor);
        
        Array.Resize(ref array, size);
        
        if (defaultValue is null)
            return;
        
        Array.Fill(array, defaultValue, length, size - length);
    }
}