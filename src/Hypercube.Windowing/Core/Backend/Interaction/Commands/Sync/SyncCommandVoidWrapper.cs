namespace Hypercube.Windowing.Core.Backend.Interaction.Commands.Sync;

public sealed class SyncCommandVoidWrapper : ISyncCommandVoid
{
    private readonly TaskCompletionSource _tcs;
    public ICommand InnerCommand { get; }

    public SyncCommandVoidWrapper(ICommand innerCommand, TaskCompletionSource tcs)
    {
        InnerCommand = innerCommand;
        _tcs = tcs;
    }

    public void SetResult() => _tcs.TrySetResult();
}
