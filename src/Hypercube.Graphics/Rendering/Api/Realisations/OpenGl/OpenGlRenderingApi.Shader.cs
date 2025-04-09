using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Graphics.Rendering.Shaders.Exceptions;
using Silk.NET.OpenGL;
using ShaderType = Hypercube.Graphics.Rendering.Shaders.ShaderType;

namespace Hypercube.Graphics.Rendering.Api.Realisations.OpenGl;

public sealed partial class OpenGlRenderingApi
{
    private sealed class GlShader : BaseShader
    {
        private readonly GL _gl;
        
        public GlShader(GL gl, uint handle, ShaderType type, string source) : base(handle, type)
        {
            _gl = gl;

            _gl.ShaderSource(Handle, source);
            _gl.CompileShader(Handle);

            _gl.GetShader(Handle, ShaderParameterName.CompileStatus, out var code);
            if (code == (int) GLEnum.True)
                return;

            var info = _gl.GetShaderInfoLog(Handle);
            throw new ShaderCompilationException(Handle, info);
        }

        protected override void InternalDelete()
        {
            _gl.DeleteShader(Handle);
        }
    }
}