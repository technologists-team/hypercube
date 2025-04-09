using System.Collections.Frozen;
using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Graphics.Utilities.Extensions;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Vectors;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Rendering.Api.Realisations.OpenGl;

public sealed partial class OpenGlRenderingApi
{
    private sealed class ShaderProgram : BaseShaderProgram
    {
        private readonly GL _gl;
        private readonly FrozenDictionary<string, int> _uniformLocations;
        
        public ShaderProgram(GL gl, uint handle, IEnumerable<IShader> shaders) : base(handle)
        {
            _gl = gl;
            
            Setup(shaders as IShader[] ?? shaders.ToArray()); 
            _uniformLocations = GetUniformLocations();
        }

        private void Setup(IShader[] shaders)
        {
            if (shaders.Length == 0)
                throw new InvalidOperationException();
            
            foreach (var shader in shaders)
            {
                Attach(shader);
            }

            Link();

            foreach (var shader in shaders)
            {
                Detach(shader);
            }
        }

        private void Link()
        {
            _gl.LinkProgram(Handle);
            
            _gl.GetProgram(Handle, ProgramPropertyARB.LinkStatus, out var code);

            if (code == (int) GLEnum.True)
                return;

            _gl.GetProgramInfoLog(Handle, out var info);
            throw new Exception($"Error occurred whilst linking Program {Handle} ({info})");
        }

        private void Attach(IShader shader)
        {
            _gl.AttachShader(Handle, shader.Handle);
        }

        private void Detach(IShader shader)
        {
            _gl.DetachShader(Handle, shader.Handle);
        }

        public override void SetUniform(string name, int value)
        {
            _gl.Uniform1(_uniformLocations[name], value);
        }

        public override void SetUniform(string name, float value)
        {
            _gl.Uniform1(_uniformLocations[name], value);
        }

        public override void SetUniform(string name, double value)
        {
            _gl.Uniform1(_uniformLocations[name], value);
        }

        public override void SetUniform(string name, Vector2 value)
        {
            _gl.Uniform2(_uniformLocations[name], value.X, value.Y);
        }

        public override void SetUniform(string name, Vector2i value)
        {
            _gl.Uniform2(_uniformLocations[name], value.X, value.Y);
        }

        public override void SetUniform(string name, Vector3 value)
        {
            _gl.Uniform3(_uniformLocations[name], value.X, value.Y, value.Z);
        }

        public override void SetUniform(string name, Vector3i value)
        {
            _gl.Uniform3(_uniformLocations[name], value.X, value.Y, value.Z);
        }

        public override void SetUniform(string name, Vector4 value)
        {
            _gl.Uniform4(_uniformLocations[name], value.X, value.Y, value.Z, value.W);
        }

        public override unsafe void SetUniform(string name, Matrix3x3 value, bool transpose = false)
        {
            var matrix = new Matrix3x3(value);
            _gl.UniformMatrix3(_uniformLocations[name], 1, transpose, (float*) &matrix);
        }

        public override unsafe void SetUniform(string name, Matrix4x4 value, bool transpose = false)
        {
            var matrix = new Matrix4x4(value);
            _gl.UniformMatrix4(_uniformLocations[name], 1, transpose, (float*) &matrix);
        }

        public override void SetUniform(string name, Color value)
        {
            _gl.Uniform4(_uniformLocations[name], value.R, value.G, value.B, value.A);
        }

        protected override void InternalUseProgram(uint handle)
        {
            _gl.UseProgram(handle);
        }

        protected override void InternalLabel(string name)
        {
            _gl.ObjectLabel(ObjectIdentifier.Program, Handle, name);
        }

        protected override void InternalDelete(uint handle)
        {
            _gl.DeleteProgram(handle);
        }

        private FrozenDictionary<string, int> GetUniformLocations()
        {
            _gl.GetProgram(Handle, ProgramPropertyARB.ActiveUniforms, out var uniforms);
       
            var uniformLocations = new Dictionary<string, int>();
            for (var i = 0u; i < uniforms; i++)
            {
                var key = _gl.GetActiveUniform(Handle, i, out _, out _);
                var location = _gl.GetUniformLocation(Handle, key);
                uniformLocations.Add(key, location);
            }

            return uniformLocations.ToFrozenDictionary();
        }
    }
}