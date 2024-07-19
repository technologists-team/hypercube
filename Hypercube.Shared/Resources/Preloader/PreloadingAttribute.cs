namespace Hypercube.Shared.Resources.Preloader;

[AttributeUsage(AttributeTargets.Method)]
public sealed class PreloadingAttribute : Attribute
{
    public readonly Type? Event;

    public PreloadingAttribute(Type? @event = null)
    {
        Event = @event;
    }
}