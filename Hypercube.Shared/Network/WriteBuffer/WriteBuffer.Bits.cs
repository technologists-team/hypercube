using System.Collections;

namespace Hypercube.Shared.Network.WriteBuffer;

public partial class WriteBuffer
{
    public static void SetBitInByte(ref byte data, byte bitNumber, bool value)
    {
        if (bitNumber > 7)
            throw new IndexOutOfRangeException(
                $"Trying to set bit {bitNumber} from a byte. There are only 8 bits in a byte!");
        if (value)
            data |= (byte)(1 << bitNumber);
        else
            data &= (byte)~(1 << bitNumber);
    }

    public void WriteBit(bool bit)
    {
        SetBitInByte(ref Data[^1], _writeBitPos, bit);
        WriteBitPos++;
    }

    /// <remarks>
    /// Check that there is a fixed array size. You DO NOT need to save arrays with variable length here
    /// </remarks>
    public void WriteBits(bool[] bits)
    {
        foreach (var bit in bits)
        {
            WriteBit(bit);
        }
    }

    /// <remarks>
    /// Check that there is a fixed array size. You DO NOT need to save arrays with variable length here
    /// </remarks>
    public void WriteBits(BitArray bits)
    {
        for (var i = 0; i < bits.Length; i++)
        {
            WriteBit(bits[i]);
        }
    }
}
