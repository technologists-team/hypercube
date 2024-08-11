using JetBrains.Annotations;

namespace Hypercube.Shared.Network.ReadBuffer;

[PublicAPI]
public sealed partial class ReadBuffer : Buffer
{
    private uint _readPos;
    private byte _readBitPos;

    private uint ReadBitPos
    {
        get => (uint)(_readBitPos + 1);
        set
        {
            var valueCopy = value;
            while (valueCopy > 8)
            {
                valueCopy -= 8;
                _readPos += 1;
                if (_readPos > _maxRead)
                    throw new IndexOutOfRangeException("Message ended");
            }

            _readBitPos = (byte)(valueCopy - 1);
        }
    }

    private readonly uint _maxRead;

    public ReadBuffer(byte[] data)
    {
        Data = data;
        _readPos = 0;
        _maxRead = (uint)data.Length;
    }
}