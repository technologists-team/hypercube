namespace Hypercube.Client.Graphics.Texturing;

public interface ITextureHandle
{
    public int Handle { get; }
    public ITexture Texture { get; }

    void Bind();
}