using Hypercube.Input;
using Hypercube.Mathematics.Vectors;
using JetBrains.Annotations;

namespace Hypercube.ImGui;

[PublicAPI]
public interface IImGuiController : IImGui
{
    event Action<string>? OnErrorHandled;
    
    void Initialize();
    void Update(float deltaTime);
    void Render();

    void InputFrame();
    void UpdateMousePosition(Vector2Int position);
    void UpdateMouseButtons(MouseButton button, bool state);
    void UpdateMouseScroll(Vector2 offset);
}