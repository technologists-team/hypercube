using Hypercube.Mathematics.Shapes;
using StbTrueTypeSharp;

namespace Hypercube.Core.Graphics.Utilities.Helpers;

[EngineInternal, PublicAPI]
public static class StbTrueTypeHelper
{
    #region Font

    public static unsafe StbTrueType.stbtt_fontinfo GetFont(byte[] data, int offset = 0)
    {
        var fontInfo = new StbTrueType.stbtt_fontinfo();
        fixed (byte* fontPtr = data)
            StbTrueType.stbtt_InitFont(fontInfo, fontPtr, offset);
        return fontInfo;
    }
    
    public static unsafe void GetFontVMetrics(StbTrueType.stbtt_fontinfo fontInfo, out int ascent, out int descent, out int lineGap)
    {
        int ascentPtr, descentPtr, lineGapPtr;
        StbTrueType.stbtt_GetFontVMetrics(fontInfo, &ascentPtr, &descentPtr, &lineGapPtr);
        ascent = ascentPtr;
        descent = descentPtr;
        lineGap = lineGapPtr;
    }
    
    public static unsafe void GetFontBoundingBox(StbTrueType.stbtt_fontinfo fontInfo, out Rect2i boundingBox)
    {
        int x0, y0, x1, y1;
        StbTrueType.stbtt_GetFontBoundingBox(fontInfo, &x0, &y0, &x1, &y1);
        boundingBox = new Rect2i(x0, y0, x1, y1);
    }

    public static IReadOnlyList<char> GetFontChars(StbTrueType.stbtt_fontinfo fontInfo)
    {
        const int maxCodepoint = 0xffff;
        
        var chars = new List<char>();
        for (var codepoint = 0; codepoint <= maxCodepoint; codepoint++)
        {
            if (StbTrueType.stbtt_FindGlyphIndex(fontInfo, (char) codepoint) == 0)
                continue;
            
            chars.Add((char) codepoint);
        }

        return chars;
    }

    #endregion

    #region Glyph

    public static void GetGlyphIndex(StbTrueType.stbtt_fontinfo fontInfo, char @char, out int index)
    {
        index = StbTrueType.stbtt_FindGlyphIndex(fontInfo, @char);
    }
    
    public static unsafe void GetGlyphBitmap(StbTrueType.stbtt_fontinfo fontInfo, int index, Vector2 scale, out byte* bitmap, out Vector2i size, out Vector2i offset)
    {
        var sizePtr = new Vector2i();
        var offsetPtr = new Vector2i();
        
        bitmap = StbTrueType.stbtt_GetGlyphBitmap(
            fontInfo,
            scale.X,
            scale.Y,
            index,
            &sizePtr.X,
            &sizePtr.Y,
            &offsetPtr.X,
            &offsetPtr.Y
        );
        
        size = sizePtr;
        offset = offsetPtr;
    }

    public static unsafe void GetGlyphHMetrics(StbTrueType.stbtt_fontinfo fontInfo, int glyphIndex, out int advanceWith, out int leftSideBearing)
    {
        int advanceWithPtr, leftSideBearingPtr;
        StbTrueType.stbtt_GetGlyphHMetrics(fontInfo, glyphIndex, &advanceWithPtr, &leftSideBearingPtr);
        advanceWith = advanceWithPtr;
        leftSideBearing = leftSideBearingPtr;
    }
    
    #endregion

    public static unsafe void FreeBitmap(byte* bitmap)
    {
        StbTrueType.stbtt_FreeBitmap(bitmap, null);
    }
}
