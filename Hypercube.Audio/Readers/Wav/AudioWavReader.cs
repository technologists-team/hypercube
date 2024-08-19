using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace Hypercube.Audio.Readers.Wav;

[PublicAPI]
public sealed class AudioWavReader : IDisposable
{
    private const int ChunkSize = 4 * sizeof(byte) + 1 * sizeof(uint);
    
    private readonly Stream _stream;
    private readonly BinaryReader _reader;
    
    public AudioWavReader(Stream stream)
    {
        _stream = stream;
        _reader = new BinaryReader(_stream, Encoding.UTF8, true);
    }

    public AudioWavData Read()
    {
        Span<byte> chunk = stackalloc byte[4];
        
        // Read riff chunk
        ReadFullChunk(chunk, out var riffLength);
        if (!chunk.SequenceEqual("RIFF"u8))
            throw new InvalidOperationException();
        
        if (riffLength + ChunkSize != _stream.Length)
            throw new InvalidOperationException();
        
        // Read riff sub chunks
        ReadChunkId(chunk);
        if (!chunk.SequenceEqual("WAVE"u8))
            throw new InvalidOperationException();
        
        ReadFullChunk(chunk, out var fmtLength);
        if (!chunk.SequenceEqual("fmt "u8))
            throw new InvalidOperationException();
        
        // Read fmt chunk
        var fmtDataPosition = _reader.BaseStream.Position;
        
        var format = _reader.ReadInt16();
        var channels = _reader.ReadInt16();
        var sampleRate = _reader.ReadInt32();
        var byteRate = _reader.ReadInt32();
        var blockAlign = _reader.ReadInt16();
        var bitsPerSample = _reader.ReadInt16();

        _stream.Position = fmtDataPosition + fmtLength;
        
        SkipTo(chunk, "data"u8, out var dataLength);
        
        // Read data chunk
        var data = _reader.ReadBytes((int)dataLength);
        return new AudioWavData(format, channels, sampleRate, byteRate, blockAlign, bitsPerSample, data);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SkipTo(Span<byte> chunk, ReadOnlySpan<byte> target, out uint length)
    {
        while (true)
        {
            ReadFullChunk(chunk, out length);
            if (chunk.SequenceEqual(target))
                break;

            _stream.Position += length;
        }
    }
    
    private void ReadFullChunk(Span<byte> chunk, out uint length)
    {
        ReadChunkId(chunk);
        length = _reader.ReadUInt32();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReadChunkId(Span<byte> chunk)
    {
        chunk[0] = _reader.ReadByte();
        chunk[1] = _reader.ReadByte();
        chunk[2] = _reader.ReadByte();
        chunk[3] = _reader.ReadByte();
    }

    public void Dispose()
    {
        _stream.Dispose();
        _reader.Dispose();
    }
}