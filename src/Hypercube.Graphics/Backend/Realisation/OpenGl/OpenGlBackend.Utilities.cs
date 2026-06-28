using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Types;
using Silk.NET.OpenGL;
using PrimitiveType = Hypercube.Graphics.Core.Types.PrimitiveType;

// Silk redefine for reduce type/namespace collisions
using SilkVertexAttribPointerType = Silk.NET.OpenGL.VertexAttribPointerType;
using SilkPrimitiveType = Silk.NET.OpenGL.PrimitiveType;

namespace Hypercube.Graphics.Backend.Realisation.OpenGl;

public sealed partial class OpenGlBackend
{
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
    
    private static GLEnum Translate(CullFaceMode mode)
    {
        return mode switch
        {
            CullFaceMode.Front        => GLEnum.Front,
            CullFaceMode.Back         => GLEnum.Back,
            CullFaceMode.FrontAndBack => GLEnum.FrontAndBack,
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