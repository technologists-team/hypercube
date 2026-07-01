using Hypercube.Graphics.Backend.Commands;
using Hypercube.Utilities.Commander;
using Silk.NET.OpenGL;

using ShaderType = Hypercube.Graphics.Resources.Shaders.Data.ShaderType;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    private const int CommandStackBuffersSize = 128;
    private const int NullShader = 0;
    
    public unsafe void ExecuteCommands(IUnsafeCommandBuffer commandBuffer)
    {
        var buffer = stackalloc byte[CommandStackBuffersSize];
        
        while (commandBuffer.TryGetNext(out var handle, out var data))
        {
            var type = (LowCommandType) handle;
            switch (type)
            {
                case LowCommandType.Clear:
                {
                    _ = *(LowCommandClear*) data;
                    _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    break;
                }

                case LowCommandType.ClearSettings:
                {
                    var cmd = *(LowCommandClearSettings*) data;
                    _gl.ClearColor(cmd.Color.NormalizedR, cmd.Color.NormalizedG, cmd.Color.NormalizedB, cmd.Color.NormalizedA);
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

                case LowCommandType.CreateTexture:
                {
                    var cmd = *(LowCommandCreateTexture*) data;
                    
                    var target = Translate(cmd.Type);
                    var format = Translate(cmd.Format);
                    var pixelFormat = Translate(cmd.PixelFormat);
                    
                    var texture = _gl.GenTexture();
                    _textures[cmd.Handle] = texture;
                    
                    _gl.BindTexture(target, texture);
                    _gl.TexImage2D(
                        TextureTarget.Texture2D,
                        0,
                        format,
                        (uint) cmd.Size.X,
                        (uint) cmd.Size.Y,
                        0,
                        pixelFormat,
                        PixelType.UnsignedByte,
                        cmd.Data);
                    
                    _gl.TexParameter(target, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Nearest);
                    _gl.TexParameter(target, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);
                    _gl.TexParameter(target, TextureParameterName.TextureWrapS, (int) TextureWrapMode.ClampToEdge);
                    _gl.TexParameter(target, TextureParameterName.TextureWrapT, (int) TextureWrapMode.ClampToEdge);
                    
                    _gl.BindTexture(target, 0);
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
                
                case LowCommandType.BindShader:
                {
                    var cmd = *(LowCommandBindShader*) data;
                    _gl.UseProgram(_shaders[cmd.Handle]);
                    break;
                }

                // Yeah we do pointer hell
                case LowCommandType.CreateShader:
                {
                    var cmd = *(LowCommandCreateShader*) data;
                    var pointer = (byte*) cmd.Data;

                    var program = _gl.CreateProgram();
                    
                    var count = *pointer;
                    pointer++;
                    
                    // buffer protection
                    if (CommandStackBuffersSize - sizeof(uint) * count < 0)
                        throw  new OutOfMemoryException($"The number of shaders exceeded the command buffer stack limit. Stack buffer space: {CommandStackBuffersSize}, requested amount of memory: {sizeof(uint) * count}");
   
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
                        
                        var shader = _gl.CreateShader(shaderType);
                        
                        _gl.ShaderSource(shader, 1, &source, &length);
                        _gl.CompileShader(shader);
                        _gl.GetShader(shader, ShaderParameterName.CompileStatus, out var shaderCode);

                        if (shaderCode != (int) ErrorCode.NoError)
                        {
                            var log = _gl.GetShaderInfoLog(shader);
                            _gl.DeleteShader(shader);
                            
                            // TODO: out error
                            Console.WriteLine(log);
                            continue;
                        }

                        // Don't save failed shader
                        shaders[i] = shader;
                        
                        _gl.AttachShader(program, shader);
                    }
                    
                    _gl.LinkProgram(program);
                    _gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out var programCode);

                    if (programCode != (int) ErrorCode.NoError)
                    {
                        _gl.GetProgramInfoLog(program, out var log);
                        _gl.DeleteProgram(program);
                        
                        Console.WriteLine(log);
                    }
                    
                    for (var i = 0; i < count; i++)
                    {
                        var shader = shaders[i];
                        if (shader == NullShader)
                            continue;

                        _gl.DetachShader(program, shader);
                        _gl.DeleteShader(shader);
                    }

                    _shaders[cmd.Handle] = program;
                    break;
                }
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}