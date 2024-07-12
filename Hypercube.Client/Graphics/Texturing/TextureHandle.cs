using Hypercube.Client.Graphics.Texturing.TextureSettings;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Texturing;

public class TextureHandle : ITextureHandle
{
    public int Handle { get; init; }
    public ITexture Texture { get; init; }

    public TextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        Handle = GL.GenTexture();
        Texture = texture;
        
        GL.BindTexture(settings.TextureTarget, Handle);

        foreach (var param in settings.Parameters)
        {
            GL.TexParameter(settings.TextureTarget, param.ParameterName, param.ParameterValue);
        }
        
        GL.TexImage2D(settings.TextureTarget, settings.Level, settings.PixelInternalFormat, texture.Width, texture.Height, settings.Border, settings.PixelFormat, settings.PixelType, texture.Data);
        
        // there should more elegant way
        GL.GenerateMipmap((GenerateMipmapTarget)(int)settings.TextureTarget);
    }
    
    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}