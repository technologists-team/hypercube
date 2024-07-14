namespace Hypercube.Client.Audio.Realisations.OpenAL;

public sealed partial class OpenAlAudioManager
{
    public void StartAll()
    {
        foreach (var weakReference in _loadedSources)
        {
            if (!weakReference.TryGetTarget(out var source))
                continue;
            
            source.Start();   
        }
    }

    public void PauseAll()
    {
        foreach (var weakReference in _loadedSources)
        {
            if (!weakReference.TryGetTarget(out var source))
                continue;
            
            source.Pause();   
        }
    }

    public void StopAll()
    {
        foreach (var weakReference in _loadedSources)
        {
            if (!weakReference.TryGetTarget(out var source))
                continue;
            
            source.Stop();   
        }
    }
}