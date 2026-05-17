using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Hypercube.Core.Graphics.Rendering.Batching;
using Hypercube.Core.Windowing.Windows;
using Hypercube.Mathematics;
using Silk.NET.OpenGL;
 
using ShaderType = Hypercube.Core.Graphics.Rendering.Shaders.ShaderType;
using SilkShaderType = Silk.NET.OpenGL.ShaderType;

namespace Hypercube.Core.Graphics.Utilities.Extensions;

[EngineInternal]
public static class SilkGLExtension
{
    extension(GL gl)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TexStorage2D(TextureTarget target, uint levels, SizedInternalFormat internalFormat, int width, int height)
        {
            gl.TexStorage2D(target, levels, internalFormat, (uint) width, (uint) height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnbindTexture(TextureTarget target)
        {
            gl.BindTexture(TextureTarget.Texture2D, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBlend(bool value)
        {
            gl.SetEnableCap(EnableCap.Blend, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetScissor(bool value)
        {
            gl.SetEnableCap(EnableCap.ScissorTest, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Viewport(IWindow window)
        {
            gl.Viewport(window.Size);
        }

        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Viewport(Vector2i size)
        {
            gl.Viewport(0, 0, (uint) size.X, (uint) size.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint CreateShader(ShaderType type)
        {
            return gl.CreateShader(ToShaderType(type));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe string GetStringExt(StringName name)
        {
            var pointer = gl.GetString(name);
            return Marshal.PtrToStringUTF8((nint) pointer) ?? string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ObjectLabel(ObjectIdentifier identifier, uint handle, string name)
        {
            gl.ObjectLabel(identifier, handle, (uint) name.Length, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasErrors()
        {
            return gl.GetError() != (int) ErrorCode.NoError;
        }

        public string HasErrors(string title)
        {
            var error = gl.GetError();
            var result = string.Empty;
        
            while (error != (int) ErrorCode.NoError)
            {
                result += $"{title}: {error}{Environment.NewLine}";
                error = gl.GetError();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearColor(Color color)
        {
            gl.ClearColor(color.R, color.G, color.B, color.A);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void DrawElements(PrimitiveTopology topology, int count, DrawElementsType type, int indices)
        {
            var pointer = (void*) indices;
            gl.DrawElements(ToPrimitiveType(topology), (uint) count, type, pointer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetEnableCap(EnableCap cap, bool value)
        {
            if (value)
            {
                gl.Enable(cap);
                return;
            }
        
            gl.Disable(cap);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PrimitiveType ToPrimitiveType(PrimitiveTopology primitiveTopology)
    {
        return primitiveTopology switch
        {
            PrimitiveTopology.PointList => PrimitiveType.Points,
            PrimitiveTopology.LineList => PrimitiveType.Lines,
            PrimitiveTopology.LineStrip => PrimitiveType.LineStrip,
            PrimitiveTopology.LineLoop => PrimitiveType.LineLoop,
            PrimitiveTopology.TriangleList => PrimitiveType.Triangles,
            PrimitiveTopology.TriangleFan => PrimitiveType.TriangleFan,
            PrimitiveTopology.TriangleStrip => PrimitiveType.TriangleStrip,
            _ => throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, null)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SilkShaderType ToShaderType(ShaderType type)
    {
        return type switch
        {
            ShaderType.Vertex => SilkShaderType.VertexShader,
            ShaderType.Fragment => SilkShaderType.FragmentShader,
            ShaderType.Geometry => SilkShaderType.GeometryShader,
            ShaderType.Compute => SilkShaderType.ComputeShader,
            ShaderType.Tessellation => SilkShaderType.TessEvaluationShader,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
