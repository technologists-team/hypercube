using OpenToolkit.Audio.OpenAL;

namespace Hypercube.OpenAL;

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