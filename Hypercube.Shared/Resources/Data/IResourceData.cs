namespace Hypercube.Shared.Resources.Data;

/// <summary>
/// Data resources, is a special kind that has its own <see cref="Id"/>
/// to work with and was inspired by the prototypes from RobustToolbox.
/// Its essence is that we load many resources, and get them not by their path,
/// but by a unique <see cref="Id"/> for each type.
/// <para/>
/// This is extremely handy for creating prefabs,
/// or just different DTOs, loot tables, etc.
/// </summary>
public interface IResourceData
{
    string Id { get; }
}