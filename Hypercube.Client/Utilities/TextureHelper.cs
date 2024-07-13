using System.Runtime.CompilerServices;
using OpenToolkit.Graphics.OpenGL4;
using HPixFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelFormat;
using HPixelType = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelType;
using HPixelInternalFormat = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.PixelInternalFormat;
using HTextureParameterName = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureParameterName;
using HTextureTarget = Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters.TextureTarget;

namespace Hypercube.Client.Utilities;

public static class TextureHelper
{
    /*
     * Texture Parameter Name Conversion
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TextureParameterName ToOpenToolkit(
        this HTextureParameterName textureParameterName)
    {
        return (TextureParameterName)textureParameterName;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HTextureParameterName ToHypercube(this TextureParameterName textureParameterName)
    {
        return (HTextureParameterName)textureParameterName;
    }

    /*
     * PixelFormat Conversion
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PixelFormat ToOpenToolkit(this HPixFormat pixelFormat)
    {
        return (PixelFormat)pixelFormat;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HPixFormat ToHypercube(this PixelFormat pixelFormat)
    {
        return (HPixFormat)pixelFormat;
    }
    
    /*
     * PixelType Conversion
     */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PixelType ToOpenToolkit(this HPixelType pixelType)
    {
        return (PixelType)pixelType;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HPixelType ToHypercube(PixelType pixelType)
    {
        return (HPixelType)pixelType;
    }
    
    /*
     * Pixel Internal Format Conversion
     */
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PixelInternalFormat ToOpenToolkit(this HPixelInternalFormat pixelFormat)
    {
        return (PixelInternalFormat)pixelFormat;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HPixelInternalFormat ToHypercube(this PixelInternalFormat pixelFormat)
    {
        return (HPixelInternalFormat)pixelFormat;
    }
    
    /*
     * Texture Target Conversion
     */

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TextureTarget ToOpenToolkit(this HTextureTarget textureTarget)
    {
        return (TextureTarget)textureTarget;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static HTextureTarget ToHypercube(this TextureTarget textureTarget)
    {
        return (HTextureTarget)textureTarget;
    }
}