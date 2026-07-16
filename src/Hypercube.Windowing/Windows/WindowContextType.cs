namespace Hypercube.Windowing.Windows;

/// <summary>
/// Specifies the type of graphics context to be created and managed for a window.
/// </summary>
public enum WindowContextType : byte
{
    /// <summary>
    /// The windowing subsystem does not create a graphics context (NoApi).
    /// The context and rendering surface are created and managed entirely by an external graphics API (e.g., Vulkan, Metal, WebGPU).
    /// </summary>
    External, 
    
    /// <summary>
    /// A full-featured desktop graphics context is created and managed by the windowing subsystem.
    /// Typically maps to the OpenGL Core Profile.
    /// </summary>
    InternalDesktop, 
    
    /// <summary>
    /// A lightweight, embedded graphics context is created and managed by the windowing subsystem.
    /// Optimized for mobile and embedded systems, typically mapping to the OpenGL ES profile.
    /// </summary>
    InternalEmbedded
}
