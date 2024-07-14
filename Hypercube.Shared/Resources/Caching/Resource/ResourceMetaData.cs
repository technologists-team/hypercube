using Tomlyn;
using Tomlyn.Model;

namespace Hypercube.Shared.Resources.Caching.Resource;

public class ResourceMetaData
{
    public TomlTable Table { get; }

    public ResourceMetaData(string toml)
    {
        Table = Toml.ToModel(toml);;
    }
}

public sealed class ResourceMetaData<T> : ResourceMetaData where T : class, new()
{
    public new T Table { get; }

    public ResourceMetaData(string toml) : base(toml)
    {
        Table = Toml.ToModel<T>(toml);
    }
}