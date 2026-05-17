using System.Runtime.CompilerServices;
using System.Text;
using Hypercube.Utilities.Constants;
using Hypercube.Utilities.Debugging.Logger;
using Hypercube.Utilities.Extensions;
using Logger = Hypercube.Core.Debugging.Logger;

namespace Hypercube.Core.Execution;

public static class RuntimeExceptionHandling
{
    private static readonly Logger Logger = new();

    static RuntimeExceptionHandling()
    {
#if !DEBUG   
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            Handle(args.ExceptionObject as Exception, args.IsTerminating);
        };
        
        TaskScheduler.UnobservedTaskException += (_, args) => 
        {
            Handle(args.Exception, false);
        };
#endif
    }

    public static void RunClassConstructor()
    {
        RuntimeHelpers.RunClassConstructor(typeof(RuntimeExceptionHandling).TypeHandle);
    }
    
    private static void Handle(Exception? exception, bool isTerminating)
    {
        if (exception is null)
            return;

        var sb = new StringBuilder();
        
        var level = isTerminating ? LogLevel.Critical : LogLevel.Error;
        var color = Logger.GetColor(level);
        var separator = new string('=', 50);
        
        var header = isTerminating ? "FATAL EXCEPTION" : "UNHANDLED EXCEPTION";

        sb.AppendLine(header, Ansi.BrightWhite);

        sb.AppendLine(separator, color);
        sb.AppendLine($"Type: {exception.GetType().FullName}", Ansi.BrightWhite);
        sb.AppendLine($"Thread: {Thread.CurrentThread.Name ?? "Unknown"} (ID: {Environment.CurrentManagedThreadId})", Ansi.BrightWhite);
        
        sb.AppendLine(separator, color);
        sb.AppendLineWrapped($"Message: {exception.Message}", Ansi.BrightWhite);

        if (exception.InnerException is not null)
        {
            sb.AppendLine(separator, color);
            sb.AppendLine($"Inner:   {exception.InnerException.Message}");
        }
        
        sb.AppendLine(separator, color);
        sb.AppendLine("[Stack Trace]", color);
        
        var stackTrace = exception.StackTrace?.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        if (stackTrace is not null)
        {
            foreach (var line in stackTrace)
            {
                sb.AppendLine($"  {line.Trim()}");
            }
        }
        
        sb.AppendLine(separator, color);

        if (isTerminating)
            sb.Append("[System] Application is terminating.", color);
        
        Logger.Log(level, sb.ToString());

        if (isTerminating)
            Environment.Exit(1);
    }
}