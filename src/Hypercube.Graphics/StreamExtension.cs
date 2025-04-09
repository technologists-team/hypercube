using System.Text;

namespace Hypercube.Graphics;

public static class StreamExtension
{
    public static string ReadToEnd(this Stream stream, Encoding? encoding = null)
    {
        using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
        return reader.ReadToEnd();
    }
}