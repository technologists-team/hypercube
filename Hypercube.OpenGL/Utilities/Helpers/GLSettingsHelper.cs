using JetBrains.Annotations;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.OpenGL.Utilities.Helpers;

[PublicAPI]
public static class GLSettingsHelper
{
    public static bool Blend
    {
        get => GL.GetBoolean(GetPName.Blend);
        set => GLHelper.SetCap(EnableCap.Blend, value);
    }

    public static bool CullFace
    {
        get => GL.GetBoolean(GetPName.CullFace);
        set => GLHelper.SetCap(EnableCap.CullFace, value);
    }

    public static bool DepthTest
    {
        get => GL.GetBoolean(GetPName.DepthTest);
        set => GLHelper.SetCap(EnableCap.DepthTest, value);
    }
    
    public static bool ScissorTest
    {
        get => GL.GetBoolean(GetPName.ScissorTest);
        set => GLHelper.SetCap(EnableCap.ScissorTest, value);
    }
    
    public static bool StencilTest
    {
        get => GL.GetBoolean(GetPName.StencilTest);
        set => GLHelper.SetCap(EnableCap.StencilTest, value);
    }

    public static bool DebugOutput
    {
        set => GLHelper.SetCap(EnableCap.DebugOutput, value);
    }
    
    public static bool DebugOutputSynchronous
    {
        set => GLHelper.SetCap(EnableCap.DebugOutputSynchronous, value);
    }
    
    public static bool PrimitiveRestart
    {
        set => GLHelper.SetCap(EnableCap.PrimitiveRestart, value);
    }
}