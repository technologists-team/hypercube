namespace Hypercube.Shared.EventBus.Events;

public delegate void RefHandler(ref Unit ev);
public delegate void EventRefHandler<T>(ref T ev);