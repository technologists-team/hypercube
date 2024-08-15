namespace Hypercube.Shared.Logging;

public static class LoggingManager
{
    private static readonly Dictionary<string, Logger> CachedLoggers = new();
    
    public static Logger GetLogger(string name)
    {
        if (CachedLoggers.TryGetValue(name, out var logger))
            return logger;

        return CachedLoggers[name] = new Logger(name);
    }
}