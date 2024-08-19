namespace Hypercube.Logging;

[Flags]
public enum LoggingLevel
{
    Engine = 1 << 0,
    Debug = 1 << 1,
    Info = 1 << 2,
    Warning = 1 << 3, 
    Error = 1 << 4,
    Fatal = 1 << 5
}