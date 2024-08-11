using System.Collections;

namespace Hypercube.Shared.Network.ReadBuffer;

public partial class ReadBuffer
{
    private static bool ReadBitFromByte(byte data, byte bitNumber)
    {
        if (bitNumber > 7)
            throw new IndexOutOfRangeException(
                $"Trying to read bit {bitNumber} from a byte. There are only 8 bits in a byte!");
        return (data & (1 << bitNumber)) != 0;
    }

    public bool ReadBit()
    {
        var value = ReadBitFromByte(Data[_readPos], _readBitPos);
        ReadBitPos++;
        return value;
    }

    public bool[] ReadBits(uint count)
    {
        var output = new bool[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadBit();
        }

        return output;
    }
    
    public BitArray ReadBitsToArray(int count)
    {
        var output = new BitArray(count);
        for (var i = 0; i < count; i++)
        {
            output[i] = ReadBit();
        }

        return output;
    }
}