using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Hypercube.Graphics.Core.Commands;

public unsafe class CommandBuffer : IDisposable
{
    private int _capacity = 256;
    private byte* _buffer;
    private int _writeOffset;
    private int _readOffset;

    public void Push<T>(T command, RenderCommandType type) where T : unmanaged
    {
        var commandSize = sizeof(T);
        var totalSize = sizeof(CommandHeader) + commandSize;

        if (_writeOffset + totalSize > _capacity)
            Grow(totalSize);

        var header = new CommandHeader
        {
            Type = type,
            Size = commandSize
        };
        
        *(CommandHeader*)(_buffer + _writeOffset) = header;
        _writeOffset += sizeof(CommandHeader);
        
        *(T*)(_buffer + _writeOffset) = command;
        _writeOffset += commandSize;
    }

    public bool TryGetNext(out RenderCommandType type, out void* data)
    {
        if (_readOffset >= _writeOffset)
        {
            type = default;
            data = null;
            return false;
        }

        var header = *(CommandHeader*)(_buffer + _readOffset);
        _readOffset += sizeof(CommandHeader);

        type = header.Type;
        data = _buffer + _readOffset;

        _readOffset += header.Size;

        return true;
    }

    public void Reset()
    {
        _writeOffset = 0;
        _readOffset = 0;
    }

    private void Grow(int requiredSize)
    {
        Debug.Assert(_capacity > 0);

        var newCapacity = _capacity * 2;

        while (_writeOffset + requiredSize > newCapacity)
            newCapacity *= 2;

        var newBuffer = (byte*)NativeMemory.Alloc((nuint)newCapacity);

        if (_buffer is not null)
        {
            Buffer.MemoryCopy(_buffer, newBuffer, newCapacity, _writeOffset);
            NativeMemory.Free(_buffer);
        }

        _buffer = newBuffer;
        _capacity = newCapacity;
    }

    public void Dispose()
    {
        if (_buffer is null)
            return;
        
        NativeMemory.Free(_buffer);
        _buffer = null;
    }
}
