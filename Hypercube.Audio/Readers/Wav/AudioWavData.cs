using Hypercube.Audio.Loading;
using JetBrains.Annotations;

namespace Hypercube.Audio.Readers.Wav;

/// <summary>
/// Read the <a href="http://soundfile.sapp.org/doc/WaveFormat/">specification</a> for more information
/// </summary>
/// <remarks>
/// 8-bit samples are stored as unsigned bytes, ranging from 0 to 255.
/// 16-bit samples are stored as 2's-complement signed integers, ranging from -32768 to 32767.
/// </remarks>
[PublicAPI]
public readonly struct AudioWavData : IAudioData
{
    public AudioFormat Format { get; }
    public ReadOnlyMemory<byte> Data { get; }
    public int SampleRate { get; }
    public TimeSpan Length { get; }
    
    public readonly short FormatType;
    public readonly short Channels;
    public readonly int ByteRate;
    public readonly short BlockAlign;
    public readonly short BitsPerSample;
    
    public AudioWavData(short formatType, short channels, int sampleRate, int byteRate, short blockAlign, short bitsPerSample, ReadOnlyMemory<byte> data)
    {
        Format = GetFormat(channels, bitsPerSample);
        Data = data;
        SampleRate = sampleRate;
        Length = TimeSpan.FromSeconds(data.Length / (double) blockAlign / sampleRate);
        
        FormatType = formatType;
        Channels = channels;
        ByteRate = byteRate;
        BlockAlign = blockAlign;
        BitsPerSample = bitsPerSample;
    }

    public override string ToString()
    {
        return $"format type {FormatType}, channels {Channels}, sample rate {SampleRate}, byte rate {ByteRate}, block align {BlockAlign}, bits per sample {BitsPerSample}, data length {Data.Length}";
    }
    
    private static AudioFormat GetFormat(int channels, int bits)
    {
        return bits switch
        {   
            8 => channels switch
            {
                1 => AudioFormat.Mono8,
                2 => AudioFormat.Stereo8,
                _ => throw new InvalidOperationException()
            },
            16 => channels switch
            {
                1 => AudioFormat.Mono16,
                2 => AudioFormat.Stereo16,
                _ => throw new InvalidOperationException()
            },
            _ => throw new InvalidOperationException()
        };
    }
}