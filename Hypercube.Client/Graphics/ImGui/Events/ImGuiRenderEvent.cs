using Hypercube.EventBus.Events;
using Hypercube.ImGui;

namespace Hypercube.Client.Graphics.ImGui.Events;

public readonly record struct ImGuiRenderEvent(IImGui Instance) : IEventArgs;