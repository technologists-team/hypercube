using System.Runtime;
using JetBrains.Annotations;
using SysRuntimeInformation = System.Runtime.InteropServices.RuntimeInformation;

namespace Hypercube.Utilities;

[PublicAPI]
public static class RuntimeInformation
{
    public static string[] GetInformationDump()
    {
        var version = typeof(RuntimeInformation).Assembly.GetName().Version;
        
        return
        [
            $"OS: {SysRuntimeInformation.OSDescription} {SysRuntimeInformation.OSArchitecture}",
            $".NET Runtime: {SysRuntimeInformation.FrameworkDescription} {SysRuntimeInformation.RuntimeIdentifier}",
            $"Server GC: {GCSettings.IsServerGC}",
            $"Architecture: {SysRuntimeInformation.ProcessArchitecture}",
            $"Version: {version}"
        ];
    }
}