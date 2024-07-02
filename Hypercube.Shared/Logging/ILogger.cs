namespace Hypercube.Shared.Logging;

public interface ILogger
{
    void Debug(string message);
    void EngineInfo(string message);
    void Warning(string message);
    void Error(string message);
    void Fatal(string message);
}