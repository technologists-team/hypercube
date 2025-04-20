using System.Diagnostics.CodeAnalysis;
using Hypercube.Core.Ecs.Core;
using Hypercube.Core.Ecs.Core.Events;
using Hypercube.Core.Ecs.Core.Query;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Dependencies;
using JetBrains.Annotations;

namespace Hypercube.Core.Ecs;

/// <summary>
/// An abstract base class for entity systems that operate within a <see cref="World"/>.
/// Entity systems manage and process entities and their components.
/// </summary>
public abstract class EntitySystem : IEntitySystem
{
    [Dependency] protected readonly ILogger Logger = default!;
    
    /// <inheritdoc/>
    [UsedImplicitly(ImplicitUseKindFlags.Assign)]
    public World World { get; private set; } = default!;

    protected EntityQueryBuilder EntityQueryBuilder => World.EntityQueryBuilder;
    
    /// <inheritdoc/>
    public virtual void Startup()
    {
    }

    /// <inheritdoc/>
    public virtual void Shutdown()
    {
    }

    /// <inheritdoc/>
    public virtual void Update(float deltaTime)
    {
    }
    
    protected Entity CreateEntity()
    {
        return World.CreateEntity();
    }
    
    protected bool DestroyEntity(Entity entity)
    {
        return World.DestroyEntity(entity);
    }
    
    /// <summary>
    /// Checks if the specified entity has a component of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the component to check for.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns><c>true</c> if the entity has the component; otherwise, <c>false</c>.</returns>
    protected bool HasComponent<T>(Entity entity) where T : IComponent
    {
        return World.HasComponent<T>(entity);
    }

    /// <summary>
    /// Adds a component of type <typeparamref name="T"/> to the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to add.</typeparam>
    /// <param name="entity">The entity to add the component to.</param>
    /// <returns><c>true</c> if the component was added successfully; otherwise, <c>false</c>.</returns>
    protected bool AddComponent<T>(Entity entity) where T : IComponent
    {
        return World.AddComponent<T>(entity);
    }

    /// <summary>
    /// Removes a component of type <typeparamref name="T"/> from the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to remove.</typeparam>
    /// <param name="entity">The entity to remove the component from.</param>
    /// <returns><c>true</c> if the component was removed successfully; otherwise, <c>false</c>.</returns>
    protected bool RemoveComponent<T>(Entity entity) where T : IComponent
    {
        return World.RemoveComponent<T>(entity);
    }

    /// <summary>
    /// Retrieves the component of type <typeparamref name="T"/> from the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve.</typeparam>
    /// <param name="entity">The entity to retrieve the component from.</param>
    /// <returns>The component of type <typeparamref name="T"/>.</returns>
    protected T GetComponent<T>(Entity entity) where T : IComponent
    {
        return World.GetComponent<T>(entity);
    }

    /// <summary>
    /// Ensures that the specified entity has a component of type <typeparamref name="T"/>.
    /// If the component does not exist, it is added to the entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to ensure.</typeparam>
    /// <param name="entity">The entity to ensure the component for.</param>
    /// <returns>The component of type <typeparamref name="T"/>.</returns>
    protected T EnsureComponent<T>(Entity entity) where T : IComponent
    {
        return World.EnsureComponent<T>(entity);
    }

    /// <summary>
    /// Attempts to retrieve the component of type <typeparamref name="T"/> from the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to retrieve.</typeparam>
    /// <param name="entity">The entity to retrieve the component from.</param>
    /// <param name="component">The output parameter that will contain the component if it exists.</param>
    /// <returns><c>true</c> if the component was found; otherwise, <c>false</c>.</returns>
    protected bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T? component) where T : IComponent
    {
        return World.TryGetComponent(entity, out component);
    }
    
    /// <summary>
    /// Raises an event for a specific component and entity.
    /// </summary>
    /// <typeparam name="TComp">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <typeparam name="TEvent">The type of the event, which must implement <see cref="IEvent"/>.</typeparam>
    /// <param name="entity">The entity associated with the event.</param>
    /// <param name="component">The component associated with the event.</param>
    /// <param name="ev">The event to raise.</param>
    protected void Raise<TComp, TEvent>(Entity entity, TComp component, ref TEvent ev)
        where TComp : IComponent where TEvent : IEvent
    {
        World.Raise(entity, component, ref ev);
    }

    /// <summary>
    /// Subscribes a handler to events of type <typeparamref name="TEvent"/> for components of type <typeparamref name="TComp"/>.
    /// </summary>
    /// <typeparam name="TComp">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <typeparam name="TEvent">The type of the event, which must implement <see cref="IEvent"/>.</typeparam>
    /// <param name="handler">The handler to subscribe.</param>
    protected void Subscribe<TComp, TEvent>(EventRefHandler<TComp, TEvent> handler)
        where TComp : IComponent where TEvent : IEvent
    {
        World.Subscribe(handler);
    }
}