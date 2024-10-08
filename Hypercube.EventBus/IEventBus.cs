﻿using Hypercube.EventBus.Events;
using JetBrains.Annotations;

namespace Hypercube.EventBus;

/// <summary>
/// EventBus providing interaction between loosely coupled
/// components according to the principle of "event publisher -> event subscriber".
/// </summary>
[PublicAPI]
public interface IEventBus
{
    void Raise<T>(ref T eventArgs) where T : IEventArgs;
    void Raise<T>(IEventSubscriber target, ref T eventArgs) where T : IEventArgs;
    void Raise<T>(T eventArgs) where T : IEventArgs;
    void Raise<T>(IEventSubscriber target, T eventArgs) where T : IEventArgs;
    void Subscribe<T>(IEventSubscriber subscriber, EventRefHandler<T> refHandler) where T : IEventArgs;
    void Unsubscribe<T>(IEventSubscriber subscriber) where T : IEventArgs;
}