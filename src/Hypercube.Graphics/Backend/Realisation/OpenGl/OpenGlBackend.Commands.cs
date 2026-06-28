using Hypercube.Graphics.Backend.Commands;
using Hypercube.Utilities.Commander;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    public unsafe void ExecuteCommands(IUnsafeCommandBuffer commandBuffer)
    {
        while (commandBuffer.TryGetNext(out var handle, out var data))
        {
            var type = (LowCommandType) handle;
            switch (type)
            {
                case LowCommandType.Color:
                {
                    _ = *(LowCommandClear*) data;
                    _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    break;
                }

                case LowCommandType.CullFaceMode:
                {
                    var cmd = *(LowCommandCullFace*)data;
                    _gl.CullFace(Translate(cmd.Mode));
                    break;
                }

                case LowCommandType.Scissor:
                {
                    var cmd = *(LowCommandScissor*) data;
                    if (cmd.Enabled)
                    {
                        _gl.Enable(EnableCap.ScissorTest);
                        _gl.Scissor(cmd.Box.Left, cmd.Box.Bottom, (uint) cmd.Box.Right, (uint) cmd.Box.Top);
                        break;
                    }
                    
                    _gl.Disable(EnableCap.ScissorTest);
                    break;
                }

                case LowCommandType.Viewport:
                {
                    var cmd = *(LowCommandViewport*) data;
                    _gl.Viewport(cmd.Viewport.Left, cmd.Viewport.Bottom, (uint) cmd.Viewport.Right, (uint) cmd.Viewport.Top);
                    break;
                }

                case LowCommandType.BindTexture:
                {
                    var cmd = *(LowCommandBindTexture*) data;
                    _gl.ActiveTexture(TextureUnit.Texture0 + (int) cmd.Slot);
                    _gl.BindTexture(TextureTarget.Texture2D, cmd.TextureId);
                    break;
                }

                case LowCommandType.BindShader:
                {
                    var cmd = *(LowCommandBindShader*) data;
                    _gl.UseProgram(cmd.ProgramId);
                    break;
                }
                
                case LowCommandType.Projection:
                {
                    var cmd = *(LowCommandProjection*) data;
                    _projection = cmd.Projection;
                    break;
                }

                case LowCommandType.View:
                {
                    var cmd = *(LowCommandView*) data;
                    _view = cmd.View;
                    break;
                }
                
                case LowCommandType.Primitive:
                {
                    var cmd = *(LowCommandPrimitive*) data;
                    _primitive = Translate(cmd.Type);
                    break;
                }
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}