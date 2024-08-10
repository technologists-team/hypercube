namespace Hypercube.Client.Graphics.ImGui;

public interface IImGui
{
    void Begin(string name);
    void Text(string label);
    bool Button(string label);
    void End();
}