using Hypercube.Graphics.Texturing.Parameters;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing;

[PublicAPI]
public interface ITextureHandle : IDisposable
{
    int Handle { get; }
    ITexture Texture { get; }

    void Bind(TextureTarget target);
    void Unbind(TextureTarget target);
}