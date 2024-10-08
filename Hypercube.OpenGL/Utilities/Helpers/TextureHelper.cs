﻿using System.Runtime.CompilerServices;
using OpenToolkit.Graphics.OpenGL4;

using HPixFormat = Hypercube.Graphics.Texturing.Parameters.PixelFormat;
using HPixelType = Hypercube.Graphics.Texturing.Parameters.PixelType;
using HPixelInternalFormat = Hypercube.Graphics.Texturing.Parameters.PixelInternalFormat;
using HTextureParameterName = Hypercube.Graphics.Texturing.Parameters.TextureParameterName;
using HTextureTarget = Hypercube.Graphics.Texturing.Parameters.TextureTarget;

namespace Hypercube.OpenGL.Utilities.Helpers;

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