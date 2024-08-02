using JetBrains.Annotations;

namespace Hypercube.Shared.Entities.Realisation;

public readonly struct EntityUid(int id)
{
    /// <summary>
    /// All Ids equals or less than -1 are Invalid.
    /// </summary>
    [PublicAPI] public static readonly EntityUid Invalid = new(-1);

    public bool Valid => Id > Invalid.Id;

    [PublicAPI] public readonly int Id = id;

    private bool Equals(EntityUid entityUid)
    {
        return Id == entityUid.Id;
    }
    
    public override int GetHashCode()
    {
        return Id;
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