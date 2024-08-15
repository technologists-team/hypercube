using System.Collections.Frozen;
using Hypercube.Client.Audio;
using Hypercube.Utilities.Extensions;

namespace Hypercube.Client.Utilities.Helpers;

public static class AudioTypeHelper
{
    private static readonly FrozenDictionary<string, AudioType> TypesAssociation;

    static AudioTypeHelper()
    {
        var typesAssociation = new Dictionary<string, AudioType>();
        
        foreach (var value in Enum.GetValues(typeof(AudioType)))
        {
            var name = Enum.GetName((AudioType) value) ?? throw new InvalidOperationException();
            typesAssociation.Add($".{name.ToLower().RemoveChar('_')}", (AudioType) value);
        }

        TypesAssociation = typesAssociation.ToFrozenDictionary();
    }
    
    /// <exception cref="ArgumentOutOfRangeException">
    /// Throws an exception if the specified extension is not declared in <see cref="AudioType"/>.
    /// </exception>
    public static AudioType GetAudioType(string extension)
    {
        if (!TypesAssociation.TryGetValue(extension, out var audioType))
            throw new ArgumentOutOfRangeException();

        return audioType;
    }
}