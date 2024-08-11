using Hypercube.Shared.Network.ReadBuffer;
using Hypercube.Shared.Network.WriteBuffer;

namespace Hypercube.UnitTests.Network;

public class BufferTest
{
    private static readonly bool[] Bits = [false, false, false, true, false, true, true];
    private static readonly byte[] Bytes = [1, 9, 8, 4];
    private static readonly sbyte[] SBytes = [-1, -9, -8, -4];
    private static readonly short[] Shorts = [-5, -4, -5, 19, 4556, 2546, -4978, 12565];
    private static readonly ushort[] UShorts = [8, 4, 14545, 16854, 28804, 100, 0, 1];
    private static readonly int[] Ints = [-1984, 123456, -654321, 987654321];
    private static readonly uint[] UInts = [1984, 123456, 654321, 987654321];

    private static readonly long[] Longs =
        [-9223372036854775807L, 9223372036854775807L, -1234567890123456789L, 1234567890123456789L];

    private static readonly ulong[] ULongs =
        [9223372036854775807UL, 18446744073709551615UL, 1234567890123456789UL, 9876543210987654321UL];

    private static readonly float[] Floats = [1.234f, -5.678f, 9.012f, -3.456f];

    private static readonly double[] Doubles =
        [1.2345678901234567, -5.67890123456789, 9.012345678901234, -3.456789012345678];

    private const string Str = "The quick brown fox jumps over the lazy dog.";

    [Test]
    public void BufferBitTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteBit(true);
        wBuf.WriteBits(Bits);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadBit(), Is.EqualTo(true));
            Assert.That(rBuf.ReadBits(7), Is.EqualTo(Bits));
        });
        Assert.Pass("Bit RWBuffer passed.");
    }

    [Test]
    public void BufferByteTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteByte(1);
        wBuf.WriteBytes(Bytes);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadByte(), Is.EqualTo((byte)1));
            Assert.That(rBuf.ReadBytes(Bytes.Length), Is.EqualTo(Bytes));
        });
        Assert.Pass("Byte RWBuffer passed.");
    }

    [Test]
    public void BufferSByteTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteSByte(-1);
        wBuf.WriteSBytes(SBytes);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadSByte(), Is.EqualTo((sbyte)-1));
            Assert.That(rBuf.ReadSBytes(SBytes.Length), Is.EqualTo(SBytes));
        });
        Assert.Pass("SByte RWBuffer passed.");
    }

    [Test]
    public void BufferShortTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteShort(-1984);
        wBuf.WriteShorts(Shorts);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadShort(), Is.EqualTo((short)-1984));
            Assert.That(rBuf.ReadShorts(Shorts.Length), Is.EqualTo(Shorts));
        });
        Assert.Pass("Short RWBuffer passed.");
    }

    [Test]
    public void BufferUShortTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteUShort(1984);
        wBuf.WriteUShorts(UShorts);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadUShort(), Is.EqualTo((ushort)1984));
            Assert.That(rBuf.ReadUShorts(UShorts.Length), Is.EqualTo(UShorts));
        });
        Assert.Pass("UShort RWBuffer passed.");
    }

    [Test]
    public void BufferIntTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteInt(-1984);
        wBuf.WriteInts(Ints);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadInt(), Is.EqualTo(-1984));
            Assert.That(rBuf.ReadInts(Ints.Length), Is.EqualTo(Ints));
        });
        Assert.Pass("Int RWBuffer passed.");
    }

    [Test]
    public void BufferUIntTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteUInt(1984);
        wBuf.WriteUInts(UInts);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadUInt(), Is.EqualTo((uint)1984));
            Assert.That(rBuf.ReadUInts(UInts.Length), Is.EqualTo(UInts));
        });
        Assert.Pass("UInt RWBuffer passed.");
    }

    [Test]
    public void BufferLongTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteLong(-9223372036854775807L);
        wBuf.WriteLongs(Longs);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadLong(), Is.EqualTo(-9223372036854775807L));
            Assert.That(rBuf.ReadLongs(Longs.Length), Is.EqualTo(Longs));
        });
        Assert.Pass("Long RWBuffer passed.");
    }

    [Test]
    public void BufferULongTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteULong(9223372036854775807UL);
        wBuf.WriteULongs(ULongs);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadULong(), Is.EqualTo(9223372036854775807UL));
            Assert.That(rBuf.ReadULongs(ULongs.Length), Is.EqualTo(ULongs));
        });
        Assert.Pass("ULong RWBuffer passed.");
    }

    [Test]
    public void BufferFloatTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteFloat(1.234f);
        wBuf.WriteFloats(Floats);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadFloat(), Is.EqualTo(1.234f));
            Assert.That(rBuf.ReadFloats(Floats.Length), Is.EqualTo(Floats));
        });
        Assert.Pass("Float RWBuffer passed.");
    }

    [Test]
    public void BufferDoubleTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteDouble(1.2345678901234567);
        wBuf.WriteDoubles(Doubles);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadDouble(), Is.EqualTo(1.2345678901234567));
            Assert.That(rBuf.ReadDoubles(Doubles.Length), Is.EqualTo(Doubles));
        });
        Assert.Pass("Double RWBuffer passed.");
    }
    
    [Test]
    public void BufferByteArrayTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteByteArray(Bytes);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadByteArray(), Is.EqualTo(Bytes));
        });
        Assert.Pass("Bytearray RWBuffer passed.");
    }

    [Test]
    public void BufferStringTest()
    {
        var wBuf = new WriteBuffer();
        wBuf.WriteString("CURSE OF 220");
        wBuf.WriteString(Str);
        var rBuf = new ReadBuffer(wBuf.GetData());
        Assert.Multiple(() =>
        {
            Assert.That(rBuf.ReadString(), Is.EqualTo("CURSE OF 220"));
            Assert.That(rBuf.ReadString(), Is.EqualTo(Str));
        });
        Assert.Pass("String RWBuffer passed.");
    }
}