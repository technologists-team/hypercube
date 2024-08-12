using Hypercube.Client.Graphics.Windows;
using Hypercube.Input;
using Hypercube.Shared.EventBus;

namespace Hypercube.Client.Input.Handler;

/// <summary>
/// Receives requests from user input, via <see cref="IWindowManager"/>,
/// and submits them for further work via events.
/// </summary>
public interface IInputHandler : IEventSubscriber
{
    bool IsKeyDown(Key key);
}