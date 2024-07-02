namespace Hypercube.Shared.Logging;

public static class LoggingManager
{
    public static Logger GetLogger(string name)
    {
        return new Logger(name);
    }
}