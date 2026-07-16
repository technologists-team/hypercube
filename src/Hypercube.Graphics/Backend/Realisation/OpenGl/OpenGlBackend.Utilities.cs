using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Types;
using Hypercube.Graphics.Resources.Shaders.Data;
using Hypercube.Graphics.Resources.Textures.Types;

// Silk redefine for reduce type/namespace collisions
using SilkVertexAttribPointerType = Silk.NET.OpenGL.VertexAttribPointerType;
using SilkPrimitiveType = Silk.NET.OpenGL.PrimitiveType;
using SilkTextureTarget = Silk.NET.OpenGL.TextureTarget;
using SilkInternalFormat = Silk.NET.OpenGL.InternalFormat;
using SilkPixelFormat = Silk.NET.OpenGL.PixelFormat;
using SilkGLEnum = Silk.NET.OpenGL.GLEnum;
using SilkShaderType = Silk.NET.OpenGL.ShaderType;
using SilkClearBufferMask = Silk.NET.OpenGL.ClearBufferMask;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
    private static SilkClearBufferMask Translate(ClearBufferMask mask)
    {
        var result = SilkClearBufferMask.None;
        
        if ((mask & ClearBufferMask.DepthBuffer) != 0)
            result |= SilkClearBufferMask.DepthBufferBit;
        
        if ((mask & ClearBufferMask.StencilBuffer) != 0)
            result |= SilkClearBufferMask.StencilBufferBit;

        if ((mask & ClearBufferMask.ColorBuffer) != 0)
            result |= SilkClearBufferMask.ColorBufferBit;

        if ((mask & ClearBufferMask.CoverageBufferNv) != 0)
            result |= SilkClearBufferMask.CoverageBufferBitNV;

        return result;
    }
    
    private static SilkShaderType Translate(ShaderType type)
    {
        return type switch
        {
            ShaderType.Vertex       => SilkShaderType.VertexShader,
            ShaderType.Fragment     => SilkShaderType.FragmentShader,
            ShaderType.Geometry     => SilkShaderType.GeometryShader,
            ShaderType.Compute      => SilkShaderType.ComputeShader,
            ShaderType.Tessellation => SilkShaderType.TessControlShader,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    private static SilkPixelFormat Translate(TexturePixelFormat type)
    {
        return type switch
        {
            TexturePixelFormat.R    => SilkPixelFormat.Red,
            TexturePixelFormat.Rg   => SilkPixelFormat.RG,
            TexturePixelFormat.Rgb  => SilkPixelFormat.Rgb,
            TexturePixelFormat.Rgba => SilkPixelFormat.Rgba,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    private static SilkInternalFormat Translate(TextureFormat type)
    {
        return type switch
        {
            TextureFormat.R8      => SilkInternalFormat.R8,
            TextureFormat.Rg8     => SilkInternalFormat.Rgb,
            TextureFormat.Rgb8    => SilkInternalFormat.Rgb8,
            TextureFormat.Rgba8   => SilkInternalFormat.Rgba8,
            TextureFormat.R16F    => SilkInternalFormat.R16f,
            TextureFormat.Rgba16F => SilkInternalFormat.Rgba16f,
            TextureFormat.Rgba32F => SilkInternalFormat.Rgba32f,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    private static SilkTextureTarget Translate(TextureType type)
    {
        return type switch
        {
            TextureType.Texture1d      => SilkTextureTarget.Texture1D,
            TextureType.Texture2d      => SilkTextureTarget.Texture2D,
            TextureType.Texture2dArray => SilkTextureTarget.Texture2DArray,
            TextureType.Texture3d      => SilkTextureTarget.Texture3D,
            TextureType.CubeMap        => SilkTextureTarget.TextureCubeMap,
            TextureType.CubeMapArray   => SilkTextureTarget.TextureCubeMapArray,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    private static SilkPrimitiveType Translate(PrimitiveType type)
    {
        return type switch
        {
            PrimitiveType.Triangles     => SilkPrimitiveType.Triangles,
            PrimitiveType.TriangleStrip => SilkPrimitiveType.TriangleStrip,
            PrimitiveType.Lines         => SilkPrimitiveType.Lines,
            PrimitiveType.LineStrip     => SilkPrimitiveType.LineStrip,
            PrimitiveType.Points        => SilkPrimitiveType.Points,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    private static SilkGLEnum Translate(CullFaceMode mode)
    {
        return mode switch
        {
            CullFaceMode.Front        => SilkGLEnum.Front,
            CullFaceMode.Back         => SilkGLEnum.Back,
            CullFaceMode.FrontAndBack => SilkGLEnum.FrontAndBack,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private static SilkVertexAttribPointerType Translate(VertexAttributeType type)
    {
        return type switch
        {
            VertexAttributeType.Float => SilkVertexAttribPointerType.Float,
            VertexAttributeType.Int   => SilkVertexAttribPointerType.Int,
            VertexAttributeType.Byte  => SilkVertexAttribPointerType.Byte,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    private void SetupVertexLayout(VertexAttribute[] layout)
    {
        var stride = GetVertexStride(layout);
        var pointer = nint.Zero;

        foreach (var attribute in layout)
        {
            var type = Translate(attribute.Type);
            _gl.VertexAttribPointer(
                attribute.Location, 
                attribute.ComponentCount, 
                type, 
                attribute.Normalized,
                stride,
                pointer
            );
        
            _gl.EnableVertexAttribArray(attribute.Location);
            pointer += attribute.ComponentCount * attribute.GetSize();
        }
    }

    private static uint GetVertexStride(VertexAttribute[] layout)
    {
        return (uint) layout.Sum(attribute => attribute.ComponentCount * attribute.GetSize());
    }
}