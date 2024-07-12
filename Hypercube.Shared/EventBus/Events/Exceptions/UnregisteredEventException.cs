namespace Hypercube.Shared.EventBus.Events.Exceptions;

public sealed class UnregisteredEventException(Type registrationType) :
    Exception($"Attempted to resolve unregistered event {registrationType.FullName}.");