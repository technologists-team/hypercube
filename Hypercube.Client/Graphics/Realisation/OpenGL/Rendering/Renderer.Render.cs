using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Events;
using Hypercube.Graphics.Shaders;
using Hypercube.Graphics.Windowing;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Matrices;
using Hypercube.OpenGL.Objects;
using Hypercube.OpenGL.Utilities.Helpers;
using Hypercube.Runtime.Events;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public sealed partial class Renderer
{
    private IShaderProgram _primitiveShaderProgram = default!;
    private IShaderProgram _texturingShaderProgram = default!;

    private const int MaxBatchVertices = 65532;
    private const int IndicesPerVertex = 6;
    private const int MaxBatchIndices = MaxBatchVertices * IndicesPerVertex;

    private readonly List<Batch> _batches = new();
    private readonly Vertex[] _batchVertices = new Vertex[MaxBatchVertices];
    private readonly uint[] _batchIndices = new uint[MaxBatchIndices];

    private int _batchVertexIndex;
    private int _batchIndexIndex; // Haha name it's fun

    /// <summary>
    /// Contains info about currently running batch.
    /// </summary>
    private BatchData? _currentBatchData;

    private BufferObject _vbo = default!;
    private ArrayObject _vao = default!;
    private BufferObject _ebo = default!;

    private void OnLoad()
    {
        _primitiveShaderProgram = _resourceContainer.GetResource<ShaderSourceResource>("/Shaders/base_primitive")
            .ShaderProgram;
        _texturingShaderProgram = _resourceContainer.GetResource<ShaderSourceResource>("/Shaders/base_texturing")
            .ShaderProgram;

        _vao = new ArrayObject();
        _vbo = new BufferObject(BufferTarget.ArrayBuffer);
        _ebo = new BufferObject(BufferTarget.ElementArrayBuffer);

        _vao.Bind();
        _vbo.SetData(_batchVertices);
        _ebo.SetData(_batchIndices);

        // aPos
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // aColor
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float),
            3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        // aTexCoords
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float),
            7 * sizeof(float));
        GL.EnableVertexAttribArray(2);

        _logger.EngineInfo("Loaded");
    }

    private void OnFrameUpdate(ref UpdateFrameEvent args)
    {
#if DEBUG
        var cameraTitle = string.Empty;
        if (_cameraManager.MainCamera is not null)
        {
            cameraTitle =
                $"| cPos: {_cameraManager.MainCamera.Position}| cRot: {_cameraManager.MainCamera.Rotation * HyperMathF.RadiansToDegrees} | cScale: {_cameraManager.MainCamera.Scale}";
        }

        _windowing.WindowSetTitle(MainWindow,
            $"FPS: {_timing.Fps} | RealTime: {_timing.RealTime} {cameraTitle} | Batches: {_batches.Count}");
#endif
        _windowing.PollEvents();
    }

    private void OnFrameRender(ref RenderFrameEvent args)
    {
        Render(MainWindow);
    }

    public void SetupRender()
    {
        GL.Enable(EnableCap.Blend);
        
        GL.BlendEquation(BlendEquationMode.FuncAdd);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
   
        GL.Disable(EnableCap.ScissorTest);
        
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        
        GL.ClearColor(0, 0, 0, 0);
    }

    public void Render(WindowHandle window)
    {
        Clear();

        GL.Viewport(window.Size);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        var ev = new RenderDrawingEvent();
        _eventBus.Raise(ref ev);

        // break batch so we get all batches
        BreakCurrentBatch();
        SetupRender();
        
        _vao.Bind();
        _vbo.SetData(_batchVertices);
        _ebo.SetData(_batchIndices);

        foreach (var batch in _batches)
        {
            Render(batch);
        }

        _vao.Unbind();
        _vbo.Unbind();
        _ebo.Unbind();
        
        var evUI = new RenderAfterDrawingEvent();
        _eventBus.Raise(ref evUI);
        
        _windowing.WindowSwapBuffers(window);
    }
    
    public void Clear()
    {
        Array.Clear(_batchVertices, 0, _batchVertexIndex);
        Array.Clear(_batchIndices, 0, _batchIndexIndex);

        _batchVertexIndex = 0;
        _batchIndexIndex = 0;
        _currentBatchData = null;

        _batches.Clear();
    }

    private void Render(Batch batch)
    {
        var shader = _primitiveShaderProgram;
        if (batch.TextureHandle is not null)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, batch.TextureHandle.Value);
            shader = _texturingShaderProgram;
        }

        shader.Use();
        shader.SetUniform("model", batch.Model);
        shader.SetUniform("view", _cameraManager.View);
        shader.SetUniform("projection", _cameraManager.Projection);

        GL.DrawElements(batch.PrimitiveType, batch.Size, DrawElementsType.UnsignedInt, batch.Start * sizeof(uint));

        shader.Stop();
        GLHelper.UnbindTexture(TextureTarget.Texture2D);
    }

    /// <summary>
    /// Preserves the batches data to allow multiple primitives to be rendered in one batch,
    /// note that for this to work, all current parameters must match a past call to <see cref="EnsureBatch"/>.
    /// Use this instead of directly adding the batches, and it will probably reduce their number.
    /// </summary>
    private void EnsureBatch(PrimitiveType primitiveType, int? textureHandle, int shaderInstance)
    {
        if (_currentBatchData is not null)
        {
            // It's just similar batch,
            // we need changing nothing to render different things
            if (_currentBatchData.Value.Equals(primitiveType, textureHandle, shaderInstance))
                return;

            // Creating a real batch
            GenerateBatch();
        }

        _currentBatchData = new BatchData(primitiveType, _batchIndexIndex, textureHandle, shaderInstance);
    }

    /// <summary>
    /// In case we need to get current batch, or start new one
    /// </summary>
    private void BreakCurrentBatch()
    {
        if (_currentBatchData is null)
            return;

        GenerateBatch();
        _currentBatchData = null;
    }

    private void GenerateBatch()
    {
        if (_currentBatchData is null)
            throw new NullReferenceException();

        var data = _currentBatchData.Value;
        var currentIndex = _batchIndexIndex;

        var batch = new Batch(data.StartIndex, currentIndex - data.StartIndex, data.TextureHandle, data.PrimitiveType,
            Matrix4X4.Identity);

        _batches.Add(batch);
    }
}