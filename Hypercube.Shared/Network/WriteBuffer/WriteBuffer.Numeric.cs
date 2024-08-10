using System.Collections;

namespace Hypercube.Shared.Network.WriteBuffer;

public partial class WriteBuffer
{
    #region Bytes

    public void WriteByte(byte @byte)
    {
        for (var i = 0; i < sizeof(byte) * 8; i++)
            WriteBit((@byte & (1 << i)) != 0);
    }

    public void WriteBytes(byte[] bytes)
    {
        if (bytes.Length < 12) // 96 bits is the size of BitArray
        {
            foreach (var @byte in bytes)
            {
                WriteByte(@byte);
            }
        }
        else
        {
            WriteBits(new BitArray(bytes));
        }
    }

    public void WriteSByte(sbyte @byte)
    {
        WriteByte((byte)@byte);
    }

    public void WriteSBytes(sbyte[] bytes)
    {
        WriteBytes(Array.ConvertAll(bytes, item => (byte)item));
    }

    #endregion

    #region Shorts

    public void WriteShort(short @short)
    {
        var bytes = BitConverter.GetBytes(@short);
        WriteBytes(bytes);
    }

    public void WriteShorts(short[] shorts)
    {
        var len = sizeof(short) * shorts.Length;
        var bytes = new byte[len];
        for (var i = 0; i < shorts.Length; i++)
        {
            var shortBytes = BitConverter.GetBytes(shorts[i]);
            Array.Copy(shortBytes, 0, bytes, i * sizeof(short), sizeof(short));
        }

        WriteBytes(bytes);
    }

    public void WriteUShort(ushort @ushort)
    {
        var bytes = BitConverter.GetBytes(@ushort);
        WriteBytes(bytes);
    }

    public void WriteUShorts(ushort[] ushorts)
    {
        var len = sizeof(ushort) * ushorts.Length;
        var bytes = new byte[len];
        for (var i = 0; i < ushorts.Length; i++)
        {
            var ushortBytes = BitConverter.GetBytes(ushorts[i]);
            Array.Copy(ushortBytes, 0, bytes, i * sizeof(ushort), sizeof(ushort));
        }

        WriteBytes(bytes);
    }

    #endregion

    #region Ints

    public void WriteInt(int value)
    {
        var bytes = BitConverter.GetBytes(value);
        WriteBytes(bytes);
    }

    public void WriteInts(int[] values)
    {
        var len = sizeof(int) * values.Length;
        var bytes = new byte[len];
        for (var i = 0; i < values.Length; i++)
        {
            var intBytes = BitConverter.GetBytes(values[i]);
            Array.Copy(intBytes, 0, bytes, i * sizeof(int), sizeof(int));
        }

        WriteBytes(bytes);
    }

    public void WriteUInt(uint value)
    {
        var bytes = BitConverter.GetBytes(value);
        WriteBytes(bytes);
    }

    public void WriteUInts(uint[] values)
    {
        var len = sizeof(uint) * values.Length;
        var bytes = new byte[len];
        for (var i = 0; i < values.Length; i++)
        {
            var uintBytes = BitConverter.GetBytes(values[i]);
            Array.Copy(uintBytes, 0, bytes, i * sizeof(uint), sizeof(uint));
        }

        WriteBytes(bytes);
    }

    #endregion

    #region Longs

    public void WriteLong(long value)
    {
        var bytes = BitConverter.GetBytes(value);
        WriteBytes(bytes);
    }

    public void WriteLongs(long[] values)
    {
        var len = sizeof(long) * values.Length;
        var bytes = new byte[len];
        for (var i = 0; i < values.Length; i++)
        {
            var longBytes = BitConverter.GetBytes(values[i]);
            Array.Copy(longBytes, 0, bytes, i * sizeof(long), sizeof(long));
        }

        WriteBytes(bytes);
    }

    public void WriteULong(ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        WriteBytes(bytes);
    }

    public void WriteULongs(ulong[] values)
    {
        var len = sizeof(ulong) * values.Length;
        var bytes = new byte[len];
        for (var i = 0; i < values.Length; i++)
        {
            var ulongBytes = BitConverter.GetBytes(values[i]);
            Array.Copy(ulongBytes, 0, bytes, i * sizeof(ulong), sizeof(ulong));
        }

        WriteBytes(bytes);
    }

    #endregion

    #region Float-pointing numbers

    public void WriteFloat(float value)
    {
        var bytes = BitConverter.GetBytes(value);
        WriteBytes(bytes);
    }

    public void WriteFloats(float[] values)
    {
        var len = sizeof(float) * values.Length;
        var bytes = new byte[len];
        for (var i = 0; i < values.Length; i++)
        {
            var floatBytes = BitConverter.GetBytes(values[i]);
            Array.Copy(floatBytes, 0, bytes, i * sizeof(float), sizeof(float));
        }

        WriteBytes(bytes);
    }

    public void WriteDouble(double value)
    {
        var bytes = BitConverter.GetBytes(value);
        WriteBytes(bytes);
    }

    public void WriteDoubles(double[] values)
    {
        var len = sizeof(double) * values.Length;
        var bytes = new byte[len];
        for (var i = 0; i < values.Length; i++)
        {
            var doubleBytes = BitConverter.GetBytes(values[i]);
            Array.Copy(doubleBytes, 0, bytes, i * sizeof(double), sizeof(double));
        }

        WriteBytes(bytes);
    }

    #endregion
}