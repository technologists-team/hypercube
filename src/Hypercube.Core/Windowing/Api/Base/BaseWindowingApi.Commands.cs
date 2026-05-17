using Hypercube.Core.Graphics.Objects.Texturing;
using Hypercube.Core.Windowing.Windows;

namespace Hypercube.Core.Windowing.Api.Base;

public abstract partial class BaseWindowingApi
{
    /// <summary>
    /// Basic form of the command for accessing the API internals.
    /// </summary>
    protected interface ICommand;

    /// <summary>
    /// A variation of the <see cref="ICommand"/>
    /// for synchronized access to the API internals.
    /// </summary>
    protected interface ICommandSync : ICommand
    {
        TaskCompletionSource Task { get; }
        Thread Thread { get; }
    }

    /// <summary>
    /// A variation of the <see cref="ICommand"/>
    /// for synchronized access to the API internals,
    /// with a certain result.
    /// </summary>
    protected interface ICommandSync<TResult> : ICommand
    {
        TaskCompletionSource<TResult> Task { get; }
        Thread Thread { get; }
    }
    
    private readonly record struct CommandTerminate
        : ICommand;

    private readonly record struct CommandWindowSetTitle(
        WindowHandle Window,
        string Title,
        Thread Thread
    ) : ICommand;

    private readonly record struct CommandWindowSetPosition(
        WindowHandle Window,
        Vector2i Position
    ): ICommand;

    private readonly record struct CommandWindowSetSize(
        WindowHandle Window,
        Vector2i Size
    ) : ICommand;

    private readonly record struct CommandWindowSetFramebufferSize(
        WindowHandle Window,
        Vector2i Size
    ) : ICommand;
    
    private readonly record struct CommandWindowSetIcon(
        WindowHandle Window,
        IImage Icon
    ) : ICommand;
    
    private readonly record struct CommandWindowCreate(
        WindowCreateSettings Settings
    ) : ICommand;

    private readonly record struct CommandWindowCreateSync(
        WindowCreateSettings Settings,
        TaskCompletionSource<nint> Task,
        Thread Thread
    ) : ICommandSync<nint>;
}