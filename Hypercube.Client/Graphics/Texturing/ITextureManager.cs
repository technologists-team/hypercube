using Hypercube.Client.Graphics.Texturing.Settings;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureManager
{
    ITexture GetTexture(ResourcePath path);
    
    ITextureHandle GetTextureHandle(ResourcePath path, ITextureCreationSettings settings);
    ITextureHandle GetTextureHandle(ResourcePath path);

    ITextureHandle GetTextureHandle(ITexture texture, ITextureCreationSettings settings);
    ITextureHandle GetTextureHandle(ITexture texture);
}