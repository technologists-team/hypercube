namespace Hypercube.Graphics.Core.Context;

/// <summary>
/// Represents a way to get information about specific functions or procedures in a process or application.
/// This is used to find and access particular parts of the program's code dynamically.
/// </summary>
public interface IContextInfoProvider
{
    /// <summary>
    /// Looks up the address of a specific function or procedure by its name.
    /// This is useful when you need to call a function, but you only have its name, not its location in memory.
    /// </summary>
    /// <param name="name">The name of the function or procedure you want to find.</param>
    /// <returns>The memory address of the function, which can be used to call it.</returns>
    nint GetProcAddress(string name);
}
