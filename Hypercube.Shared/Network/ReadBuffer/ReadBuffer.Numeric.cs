namespace Hypercube.Shared.Network.ReadBuffer;

public partial class ReadBuffer
{
    #region Bytes

    public byte ReadByte()
    {
        var array = ReadBits(sizeof(byte) * 8);
        byte result = 0;
        for (byte i = 0; i < sizeof(byte) * 8; i++)
        {
            if (array[i])
            {
                result |= (byte)(1 << i);
            }
        }

        return result;
    }


    public byte[] ReadBytes(int count)
    {
        var output = new byte[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadByte();
        }

        return output;
    }

    public sbyte ReadSByte()
    {
        var array = ReadBits(sizeof(sbyte) * 8);
        sbyte result = 0;
        for (byte i = 0; i < sizeof(sbyte) * 8; i++)
        {
            if (array[i])
            {
                result |= (sbyte)(1 << i);
            }
        }

        return result;
    }


    public sbyte[] ReadSBytes(int count)
    {
        var output = new sbyte[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadSByte();
        }

        return output;
    }

    #endregion

    #region Shorts

    public short ReadShort()
    {
        var array = ReadBits(sizeof(short) * 8);
        short result = 0;
        for (byte i = 0; i < sizeof(short) * 8; i++)
        {
            if (array[i])
            {
                result |= (short)(1 << i);
            }
        }

        return result;
    }

    public short[] ReadShorts(int count)
    {
        var output = new short[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadShort();
        }

        return output;
    }

    public ushort ReadUShort()
    {
        var array = ReadBits(sizeof(ushort) * 8);
        ushort result = 0;
        for (byte i = 0; i < sizeof(ushort) * 8; i++)
        {
            if (array[i])
            {
                result |= (ushort)(1u << i);
            }
        }

        return result;
    }

    public ushort[] ReadUShorts(int count)
    {
        var output = new ushort[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadUShort();
        }

        return output;
    }

    #endregion

    #region Ints

    public int ReadInt()
    {
        var array = ReadBits(sizeof(int) * 8);
        var result = 0;
        for (byte i = 0; i < sizeof(int) * 8; i++)
        {
            if (array[i])
            {
                result |= 1 << i;
            }
        }

        return result;
    }

    public int[] ReadInts(int count)
    {
        var output = new int[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadInt();
        }

        return output;
    }

    public uint ReadUInt()
    {
        var array = ReadBits(sizeof(uint) * 8);
        uint result = 0;
        for (byte i = 0; i < sizeof(uint) * 8; i++)
        {
            if (array[i])
            {
                result |= 1u << i;
            }
        }

        return result;
    }

    public uint[] ReadUInts(int count)
    {
        var output = new uint[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadUInt();
        }

        return output;
    }

    #endregion

    #region Longs

    public long ReadLong()
    {
        var array = ReadBits(sizeof(long) * 8);
        long result = 0;
        for (byte i = 0; i < sizeof(long) * 8; i++)
        {
            if (array[i])
            {
                result |= 1L << i;
            }
        }

        return result;
    }

    public long[] ReadLongs(int count)
    {
        var output = new long[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadLong();
        }

        return output;
    }

    public ulong ReadULong()
    {
        var array = ReadBits(sizeof(ulong) * 8);
        ulong result = 0;
        for (byte i = 0; i < sizeof(ulong) * 8; i++)
        {
            if (array[i])
            {
                result |= 1UL << i;
            }
        }

        return result;
    }

    public ulong[] ReadULongs(int count)
    {
        var output = new ulong[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadULong();
        }

        return output;
    }

    #endregion

    #region Float-pointing numbers

    public float ReadFloat()
    {
        var bytes = new byte[sizeof(float)];
        var bits = ReadBits(sizeof(float) * 8);

        for (var i = 0; i < sizeof(float) * 8; i++)
        {
            if (bits[i])
            {
                bytes[i / 8] |= (byte)(1 << (i % 8));
            }
        }

        return BitConverter.ToSingle(bytes, 0);
    }

    public float[] ReadFloats(int count)
    {
        var output = new float[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadFloat();
        }

        return output;
    }

    public double ReadDouble()
    {
        var bytes = new byte[sizeof(double)];
        var bits = ReadBits(sizeof(double) * 8);

        for (var i = 0; i < sizeof(double) * 8; i++)
        {
            if (bits[i])
            {
                bytes[i / 8] |= (byte)(1 << (i % 8));
            }
        }

        return BitConverter.ToDouble(bytes, 0);
    }

    public double[] ReadDoubles(int count)
    {
        var output = new double[count];
        for (uint i = 0; i < count; i++)
        {
            output[i] = ReadDouble();
        }

        return output;
    }

    #endregion
}