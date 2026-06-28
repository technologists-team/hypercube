namespace Hypercube.Windowing.Core.Backend.Interaction.Commands.Sync;

public interface ISyncCommandVoid : ICommand
{
    ICommand InnerCommand { get; }
    void SetResult();
}
