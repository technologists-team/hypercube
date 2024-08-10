namespace Hypercube.ImGui;

public interface IImGuiController
{
    event Action<string>? OnErrorHandled;
    
    void Initialize();
    void Update(float deltaTime);
    void Render();

    void Begin(string name);
    void End();
}