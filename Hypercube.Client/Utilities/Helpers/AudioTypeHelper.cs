using System.Collections.Frozen;
using Hypercube.Client.Audio;
using Hypercube.Shared.Utilities.Extensions;

namespace Hypercube.Client.Utilities.Helpers;

public static class AudioTypeHelper
{
    public static readonly FrozenDictionary<string, AudioType> TypesAssociation;

    static AudioTypeHelper()
    {
        var typesAssociation = new Dictionary<string, AudioType>();
        
        foreach (var value in Enum.GetValues(typeof(AudioType)))
        {
            var name = Enum.GetName((AudioType) value) ?? throw new InvalidOperationException();
            
            name = name.ToLower();
            name = name.RemoveChar('_');
            
            typesAssociation.Add(name, (AudioType) value);
        }

        TypesAssociation = typesAssociation.ToFrozenDictionary();
    }
}