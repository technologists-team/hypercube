namespace Hypercube.Windowing.Backend.Commands;

public interface ISyncCommand<T> : ICommand
{
    TaskCompletionSource<T>? ResultSource { get; set; }
}
