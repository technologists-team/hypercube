using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Math.Matrices;
using Hypercube.Math.Vectors;
using Hypercube.OpenGL.Objects;
using ImGuiNET;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.ImGui.Implementations;

public partial class GlfwImGuiController
{
    public void Render()
    {
        ImGuiNET.ImGui.Render();
        Render(ImGuiNET.ImGui.GetDrawData());
    }
    
    private void Render(ImDrawDataPtr data)
    {
        if (data.CmdListsCount == 0)
            return;
        
        var frameBufferSize = new Vector2Int(
            data.DisplaySize.X * data.FramebufferScale.X,
            data.DisplaySize.Y * data.FramebufferScale.Y);
        
        if (frameBufferSize <= 0)
            return;

        GL.GetInteger(GetPName.ActiveTexture, out var previousActiveTexture);
        
        GL.ActiveTexture(TextureUnit.Texture0);

        var vao = new ArrayObject();
        SetupRender(data, frameBufferSize, vao);
        
        var clipOff = data.DisplayPos;
        var clipScale = data.FramebufferScale;
    }

    private void SetupRender(ImDrawDataPtr data, Vector2Int frameBufferSize, ArrayObject vao)
    {
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.ScissorTest);
        
        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFuncSeparate(
            BlendingFactorSrc.SrcAlpha,
            BlendingFactorDest.OneMinusSrcAlpha,
            BlendingFactorSrc.One,
            BlendingFactorDest.OneMinusSrcAlpha
        );

        GL.Disable(EnableCap.CullFace);
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.StencilTest);
        GL.Disable(EnableCap.PrimitiveRestart);

        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

        GL.Viewport(frameBufferSize);
    }
}