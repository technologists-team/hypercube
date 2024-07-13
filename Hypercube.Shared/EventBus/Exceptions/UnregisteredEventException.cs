namespace Hypercube.Shared.EventBus.Exceptions;

public sealed class UnregisteredEventException(Type registrationType) :
    Exception($"Attempted to resolve unregistered event {registrationType.FullName}.");