using Hypercube.Windowing.Backend.Commands;
using Hypercube.Windowing.Backend.Commands.Window;
using Hypercube.Windowing.Backend.Commands.Window.Icons;

namespace Hypercube.Windowing.Backend.Wrappers;

public sealed class BackendCommander
{
    private readonly IBackend _backend;
    
    public BackendCommander(IBackend backend)
    {
        _backend = backend;   
    }

    public void Execute(ICommand raw)
    {
        switch (raw)
        {
            case CommandInitialize:
                _backend.Initialize();
                break;

            case CommandTerminate:
                _backend.Terminate();
                break;
            
            case CommandWindowCreateSync cmd:
                ExecuteSync(cmd, c => _backend.WindowCreate(c.Settings));
                break;
            
            case CommandWindowDestroy cmd:
                _backend.WindowDestroy(cmd.Window);
                break;

            case CommandWindowSetTitle cmd:
                _backend.WindowSetTitle(cmd.Window, cmd.Title);
                break;

            case CommandWindowSetPosition cmd:
                _backend.WindowSetPosition(cmd.Window, cmd.Position);
                break;

            case CommandWindowSetSize cmd:
                _backend.WindowSetSize(cmd.Window, cmd.Size);
                break;

            case CommandWindowSetIcon cmd:
                _backend.WindowSetIcon(cmd.Window, cmd.Icon);
                break;

            case CommandWindowSetIcons cmd:
                _backend.WindowSetIcons(cmd.Window, cmd.Icons);
                break;

            case CommandWindowShow cmd:
                _backend.WindowShow(cmd.Window);
                break;

            case CommandWindowHide cmd:
                _backend.WindowHide(cmd.Window);
                break;

            case CommandWindowMaximize cmd:
                _backend.WindowMaximize(cmd.Window);
                break;

            case CommandWindowMinimize cmd:
                _backend.WindowMinimize(cmd.Window);
                break;

            case CommandWindowRestore cmd:
                _backend.WindowRestore(cmd.Window);
                break;
        }
    }
    
    private static void ExecuteSync<TCommand, TResult>(TCommand command, Func<TCommand, TResult> action) where TCommand : ISyncCommand<TResult>
    {
        try
        {
            var result = action(command);
            command.ResultSource?.SetResult(result);
        }
        catch (Exception ex)
        {
            command.ResultSource?.SetException(ex);
        }
    }
}
