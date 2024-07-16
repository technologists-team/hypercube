using System.Collections.Frozen;

namespace Hypercube.Shared.Logging;

public class Logger(string name) : ILogger
{
    private static readonly FrozenDictionary<LoggingLevel, (string, string)> LevelName = new Dictionary<LoggingLevel, (string, string)>
    {
        { LoggingLevel.Debug, ("DEBG", "\x1b[35m") },
        { LoggingLevel.Engine, ("ENGI", "\x1b[35m") },
        { LoggingLevel.Info, ("INFO", "\x1b[39m") },
        { LoggingLevel.Warning, ("WARN", "\x1b[33m") },
        { LoggingLevel.Error, ("ERRO", "\x1b[31m") },
        { LoggingLevel.Fatal, ("FATL", "\x1b[91m") },
    }.ToFrozenDictionary();
    

    public readonly string Name = name;

    private void Log(string message, LoggingLevel level)
    {
        var normalColor = "\x1b[39m";
        var (levelName, levelColor) = LevelName[level];
        Console.WriteLine($"{normalColor}[{levelColor}{levelName}{normalColor}] {Name}: {message}");
    }

    public void Debug(string message)
    {
        Log(message, LoggingLevel.Debug);
    }

    public void EngineInfo(string message)
    {
        Log(message, LoggingLevel.Engine);
    }

    public void Info(string message)
    {
        Log(message, LoggingLevel.Info);
    }

    public void Warning(string message)
    {
        Log(message, LoggingLevel.Warning);
    }

    public void Error(string message)
    {
        Log(message, LoggingLevel.Error);
    }

    public void Fatal(string message)
    {
        Log(message, LoggingLevel.Fatal);
    }
}