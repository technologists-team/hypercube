using Hypercube.Client.Audio;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching.Resource;

namespace Hypercube.Client.Resources.Caching;

public sealed class AudioSourceResource : Resource
{
    public ResourcePath Path;
    public AudioStream Stream;
    
    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        var audioMan = container.Resolve<IAudioManager>();
        Stream = audioMan.GetAudio(path, new AudioSettings());
    }
}