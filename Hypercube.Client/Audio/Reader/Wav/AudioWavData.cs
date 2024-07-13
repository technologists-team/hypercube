namespace Hypercube.Client.Audio.Reader.Wav;

/// <summary>
/// Read the <a href="http://soundfile.sapp.org/doc/WaveFormat/">specification</a> for more information
/// </summary>
/// <remarks>
/// 8-bit samples are stored as unsigned bytes, ranging from 0 to 255.
/// 16-bit samples are stored as 2's-complement signed integers, ranging from -32768 to 32767.
/// </remarks>
public readonly struct AudioWavData : IAudioData
{
    public AudioFormat Format { get; }
    
    public readonly short FormatType;
    public readonly short Channels;
    public readonly int SampleRate;
    public readonly int ByteRate;
    public readonly short BlockAlign;
    public readonly short BitsPerSample;
    public readonly byte[] Data;
    
    public AudioWavData(short formatType, short channels, int sampleRate, int byteRate, short blockAlign, short bitsPerSample, byte[] data)
    {
        Format = GetFormat(channels, byteRate);
        
        FormatType = formatType;
        Channels = channels;
        SampleRate = sampleRate;
        ByteRate = byteRate;
        BlockAlign = blockAlign;
        BitsPerSample = bitsPerSample;
        Data = data;
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