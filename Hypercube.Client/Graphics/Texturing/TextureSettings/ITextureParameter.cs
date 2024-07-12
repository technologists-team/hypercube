using OpenToolkit.Graphics.OpenGL4;
namespace Hypercube.Client.Graphics.Texturing.TextureSettings;

public interface ITextureParameter
{
    TextureParameterName ParameterName { get; }
    int ParameterValue { get; }
}