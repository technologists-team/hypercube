namespace Hypercube.Shared.Scenes;

public readonly struct SceneId(int value)
{
    public static readonly SceneId Nullspace = new(-1);

    public readonly int Value = value;
}