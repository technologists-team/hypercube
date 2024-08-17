namespace Hypercube.ImGui;

public interface IImGui
{
    void Begin(string name);
    void Text(string label);
    bool Button(string label);
    void End();

    void DockSpaceOverViewport();
    void ShowDemoWindow();
    void ShowDebugInput();
}