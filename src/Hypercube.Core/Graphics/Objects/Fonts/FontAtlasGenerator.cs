using System.Collections.Frozen;
using Hypercube.Core.Graphics.Utilities.Helpers;
using Hypercube.Mathematics.Shapes;
using StbImageWriteSharp;
using StbTrueTypeSharp;

namespace Hypercube.Core.Graphics.Objects.Fonts;

public static class FontAtlasGenerator
{
    public static unsafe Stream Generate(byte[] data, int fontSize, out FontInfo info)
    {
        const int padding = 2;
        
        var font = StbTrueTypeHelper.GetFont(data);
        var scale = StbTrueType.stbtt_ScaleForPixelHeight(font, fontSize);

        StbTrueTypeHelper.GetFontVMetrics(font, out var ascent, out var descent, out var lineGap);

        var cellSize = fontSize + padding;

        var chars = StbTrueTypeHelper.GetFontChars(font);

        var grid = GetGrid(chars.Count);
        var atlasSize = grid * cellSize;

        // RGBA image
        var atlasData = new byte[atlasSize.X * atlasSize.Y * 4];
        var x = 0;
        var y = 0;

        var glyphs = new Dictionary<char, Glyph>();
        for (var i = 0; i < chars.Count; i++)
        {
            var character = chars[i];
            
            StbTrueTypeHelper.GetGlyphIndex(font, character, out var index);
            if (index == 0)
                continue;
            
            StbTrueTypeHelper.GetGlyphBitmap(font, index, new Vector2(scale), out var bitmap, out var bitmapSize, out var offset);
            
            // Copy glyph into RGBA image
            for (var j = 0; j < bitmapSize.Y; j++)
            {
                for (var k = 0; k < bitmapSize.X; k++)
                {
                    var value = bitmap[j * bitmapSize.X + k];
                    var dstX = x + k;
                    var dstY = y + j;
                    var dstIndex = (dstY * atlasSize.X + dstX) * 4;
                    
                    atlasData[dstIndex + 0] = byte.MaxValue;
                    atlasData[dstIndex + 1] = byte.MaxValue;
                    atlasData[dstIndex + 2] = byte.MaxValue;
                    atlasData[dstIndex + 3] = value;
                }
            }

            StbTrueTypeHelper.FreeBitmap(bitmap);
            StbTrueTypeHelper.GetGlyphHMetrics(font, index, out var advanceWidth, out var bearingX);
            
            glyphs.Add(character, new Glyph(
                character,
                Rect2.FromSize(new Vector2(x, y), bitmapSize),
                offset,
                advanceWidth * scale
            ));

            x += cellSize;
            
            if (x + cellSize <= atlasSize.X)
                continue;
            
            x = 0;
            y += cellSize;
        }

        info = new FontInfo(
            glyphs.ToFrozenDictionary(),
            0,
            ascent,
            descent,
            lineGap,
            scale
        );
        
        return CreateImageStream(atlasData, atlasSize);
    }

    private static Vector2i GetGrid(int count)
    {
        var columns = (int) MathF.Ceiling(MathF.Sqrt(count));
        var rows = (int) MathF.Ceiling(count / (float) columns);
        
        var grid = new Vector2i(columns, rows);

        return grid;
    }
    
    private static MemoryStream CreateImageStream(byte[] data, Vector2i size)
    {
        var stream = new MemoryStream();
        var writer = new ImageWriter();
        
        writer.WritePng(data, size.X, size.Y, ColorComponents.RedGreenBlueAlpha, stream);
        stream.Position = 0;

        return stream;
    }
}