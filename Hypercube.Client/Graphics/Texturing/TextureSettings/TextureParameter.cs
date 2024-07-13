using Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public readonly struct TextureParameter(TextureParameterName name, int value)
{
    public TextureParameterName ParameterName { get; } = name;
    public int ParameterValue { get; } = value;
}