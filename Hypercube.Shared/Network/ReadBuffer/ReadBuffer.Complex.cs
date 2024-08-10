using System.Text;

namespace Hypercube.Shared.Network.ReadBuffer;

public partial class ReadBuffer
{
    public byte[] ReadByteArray()
    {
        var length = ReadUInt();
        return ReadBytes((int)length);
    }

    public string ReadString(uint count)
    {
        var bytes = ReadByteArray();
        return Encoding.UTF8.GetString(bytes);
    }
}