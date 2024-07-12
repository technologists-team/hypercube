using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Utilities;

public static class TextureHelper
{
    public static TextureParameterName ToOpenToolkit(
        this Graphics.Texturing.TextureSettings.TextureParameterName textureParameterName)
    {
        return (TextureParameterName)(int)textureParameterName;
    }
}