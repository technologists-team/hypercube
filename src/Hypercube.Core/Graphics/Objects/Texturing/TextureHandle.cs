using Hypercube.Utilities.Attributes;

namespace Hypercube.Core.Graphics.Objects.Texturing;

/// <summary>
/// Represents a strongly-typed handle to a texture resource.
/// </summary>
[IdStruct(typeof(uint?))]
public readonly partial struct TextureHandle
{
    /// <summary>
    /// Represents a null or uninitialized texture handle.
    /// </summary>
    public static readonly TextureHandle Zero = new(0);
    
    /// <summary>
    /// Gets a value indicating whether this handle contains a valid underlying value.
    /// </summary>
    public bool HasValue => Value.HasValue;
}
