namespace Hypercube.Client.Graphics.Shaders;

public interface IShader : IDisposable
{
    public int Handle { get; }
    public void Delete();
}