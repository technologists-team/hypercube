using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Hypercube.Utilities.Extensions;

[PublicAPI]
public static class BinaryReaderExtension
{
    public static short[] ReadInts16(this BinaryReader reader, int count)
    {
        if (count == 0)
            return [];

        var result = new short[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = reader.ReadInt16();
        }
        
        return result;
    }
    
    public static ushort[] ReadUInts16(this BinaryReader reader, int count)
    {
        if (count == 0)
            return [];

        var result = new ushort[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = reader.ReadUInt16();
        }
        
        return result;
    }
    
    public static int[] ReadInts32(this BinaryReader reader, int count)
    {
        if (count == 0)
            return [];

        var result = new int[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = reader.ReadInt32();
        }
        
        return result;
    }
    
    public static uint[] ReadUInts32(this BinaryReader reader, int count)
    {
        if (count == 0)
            return [];

        var result = new uint[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = reader.ReadUInt32();
        }
        
        return result;
    }
    
    public static long[] ReadInts64(this BinaryReader reader, int count)
    {
        if (count == 0)
            return [];

        var result = new long[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = reader.ReadInt64();
        }
        
        return result;
    }
    
    public static ulong[] ReadUInts64(this BinaryReader reader, int count)
    {
        if (count == 0)
            return [];

        var result = new ulong[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = reader.ReadUInt64();
        }
        
        return result;
    }
    
    public static T ReadStruct<T>(this BinaryReader reader) where T : notnull
    {
        var bytes = reader.ReadBytes(Marshal.SizeOf<T>());
        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        var result = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject()) ?? throw new NullReferenceException();
        handle.Free();
        return result;
    }
}