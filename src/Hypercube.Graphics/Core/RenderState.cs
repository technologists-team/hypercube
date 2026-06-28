using Hypercube.Graphics.Core.Types;
using Hypercube.Graphics.Resources.Shaders;
using Hypercube.Graphics.Resources.Textures;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Shapes;

namespace Hypercube.Graphics.Core;

public sealed class RenderState
{
    // Resources
    public TextureId Texture;
    public ShaderId Shader;

    // Conveyor settings
    public BlendMode BlendMode;
    public CullFaceMode CullFaceMode;
    
    // Scissor
    public bool Scissor;
    public Rect2i ScissorBox;
    
    // Matrices
    public Matrix4x4 Projection;
    public Matrix4x4 View;
}
