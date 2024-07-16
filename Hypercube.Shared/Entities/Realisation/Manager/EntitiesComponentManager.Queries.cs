using Hypercube.Shared.Entities.Realisation.Components;

namespace Hypercube.Shared.Entities.Realisation.Manager;

public sealed partial class EntitiesComponentManager
{
    public IEnumerable<Entity<T>> GetEntities<T>() where T : IComponent
    {
        foreach (var (entityUid, component) in _entitiesComponents[typeof(T)])
        {
            yield return new Entity<T>(entityUid, (T)component);
        }
    }

    public IEnumerable<Entity<T1, T2>> GetEntities<T1, T2>() where T1 : IComponent where T2 : IComponent
    {
        var compSet1 = _entitiesComponents[typeof(T1)];
        var compSet2 = _entitiesComponents[typeof(T2)];

        foreach (var ent in compSet1)
        {
            var uid = ent.Key;
            var comp = ent.Value;
            
            if (!uid.Valid)
                continue;
            
            if (!compSet2.TryGetValue(uid, out var comp2))
                continue;

            yield return new Entity<T1, T2>(uid, (T1)comp, (T2)comp2);
        }
    }
    
    public IEnumerable<Entity<T1, T2, T3>> GetEntities<T1, T2, T3>() where T1 : IComponent where T2 : IComponent where T3 : IComponent
    {
        var compSet1 = _entitiesComponents[typeof(T1)];
        var compSet2 = _entitiesComponents[typeof(T2)];
        var compSet3 = _entitiesComponents[typeof(T3)];

        foreach (var ent in compSet1)
        {
            var uid = ent.Key;
            var comp = ent.Value;
            
            if (!uid.Valid)
                continue;
            
            if (!compSet2.TryGetValue(uid, out var comp2))
                continue;
            
            if (!compSet3.TryGetValue(uid, out var comp3))
                continue;

            yield return new Entity<T1, T2, T3>(uid, (T1)comp, (T2)comp2, (T3)comp3);
        }
    }
}