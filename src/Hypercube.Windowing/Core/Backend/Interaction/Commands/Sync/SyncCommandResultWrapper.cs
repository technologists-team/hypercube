namespace Hypercube.Windowing.Core.Backend.Interaction.Commands.Sync;

public sealed class SyncCommandResultWrapper<T> : ISyncCommandResult<T> where T : unmanaged
{
    private readonly TaskCompletionSource<T> _tcs;
    public ICommand<T> InnerCommand { get; }

    public SyncCommandResultWrapper(ICommand<T> innerCommand, TaskCompletionSource<T> tcs)
    {
        InnerCommand = innerCommand;
        _tcs = tcs;
    }

    public void SetResult(T result) => _tcs.TrySetResult(result);
}
