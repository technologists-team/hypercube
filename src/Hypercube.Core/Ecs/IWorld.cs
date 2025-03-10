using System.Diagnostics.CodeAnalysis;
using Hypercube.Core.Ecs.Core.Events;
using Hypercube.Core.Ecs.Events;

namespace Hypercube.Core.Ecs;

/// <summary>
/// Represents a world in an Entity-Component-System (ECS) architecture.
/// The world manages entities, components, and systems, and provides methods
/// for creating, updating, and interacting with them.
/// </summary>
public interface IWorld
{
    /// <summary>
    /// Gets the unique identifier of the world.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Updates the world logic. Called every frame or tick.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last update.</param>
    void Update(float deltaTime);

    /// <summary>
    /// Retrieves a system of type <typeparamref name="T"/> from the world.
    /// </summary>
    /// <typeparam name="T">The type of the system to retrieve.</typeparam>
    /// <returns>The system instance of type <typeparamref name="T"/>.</returns>
    T GetSystem<T>() where T : IEntitySystem;

    /// <summary>
    /// Creates a new entity in the world.
    /// </summary>
    /// <returns>The newly created entity.</returns>
    Entity CreateEntity();

    /// <summary>
    /// Destroys an existing entity in the world.
    /// </summary>
    /// <param name="entity">The entity to destroy.</param>
    /// <returns>True if the entity was destroyed successfully, otherwise false.</returns>
    bool DestroyEntity(Entity entity);

    /// <summary>
    /// Adds a component of type <typeparamref name="T"/> to the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <param name="entity">The entity to add the component to.</param>
    /// <returns>True if the component was added successfully, otherwise false.</returns>
    bool AddComponent<T>(Entity entity) where T : IComponent;

    /// <summary>
    /// Removes a component of type <typeparamref name="T"/> from the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <param name="entity">The entity to remove the component from.</param>
    /// <returns>True if the component was removed successfully, otherwise false.</returns>
    bool RemoveComponent<T>(Entity entity) where T : IComponent;

    /// <summary>
    /// Checks if the specified entity has a component of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <param name="entity">The entity to check.</param>
    /// <returns>True if the entity has the component, otherwise false.</returns>
    bool HasComponent<T>(Entity entity) where T : IComponent;

    /// <summary>
    /// Retrieves a component of type <typeparamref name="T"/> from the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <param name="entity">The entity to get the component from.</param>
    /// <returns>The component instance of type <typeparamref name="T"/>.</returns>
    T GetComponent<T>(Entity entity) where T : IComponent;

    /// <summary>
    /// Attempts to retrieve a component of type <typeparamref name="T"/> from the specified entity.
    /// </summary>
    /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <param name="entity">The entity to get the component from.</param>
    /// <param name="component">Output parameter for the component if found.</param>
    /// <returns>True if the component was found, otherwise false.</returns>
    bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T? component)
        where T : IComponent;

    /// <summary>
    /// Ensures that the specified entity has a component of type <typeparamref name="T"/>.
    /// If the component does not exist, it is added to the entity.
    /// </summary>
    /// <typeparam name="T">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <param name="entity">The entity to ensure the component for.</param>
    /// <returns>The component instance of type <typeparamref name="T"/>.</returns>
    T EnsureComponent<T>(Entity entity) where T : IComponent;

    /// <summary>
    /// Raises an event for a specific component and entity.
    /// </summary>
    /// <typeparam name="TComp">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <typeparam name="TEvent">The type of the event, which must implement <see cref="IEvent"/>.</typeparam>
    /// <param name="entity">The entity associated with the event.</param>
    /// <param name="component">The component associated with the event.</param>
    /// <param name="ev">The event to raise.</param>
    public void Raise<TComp, TEvent>(Entity entity, TComp component, ref TEvent ev)
        where TComp : IComponent where TEvent : IEvent;

    /// <summary>
    /// Subscribes a handler to events of type <typeparamref name="TEvent"/> for components of type <typeparamref name="TComp"/>.
    /// </summary>
    /// <typeparam name="TComp">The type of the component, which must implement <see cref="IComponent"/>.</typeparam>
    /// <typeparam name="TEvent">The type of the event, which must implement <see cref="IEvent"/>.</typeparam>
    /// <param name="handler">The handler to subscribe.</param>
    public void Subscribe<TComp, TEvent>(EventRefHandler<TComp, TEvent> handler)
        where TComp : IComponent where TEvent : IEvent;
}