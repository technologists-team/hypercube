using Hypercube.Graphics.Rendering.Api.Handlers;
using Hypercube.Graphics.Rendering.Api.Settings;
using Hypercube.Graphics.Rendering.Batching;
using Hypercube.Graphics.Rendering.Shaders;
using Hypercube.Graphics.Windowing;

namespace Hypercube.Graphics.Rendering.Api;

public interface IRenderingApi
{
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
    
    void Init(IContextInfo context, RenderingApiSettings settings);
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
}