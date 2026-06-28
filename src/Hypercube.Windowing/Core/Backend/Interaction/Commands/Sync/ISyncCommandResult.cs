namespace Hypercube.Windowing.Core.Backend.Interaction.Commands.Sync;

public interface ISyncCommandResult<T> : ICommand where T : unmanaged
{
    ICommand<T> InnerCommand { get; }
    void SetResult(T result);
}
