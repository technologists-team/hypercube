using Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters;

namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureHandle : IDisposable
{
    int Handle { get; }
    ITexture Texture { get; }

    void Bind(TextureTarget target);
    void Unbind(TextureTarget target);
}