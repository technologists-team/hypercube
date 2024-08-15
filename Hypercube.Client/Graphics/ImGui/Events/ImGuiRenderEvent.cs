using Hypercube.EventBus.Events;

namespace Hypercube.Client.Graphics.ImGui.Events;

public readonly record struct ImGuiRenderEvent(IImGui Instance) : IEventArgs;