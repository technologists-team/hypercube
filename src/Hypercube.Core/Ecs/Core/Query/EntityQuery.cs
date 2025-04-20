using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Hypercube.Core.Ecs.Core.Components;

namespace Hypercube.Core.Ecs.Core.Query;

[EngineInternal]
public sealed class EntityQuery : IDisposable
{
    private const int FirstEntity = 0;
    
    private readonly World _world;
    private readonly ComponentQueryData[] _requiredComponents;
    private readonly ComponentQueryData[] _excludedComponents;
    
    private List<int> _matchingEntities = [];
    
    public Enumerator GetEnumerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(this);
    }
    
    private IComponentMapper PrimaryMapper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _requiredComponents[0].Mapper;
    }

    public EntityQuery(World world, ComponentQueryData[] requiredComponents, ComponentQueryData[] excludedComponents)
    {
        if (requiredComponents.Length == 0)
            throw new ArgumentException("At least one required component must be specified", nameof(requiredComponents));
        
        _world = world;
        _requiredComponents = requiredComponents;
        _excludedComponents = excludedComponents;
        
        Subscribe();
        
        foreach (var queryData in _requiredComponents)
        {
            queryData.Mapper.Added += OnChange;
            queryData.Mapper.Removed += OnChange;
        }
        
        if (_requiredComponents.Length == 1 && _excludedComponents.Length == 0)
        {
            _matchingEntities = PrimaryMapper.Entities.ToList();
            return;
        }
        
        foreach (var entity in PrimaryMapper.Entities)
        {
            var isValid = true;

            foreach (var queryData in _requiredComponents)
            {
                if (queryData.Mapper.Has(entity))
                    continue;
                
                isValid = false;
                break;
            }
            
            if (!isValid)
                continue;

            foreach (var queryData in _excludedComponents)
            {
                if (!queryData.Mapper.Has(entity))
                    continue;
                
                isValid = false;
                break;
            }

            if (isValid)
                _matchingEntities.Add(entity);
        }
    }

    public void Dispose()
    {
        Unsubscribe();
        _matchingEntities = [];
    }

    private void OnChange(int entity)
    {
        var has = _matchingEntities.Contains(entity);
        
        foreach (var queryData in _requiredComponents)
        {
            if (queryData.Mapper.Has(entity))
                continue;
            
            if (!has)
                return;
            
            _matchingEntities.Remove(entity);
            return;
        }

        foreach (var queryData in _excludedComponents)
        {
            if (!queryData.Mapper.Has(entity))
                continue;
            
            if (!has)
                return;
            
            _matchingEntities.Remove(entity);
            return;
        }
        
        _matchingEntities.Add(entity);
    }

    private void Subscribe()
    {
        foreach (var queryData in _requiredComponents)
        {
            queryData.Mapper.Added += OnChange;
            queryData.Mapper.Removed += OnChange;
        }
        
        foreach (var queryData in _excludedComponents)
        {
            queryData.Mapper.Added += OnChange;
            queryData.Mapper.Removed += OnChange;
        }
    }

    private void Unsubscribe()
    {
        foreach (var queryData in _requiredComponents)
        {
            queryData.Mapper.Added -= OnChange;
            queryData.Mapper.Removed -= OnChange;
        }
        
        foreach (var queryData in _excludedComponents)
        {
            queryData.Mapper.Added -= OnChange;
            queryData.Mapper.Removed -= OnChange;
        }
    }

    public struct Enumerator : IEnumerator<Entity>
    {
        private readonly EntityQuery _query;
        private readonly List<int> _entities;
        
        private int _index;

        object IEnumerator.Current => Current;

        public Entity Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(_entities[_index], _query._world);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator(EntityQuery query)
        {
            _query = query;
            _entities = query._matchingEntities;
            _index = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return ++_index < _entities.Count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext(out Entity entity)
        {
            entity = default;
            
            var result = ++_index < _entities.Count;
            if (!result)
                return false;
            
            entity = Current;
            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reset()
        {
            _index = 0;
        }

        public void Dispose()
        {

        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Enumerator(EntityQuery query)
    {
        return query.GetEnumerator;
    }
}