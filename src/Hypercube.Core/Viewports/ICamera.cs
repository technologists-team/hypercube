using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Quaternions;

namespace Hypercube.Core.Viewports;

/// <summary>
/// Represents a camera used for rendering a viewport.
/// Provides projection and view matrices, as well as transform and clipping settings.
/// </summary>
public interface ICamera
{
    /// <summary>
    /// Gets the projection matrix used to transform world-space coordinates into clip space.
    /// </summary>
    Matrix4x4 Projection { get; }

    /// <summary>
    /// Gets the view matrix used to transform world-space coordinates into camera-space.
    /// </summary>
    Matrix4x4 View { get; }

    /// <summary>
    /// Gets or sets the viewport size associated with this camera.
    /// Typically used to calculate the projection matrix.
    /// </summary>
    Vector2i Size { get; set; }

    /// <summary>
    /// Gets or sets the world-space position of the camera.
    /// </summary>
    Vector3 Position { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the camera as a quaternion.
    /// </summary>
    Quaternion Rotation { get; set; }

    /// <summary>
    /// Gets or sets the scale of the camera transform.
    /// </summary>
    Vector3 Scale { get; set; }

    /// <summary>
    /// Gets or sets the far clipping plane distance.
    /// Objects farther than this distance will not be rendered.
    /// </summary>
    float ZFar { get; set; }

    /// <summary>
    /// Gets or sets the near clipping plane distance.
    /// Objects closer than this distance will not be rendered.
    /// </summary>
    float ZNear { get; set; }

    Vector3 ScreenToWorld(Vector2i mousePosition);
}
