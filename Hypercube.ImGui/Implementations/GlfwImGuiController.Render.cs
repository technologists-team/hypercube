using System.Runtime.CompilerServices;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;
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
    
        SetupRender(data, frameBufferSize);
        
        _vao.Bind();
        _vbo.Bind();
        _ebo.Bind();
        
        CheckErrors("Buffer objects"); ;
        
        for (var n = 0; n < data.CmdListsCount; n++)
        {         
            var cmd = data.CmdLists[n];
            
            var vertexSize = cmd.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>();
            var indexSize = cmd.IdxBuffer.Size * sizeof(ushort);

            if (vertexSize > _vertexBufferSize)
            {
                var newSize = (int)System.Math.Max(_vertexBufferSize * 1.5f, vertexSize);
                
                _vertexBufferSize = newSize;
                _vbo.SetData(newSize, nint.Zero, BufferUsageHint.DynamicDraw);
            }

            if (indexSize > _indexBufferSize)
            {
                var newSize = (int)System.Math.Max(_indexBufferSize * 1.5f, indexSize);
                
                _indexBufferSize = newSize;
                _ebo.SetData(newSize, nint.Zero, BufferUsageHint.DynamicDraw);
            }
            
            CheckErrors("Setup data");
        }

        var project = Matrix4X4.CreateOrthographic(_io.DisplaySize, -1f, 1f);
        
        _shader.Use();
        _shader.SetUniform("projection", project);
        
        CheckErrors("Projection");
        
        data.ScaleClipRects(_io.DisplayFramebufferScale);
        
        // Render command lists
        for (var n = 0; n < data.CmdListsCount; n++)
        {
            var cmd = data.CmdLists[n];

            _vbo.SetSubData(cmd.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd.VtxBuffer.Data);
            _ebo.SetSubData(cmd.IdxBuffer.Size * sizeof(ushort), cmd.IdxBuffer.Data);

            CheckErrors("Setup subData");
            
            for (var i = 0; i < cmd.CmdBuffer.Size; i++)
            {
                var cmdPointer = cmd.CmdBuffer[i];
                
                if (cmdPointer.UserCallback != nint.Zero)
                    throw new NotImplementedException();

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, (int) cmdPointer.TextureId);
                
                CheckErrors("Texture");
                
                var clip = cmdPointer.ClipRect;
                GL.Scissor((int) clip.X, _window.Size.Y - (int) clip.W, (int) (clip.Z - clip.X), (int) (clip.W - clip.Y));
                
                CheckErrors("Scissor");
                
                if (_io.BackendFlags.HasFlag(ImGuiBackendFlags.RendererHasVtxOffset))
                {
                    GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)cmdPointer.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(cmdPointer.IdxOffset * sizeof(ushort)), unchecked((int)cmdPointer.VtxOffset));
                }
                else
                {
                    GL.DrawElements(BeginMode.Triangles, (int)cmdPointer.ElemCount, DrawElementsType.UnsignedShort, (int)cmdPointer.IdxOffset * sizeof(ushort));
                }
                
                CheckErrors("Draw");
            }
        }
        
        _shader.Stop();
        
        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
    }

    private void SetupRender(ImDrawDataPtr data, Vector2Int frameBufferSize)
    {
        GL.Enable(EnableCap.Blend);
        GL.Enable(EnableCap.ScissorTest);
        
        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        
        GL.Disable(EnableCap.CullFace);
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.StencilTest);
        GL.Disable(EnableCap.PrimitiveRestart);

        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

        GL.Viewport(frameBufferSize);
    }
}