using Hypercube.Graphics;
using JetBrains.Annotations;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.OpenGL;

[PublicAPI]
public struct ContextInfo : IContextInfo
{
    public Version Version { get; init; }
    public ContextApi Api { get; init; }
    public ContextProfile Profile { get; init; }
    public ContextFlags Flags { get; init; }

    public bool Compatibility => Profile == ContextProfile.Compatability;

    public override string ToString()
    {
        return $"Context {Version}, api: {Api}, profile: {Profile}, flags: {Flags}";
    }
}