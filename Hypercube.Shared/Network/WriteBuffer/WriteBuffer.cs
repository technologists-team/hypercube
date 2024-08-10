using JetBrains.Annotations;

namespace Hypercube.Shared.Network.WriteBuffer;

[PublicAPI]
public partial class WriteBuffer : Buffer
{
    private byte _writeBitPos;

    private byte WriteBitPos
    {
        get => (byte)(_writeBitPos + 1);
        set
        {
            if (value > 8)
            {
                Data[Data.Length + 1] = 0;
                _writeBitPos = 0;
            }
            else
            {
                _writeBitPos = (byte)(value - 1);
            }
            
        }
    }

    public WriteBuffer()
    {
        _writeBitPos = 0;
        Data = [];
    }
}