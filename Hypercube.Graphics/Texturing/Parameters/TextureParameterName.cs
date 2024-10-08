﻿using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing.Parameters;

[PublicAPI]
public enum TextureParameterName
{
    TextureWidth = 4096,
    TextureHeight = 4097,
    
    TextureComponents = 4099,
    TextureInternalFormat = 4099,
    
    TextureBorderColor = 4100,
    TextureBorderColorNv = 4100,
    TextureBorder = 4101,
    
    TextureMagFilter = 10240,
    TextureMinFilter = 10241,
    
    TextureWrapS = 10242,
    TextureWrapT = 10243,
    
    TextureRedSize = 32860,
    TextureGreenSize = 32861,
    TextureBlueSize = 32862,
    TextureAlphaSize = 32863,
    TextureLuminanceSize = 32864,
    TextureIntensitySize = 32865,
    
    TexturePriority = 32870,
    TexturePriorityExt = 32870,
    
    TextureResident = 32871,
    
    TextureDepth = 32881,
    TextureDepthExt = 32881,
    
    TextureWrapR = 32882,
    TextureWrapRExt = 32882,
    TextureWrapROes = 32882,
    
    DetailTextureLevelSgis = 32992,
    DetailTextureModeSgis = 32923,
    DetailTExtureFuncPointsSgis = 32924,
    
    SharpenTextureFuncPointsSgis = 32994,

    ShadowAmbientSgix = 32959,
    TextureCompareFailValue = 32959,
    
    DualTextureSelectSgis = 33060,
    QuadTextureSelectSgis = 33061,
    
    ClampToBorder = 33069,
    ClampToEdge = 33071,
    
    Texture4DsizeSgis = 33078,
    
    TextureWrapQSgis = 33079,
    
    TextureMinLod = 33082,
    TextureMinLodSgis = 33082,
    
    TextureMaxLod = 33083,
    TextureMaxLodSgis = 33083,
    
    TextureBaseLevel = 33084,
    TextureBaseLevelSgis = 33084,
    
    TextureMaxLevel = 33085,
    TextureMaxLevelSgis = 33085,
    
    TextureFilter4SizeSgis = 33095,
    
    TextureClipmapCenterSgix = 33137,
    TextureClipmapFrameSgix = 33138,
    TextureClipmapOffsetSgix = 33139,
    TextureClipmapVirtualDepthSgix = 33140,
    TextureClipmapLodOffsetSgix = 33141,
    TextureClipmapDepthSgix = 33142,
   
    PostTextureFilterBiasSgix = 33145,
    PostTextureFilterScaleSgix = 33146,
    
    TextureLodBiasSSgix = 33166,
    TextureLodBiasTSgix = 33167,
    TextureLodBiasRSgix = 33168,
    
    GenerateMipmap = 33169,
    GenerateMipmapSgis = 33169,
    
    TextureCompareSgix = 33178,
    TextureCompareOperatorSgix = 33179,
    
    TextureLequalRSgix = 33180,
    TextureGequalRSgix = 33181,
    
    TextureMaxClampSSgix = 33641,
    TextureMaxClampTSgix = 33642,
    TextureMaxClampRSgix = 33643,
    
    TextureLodBias = 34049,
    DepthTextureMode = 34891,
    
    TextureCompareMode = 34892,
    TextureCompareFunc = 34893,
    
    TextureSwizzleR = 36418,
    TextureSwizzleG = 36419,
    TextureSwizzleB = 36420,
    TextureSwizzleA = 36421,
    TextureSwizzleRgba = 36422,
    
    DepthStencilTextureMode = 37098,
    
    TextureTilingExt = 38272
}