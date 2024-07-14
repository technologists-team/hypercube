using Hypercube.Client.Audio;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Caching.Resource;

namespace Hypercube.Client.Resources.Caching;

public class AudioSourceResource : BaseResource, IDisposable
{
    public ResourcePath Path;
    public IAudioSource Stream;
    
    public override void Load(ResourcePath path, DependenciesContainer container)
    {
        var audioMan = container.Resolve<IAudioManager>();
        Stream = audioMan.CreateSource(path, new AudioSettings());
    }

    public void Dispose()
    {
        Stream.Dispose();
    }
}