namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureHandle
{
    int Handle { get; }
    ITexture Texture { get; }

    void Bind();
}