namespace Hypercube.Shared.Entities;

public readonly struct EntityUid(int id)
{
    public static readonly EntityUid Invalid = new(-1);
    
    public readonly int Id = id;
    
    public bool Equals(EntityUid entityUid)
    {
        return Id == entityUid.Id;
    }

    public override bool Equals(object? @object)
    {
        return @object is EntityUid id && Equals(id);
    }
    
    public override string ToString()
    {
        return $"Entity({Id})";
    }
    
    public static bool operator ==(EntityUid a, EntityUid b)
    {
        return a.Id == b.Id;
    }
    
    public static bool operator !=(EntityUid a, EntityUid b)
    {
        return a.Id != b.Id;
    }
    
    public static implicit operator int(EntityUid entityUid)
    {
        return entityUid.Id;
    }
    
    public static implicit operator EntityUid(int value)
    {
        return new EntityUid(value);
    }
}