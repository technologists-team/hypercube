using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Client.Utilities;
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
        var target = settings.TextureTarget.ToOpenToolkit();
        
        GL.BindTexture(target, Handle);
        
        foreach (var param in settings.Parameters)
        {
            GL.TexParameter(target, param.ParameterName.ToOpenToolkit(), param.ParameterValue);
        }
        
        GL.TexImage2D(
            settings.TextureTarget.ToOpenToolkit(), 
            settings.Level, 
            settings.PixelInternalFormat.ToOpenToolkit(), 
            texture.Width, texture.Height, 
            settings.Border, 
            settings.PixelFormat.ToOpenToolkit(), 
            settings.PixelType.ToOpenToolkit(), 
            texture.Data);
        
        // there should more elegant way
        GL.GenerateMipmap((GenerateMipmapTarget)settings.TextureTarget);
    }
    
    public void Bind()
    {
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}