using OpenTK.Audio.OpenAL;

namespace Hypercube.Client.Audio.Realisations.OpenAL;

public sealed partial class OpenAlAudioManager
{
    private readonly struct AudioHandler : IDisposable
    {
        public readonly int Buffer;

        public AudioHandler(int buffer)
        {
            Buffer = buffer;
        }

        public void Dispose()
        {
            AL.DeleteBuffer(Buffer);
        }
    }
}