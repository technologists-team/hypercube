using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public class TextureParameter : ITextureParameter
{
    public TextureParameterName ParameterName { get; }
    public int ParameterValue { get; }

    public TextureParameter(TextureParameterName name, int value)
    {
        ParameterName = name;
        ParameterValue = value;
    }
}