using System.Text;

namespace Hypercube.Shared.Network.WriteBuffer;

public partial class WriteBuffer
{
    public void WriteByteArray(byte[] bytes)
    {
        WriteUInt((uint)bytes.Length);
        WriteBytes(bytes);
    }

    public void WriteString(string s)
    {
        WriteByteArray(Encoding.UTF8.GetBytes(s));
    }
}