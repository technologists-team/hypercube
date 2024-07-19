using Hypercube.Client.Audio.Settings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Resources;

namespace Hypercube.Client.Audio.Resources;

public sealed class AudioResource : Resource, IDisposable
{
    public ResourcePath Path;
    public AudioStream Stream { get; private set; } = default!;
    
    public AudioResource()
    {
        Path = string.Empty;
    }
    
    public AudioResource(ResourcePath path)
    {
        Path = path;
    }
    
    protected override void OnLoad(ResourcePath path, DependenciesContainer container)
    {
        var audioMan = container.Resolve<IAudioManager>();
        Stream = audioMan.CreateStream(path, new AudioSettings());
    }

    /// <remarks>
    /// It is better not to Dispose audio streams,
    /// as this leads to a call to delete the handler and re-create it,
    /// which can be time-consuming.
    /// </remarks>
    public void Dispose()
    {
        Stream.Dispose();
    }
}