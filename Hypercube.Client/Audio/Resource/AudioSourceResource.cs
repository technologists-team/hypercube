using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Audio.Resource;

public sealed class AudioSourceResource : Shared.Resources.Caching.Resource.Resource
{
    public ResourcePath Path;
    public AudioStream Stream = default!;
    
    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        var audioMan = container.Resolve<IAudioManager>();
        Stream = audioMan.GetAudio(path, new AudioSettings());
    }
}