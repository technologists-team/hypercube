using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Graphics.OpenGL;

public struct ContextInfo
{
    public bool Compatibility => Profile == ContextProfile.Compatability;
    
    public Version Version;
    public ContextApi Api;
    public ContextProfile Profile;
    public ContextFlags Flags;

    public override string ToString()
    {
        return $"Context {Version}, api: {Api}, profile: {Profile}, flags: {Flags}";
    }
}