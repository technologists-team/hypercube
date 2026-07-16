using System.Runtime.CompilerServices;
using Hypercube.Graphics.Core.Types;
using Hypercube.Graphics.Resources.Shaders;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Shapes;

namespace Hypercube.Graphics.Core.Batching;

public struct RenderState : IEquatable<RenderState>
{
    // Resources
    public TextureId Texture;
    public ShaderId Shader;

    // Conveyor settings
    public BlendMode BlendMode;
    public CullFaceMode CullFaceMode;
    public PrimitiveType PrimitiveType;
    
    // Scissor
    public bool Scissor;
    public Rect2i ScissorBox;
    
    // Matrices
    public Matrix4x4 Projection;
    public Matrix4x4 View;

    public bool Equals(RenderState other) =>
           Texture      == other.Texture
        && Shader       == other.Shader
        && BlendMode    == other.BlendMode
        && CullFaceMode == other.CullFaceMode
        && Scissor      == other.Scissor
        && ScissorBox   == other.ScissorBox
        && Projection   == other.Projection
        && View         == other.View;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is RenderState other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(
        Texture,
        Shader,
        (byte) BlendMode,
        (byte) CullFaceMode,
        Scissor,
        ScissorBox,
        Projection,
        View
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(RenderState left, RenderState right) => left.Equals(right);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(RenderState left, RenderState right) => !(left == right);
}
