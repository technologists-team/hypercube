using System.Runtime.InteropServices;

namespace Hypercube.Graphics.Core.Resources;

public sealed unsafe class ResourceAllocator : IDisposable
{
    private readonly HashSet<nint> _allocations = [];

    public void* Allocate(int size)
    {
        var ptr = NativeMemory.Alloc((nuint) size);
        _allocations.Add((nint) ptr);
        return ptr;
    }
    
    public void* Allocate(Stream stream, out int size)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return Allocate(memoryStream, out size);
    }

    public void* Allocate(MemoryStream stream, out int size)
    {
        return stream.TryGetBuffer(out var buffer)
            ? Allocate(buffer.AsSpan(0, (int) stream.Length), out size)
            : Allocate(stream.ToArray(), out size);
    }

    public void* Allocate(ReadOnlySpan<byte> data, out int size)
    {
        size = data.Length;
        
        var pointer = NativeMemory.Alloc((nuint) size);
        data.CopyTo(new Span<byte>(pointer, data.Length));
        
        _allocations.Add((nint) pointer);
        return pointer;
    }

    public void Free(nint ptr)
    {
        if (_allocations.Remove(ptr))
            NativeMemory.Free((void*) ptr);
    }
    
    public void Free(void* ptr)
    {
        if (_allocations.Remove((nint) ptr))
            NativeMemory.Free(ptr);
    }

    public void FreeAll()
    {
        foreach (var allocation in _allocations)
            NativeMemory.Free((void*) allocation);
        
        _allocations.Clear();
    }

    public void Dispose()
    {
        FreeAll();
    }
}