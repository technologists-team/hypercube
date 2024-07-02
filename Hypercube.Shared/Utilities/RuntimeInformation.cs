using System.Runtime;
using SysRuntimeInformation = System.Runtime.InteropServices.RuntimeInformation;

namespace Hypercube.Shared.Utilities;

public static class RuntimeInformation
{
    public static string[] GetInformationDump()
    {
        var version = typeof(RuntimeInformation).Assembly.GetName().Version;

        return new[]
        {
            $"OS: {SysRuntimeInformation.OSDescription} {SysRuntimeInformation.OSArchitecture}",
            $".NET Runtime: {SysRuntimeInformation.FrameworkDescription} {SysRuntimeInformation.RuntimeIdentifier}",
            $"Server GC: {GCSettings.IsServerGC}",
            $"Architecture: {SysRuntimeInformation.ProcessArchitecture}",
            $"Hypercube Version: {version}",
        };
    }
}