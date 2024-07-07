using System.Globalization;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Entities.EventBus;
using Hypercube.Shared.Entities.Manager;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Runtimes.Event;
using Hypercube.Shared.Runtimes.Loop.Event;

namespace Hypercube.Shared.Entities.Systems;

public abstract class EntitySystem : IEntitySystem
{
    [Dependency] private readonly IEntitiesManager _entitiesManager = default!;
    [Dependency] private readonly IEntitiesEventBus _entitiesEventBus = default!;
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;

    protected readonly ILogger Logger;

    protected EntitySystem()
    {
        Logger = LoggingManager.GetLogger(GetLoggerName());
    }
    
    public virtual void Initialize()
    {

    }

    public virtual void Shutdown(RuntimeShutdownEvent args)
    {

    }

    public virtual void FrameUpdate(UpdateFrameEvent args)
    {

    }

    private string GetLoggerName()
    {
        var name = GetType().Name;

        // Strip trailing "system"
        if (name.EndsWith("System"))
            name = name.Substring(0, name.Length - "System".Length);

        // Convert CamelCase to snake_case
        // Ignore if all uppercase, assume acronym (e.g. NPC or HTN)
        if (name.All(char.IsUpper))
        {
            name = name.ToLower(CultureInfo.InvariantCulture);
        }
        else
        {
            name = string.Concat(name.Select(x => char.IsUpper(x) ? $"_{char.ToLower(x)}" : x.ToString()));
            name = name.Trim('_');
        }

        return $"system.{name}";
    }

    public void PostInject()
    {
        throw new NotImplementedException();
    }
}