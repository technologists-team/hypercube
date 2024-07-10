using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Texturing;

public class TextureHandle : ITextureHandle
{
    public int Handle { get; init; }
    public ITexture Texture { get; init; }

    public TextureHandle(ITexture texture)
    {
        Handle = GL.GenTexture();
        Texture = texture;
        
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data);
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
    
    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}