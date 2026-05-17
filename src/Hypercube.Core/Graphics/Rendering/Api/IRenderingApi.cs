using Hypercube.Core.Graphics.Rendering.Api.Handlers;
using Hypercube.Core.Graphics.Rendering.Batching;
using Hypercube.Core.Graphics.Rendering.Shaders;
using Hypercube.Core.Viewports;
using Hypercube.Core.Windowing.Api;
using Hypercube.Core.Windowing.Windows;
using Hypercube.Mathematics.Matrices;
using Hypercube.Mathematics.Shapes;
using InitHandler = Hypercube.Core.Graphics.Rendering.Api.Handlers.InitHandler;
using TextureHandle = Hypercube.Core.Graphics.Objects.Texturing.TextureHandle;

namespace Hypercube.Core.Graphics.Rendering.Api;

public interface IRenderingApi
{
    RenderingApi Type { get; }
    
    event InitHandler? OnInit;
    event DrawHandler? OnDraw;
    
    /// <summary>
    /// Occurs when debug information is requested (typically through a debug overlay or console).
    /// </summary>
    event DebugInfoHandler? OnDebugInfo;

    IShaderProgram? PrimitiveShaderProgram { get; }
    IShaderProgram? TexturingShaderProgram { get; }
    
    int BatchVerticesIndex { get; }
    int BatchIndicesIndex { get; }
    
    int BatchCount { get; }
    int VerticesCount { get; }
    
    void Init(IContextInfoProvider context);
    void Load();
    
    void Terminate();
    void Render(IWindow window);
    
    /// <summary>
    /// Preserves the batches data to allow multiple primitives to be rendered in one batch,
    /// note that for this to work, all current parameters must match a past call to <see cref="EnsureBatch"/>.
    /// Use this instead of directly adding the batches, and it will probably reduce their number.
    /// </summary>
    void EnsureBatch(PrimitiveTopology topology, uint shader, uint? texture);
    
    /// <summary>
    /// In case we need to get current batch, or start new one
    /// </summary>
    void BreakCurrentBatch();

    void PushVertex(Vertex vertex);
    void PushIndex(uint start, uint offset);
    void PushIndex(int start, int index);
    
    /// <summary>
    /// Creates a shader program from a single source file.
    /// </summary>
    /// <param name="source">
    /// The shader source code following Hypercube's .shd specification.
    /// </param>
    /// <returns>
    /// A compiled and linked shader program ready for rendering
    /// </returns>
    IShaderProgram CreateShaderProgram(string source);
    
    TextureHandle CreateTexture(int width, int height, int channels, byte[] data);
    void DeleteTexture(TextureHandle handle);
    
    void Scissor(bool value);
    void SetScissorRect(Rect2i rect);
    
    void SetRenderState(Matrix4x4 view, Matrix4x4 projection);
    void SetRenderState(ICameraManager cameraManager);
    void SetRenderView(Matrix4x4 view);
    void SetRenderProjection(Matrix4x4 projection);
    RenderState GetCurrentRenderState();
    
    /// <summary>
    /// Internal method to get render state by ID. Used by RenderContext.
    /// </summary>
    RenderState GetRenderState(RenderStateId id);
    
    /// <summary>
    /// Internal method to get current render state ID. Used by RenderContext.
    /// </summary>
    RenderStateId GetCurrentRenderStateId();
}
