using Hypercube.Client.Graphics.Texturing.Parameters;

namespace Hypercube.Client.Graphics.Texturing.Settings;

public readonly struct TextureParameter(TextureParameterName name, int value)
{
    public readonly TextureParameterName Name = name;
    public readonly int Value = value;
}