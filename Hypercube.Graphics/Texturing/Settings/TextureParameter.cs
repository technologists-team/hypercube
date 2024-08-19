using Hypercube.Graphics.Texturing.Parameters;
using JetBrains.Annotations;

namespace Hypercube.Graphics.Texturing.Settings;

[PublicAPI]
public readonly struct TextureParameter
{
    public readonly TextureParameterName Name;
    public readonly int Value;

    public TextureParameter(TextureParameterName name, int value)
    {
        Name = name;
        Value = value;
    }
}