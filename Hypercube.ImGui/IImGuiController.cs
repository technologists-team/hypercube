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
    void UpdateMousePosition(Vector2i position);
    void UpdateKey(Key key, KeyState state, KeyModifiers modifiers);
    void UpdateMouseButtons(MouseButton button, KeyState state, KeyModifiers modifiers);
    void UpdateMouseScroll(Vector2 offset);
    void UpdateInputCharacter(char character);
}