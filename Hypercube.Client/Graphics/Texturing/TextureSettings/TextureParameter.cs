using Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public readonly struct TextureParameter(TextureParameterName name, int value)
{
    public readonly TextureParameterName Name = name;
    public readonly int Value = value;
}