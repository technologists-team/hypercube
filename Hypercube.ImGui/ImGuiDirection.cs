using JetBrains.Annotations;

namespace Hypercube.ImGui;

[PublicAPI]
public enum ImGuiDirection
{
    None = -1,
    
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3,
    
    Count = 4
}