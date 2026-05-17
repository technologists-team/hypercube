using Hypercube.Core.Windowing.Manager;
using Hypercube.Utilities.Dependencies;

namespace Hypercube.Core.Viewports;

[UsedImplicitly]
public class CameraManager : ICameraManager, IPostInject
{
    [Dependency] private readonly IWindowingManager _windowingManager = default!;

    public event Action? OnMainCameraResized;
    
    // Just for test
    public ICamera MainCamera { get; } = Camera.Default;

    public void OnPostInject()
    {
        // TODO: Implement window scale affect to all camera projections...
        // Need to add cache created windows in IWindowManager & IWindowingApi 

        _windowingManager.OnMainWindowResized += MainWindowingResized;
    }

    private void MainWindowingResized(Vector2i size)
    {
        MainCamera.Size = size;
        OnMainCameraResized?.Invoke();
    }
}
