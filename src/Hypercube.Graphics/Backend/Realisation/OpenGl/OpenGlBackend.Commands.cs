using Hypercube.Graphics.Backend.Commands;
using Hypercube.Graphics.Backend.Commands.Shader;
using Hypercube.Graphics.Backend.Commands.Texture;
using Hypercube.Graphics.Backend.Commands.Uploading;
using Hypercube.Graphics.Core.Types;
using Hypercube.Mathematics.Matrices;
using Hypercube.Utilities.Commander;
using Silk.NET.OpenGL;
using ClearBufferMask = Silk.NET.OpenGL.ClearBufferMask;
using PrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using ShaderType = Hypercube.Graphics.Resources.Shaders.Data.ShaderType;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    private const int CommandStackBuffersSize = 128;
    
    private const int NullTexture = 0;
    private const int NullShader = 0;

    #region Backend state

    private ClearBufferMask _mask;
    private PrimitiveType _primitive = PrimitiveType.Triangles;
    private Matrix4x4 _projection = Matrix4x4.Identity;
    private Matrix4x4 _view = Matrix4x4.Identity;

    #endregion
    
    public unsafe void ExecuteCommands(IUnsafeCommandBuffer commandBuffer)
    {
        var buffer = stackalloc byte[CommandStackBuffersSize];
        
        while (commandBuffer.TryGetNext(out var handle, out var data))
        {
            var type = (LowCommandType) handle;
            switch (type)
            {
                #region Clear
               
                case LowCommandType.Clear:
                {
                    _ = *(LowCommandClear*) data;
                    Gl.Clear(_mask);
                    break;
                }

                case LowCommandType.ClearSettings:
                {
                    var cmd = *(LowCommandClearSettings*) data;
                    Gl.ClearColor(cmd.Color.NormalizedR, cmd.Color.NormalizedG, cmd.Color.NormalizedB, cmd.Color.NormalizedA);
                    _mask = Translate(cmd.Mask);
                    break;
                }
                
                #endregion

                #region Render settings

                case LowCommandType.CullFaceMode:
                {
                    var cmd = *(LowCommandCullFace*)data;
                    Gl.CullFace(Translate(cmd.Mode));
                    break;
                }

                case LowCommandType.Scissor:
                {
                    var cmd = *(LowCommandScissor*) data;
                    if (cmd.Enabled)
                    {
                        Gl.Enable(EnableCap.ScissorTest);
                        Gl.Scissor(cmd.Box.Left, cmd.Box.Bottom, (uint) cmd.Box.Right, (uint) cmd.Box.Top);
                        break;
                    }
                    
                    Gl.Disable(EnableCap.ScissorTest);
                    break;
                }
                
                case LowCommandType.Viewport:
                {
                    var cmd = *(LowCommandViewport*) data;
                    Gl.Viewport(cmd.Viewport.Left, cmd.Viewport.Top, (uint) cmd.Viewport.Right, (uint) cmd.Viewport.Bottom);
                    break;
                }
                
                #endregion

                #region Texture
                
                case LowCommandType.BindTexture:
                {
                    var cmd = *(LowCommandBindTexture*) data;
                    Gl.ActiveTexture(TextureUnit.Texture0 + cmd.Slot);
                    Gl.BindTexture(TextureTarget.Texture2D, _textures[cmd.Handle]);
                    break;
                }

                case LowCommandType.UnbindTexture:
                {
                    _ = *(LowCommandUnbindTexture*) data;
                    Gl.BindTexture(TextureTarget.Texture2D, NullTexture);
                    break;
                }
                
                case LowCommandType.CreateTexture:
                {
                    var cmd = *(LowCommandCreateTexture*) data;
                    
                    var target = Translate(cmd.Type);
                    var format = Translate(cmd.Format);
                    var pixelFormat = Translate(cmd.PixelFormat);
                    
                    var texture = Gl.GenTexture();
                    _textures[cmd.Handle] = texture;
                    
                    Gl.BindTexture(target, texture);
                    Gl.TexImage2D(
                        target,
                        0,
                        format,
                        (uint) cmd.Size.X,
                        (uint) cmd.Size.Y,
                        0,
                        pixelFormat,
                        PixelType.UnsignedByte,
                        cmd.Data);
                    
                    Gl.TexParameter(target, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
                    Gl.TexParameter(target, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
                    Gl.TexParameter(target, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
                    Gl.TexParameter(target, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
                    
                    Gl.BindTexture(target, 0);
                    break;
                }

                #endregion

                #region Shader

                case LowCommandType.BindShader:
                {
                    var cmd = *(LowCommandBindShader*) data;
                    Gl.UseProgram(_shaders[cmd.Handle]);
                    break;
                }

                case LowCommandType.UnbindShader:
                {
                    _ = *(LowCommandUnbindShader*) data;
                    Gl.UseProgram(NullShader);
                    break;
                }
                
                // Yeah we do pointer hell
                case LowCommandType.CreateShader:
                {
                    var cmd = *(LowCommandCreateShader*) data;
                    var pointer = (byte*) cmd.Data;

                    var program = Gl.CreateProgram();
                    
                    var count = *pointer;
                    pointer++;
                    
                    // buffer protection
                    if (CommandStackBuffersSize - sizeof(uint) * count < 0)
                        throw new OutOfMemoryException($"The number of shaders exceeded the command buffer stack limit. Stack buffer space: {CommandStackBuffersSize}, requested amount of memory: {sizeof(uint) * count}");
   
                    var shaders = new Span<uint>(buffer, count);
                    
                    // Data specification
                    // Data:         1 byte (shader count), N shader blocks
                    // Shader block: 1 byte (shader type), 1 int (length), N byte (bytecode)
                    
                    for (var i = 0; i < count; i++)
                    {
                        // 1 byte, type header
                        var shaderType = Translate((ShaderType) (*pointer++));
                        
                        // 1 int, size header
                        var size = *(int*) pointer;
                        pointer += sizeof(int);

                        // N byte, bytecode
                        var source = pointer;
                        var length = size;
                        
                        // read end
                        // skip bytecode part
                        // we go to new shader header
                        pointer += size;
                        
                        var shader = Gl.CreateShader(shaderType);
                        
                        Gl.ShaderSource(shader, 1, &source, &length);
                        Gl.CompileShader(shader);
                        Gl.GetShader(shader, ShaderParameterName.CompileStatus, out var shaderCode);

                        if (shaderCode != 1)
                        {
                            var log = Gl.GetShaderInfoLog(shader);
                            Gl.DeleteShader(shader);
                            
                            // TODO: out error
                            Console.WriteLine(log);
                            continue;
                        }

                        // Don't save failed shader
                        shaders[i] = shader;
                        
                        Gl.AttachShader(program, shader);
                    }
                    
                    Gl.LinkProgram(program);
                    Gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out var programCode);

                    if (programCode != 1)
                    {
                        Gl.GetProgramInfoLog(program, out var log);
                        Gl.DeleteProgram(program);
                        
                        Console.WriteLine(log);
                    }
                    
                    for (var i = 0; i < count; i++)
                    {
                        var shader = shaders[i];
                        if (shader == NullShader)
                            continue;

                        Gl.DetachShader(program, shader);
                        Gl.DeleteShader(shader);
                    }

                    _shaders[cmd.Handle] = program;
                    break;
                }

                #endregion

                #region Uploading

                case LowCommandType.UploadVertices:
                {
                    var cmd = *(LowCommandUploadVertices*) data;
                    
                    _vbo.Bind();
                    _vbo.SetData(cmd.Count * Vertex.Size, (nint) cmd.Data, BufferUsageARB.StreamDraw);
                    break;
                }

                case LowCommandType.UploadIndices:
                {
                    var cmd = *(LowCommandUploadIndices*) data;
                    
                    _ebo.Bind();
                    _ebo.SetData(cmd.Count * sizeof(uint), (nint) cmd.Data, BufferUsageARB.StreamDraw);
                    break;
                }

                #endregion
                
                case LowCommandType.Draw:
                {
                    var cmd = *(LowCommandDraw*) data;
                   
                    _vao.Bind();
                    Gl.DrawElements(_primitive, (uint) (cmd.End - cmd.Start), DrawElementsType.UnsignedInt, (void*) cmd.Start);
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