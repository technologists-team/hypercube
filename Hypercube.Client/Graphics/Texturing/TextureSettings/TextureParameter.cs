using System.Runtime.Serialization;
using Hypercube.Client.Graphics.Texturing.TextureSettings.TextureParameters;

namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

[Serializable]
public readonly struct TextureParameter(TextureParameterName name, int value)
{
    [DataMember(Name = "name")]
    public readonly TextureParameterName Name = name;
    
    [DataMember(Name = "value")]
    public readonly int Value = value;
}