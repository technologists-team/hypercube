using Hypercube.Client.Graphics.Event;
using Hypercube.Client.Graphics.Shading;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Texturing.TextureSettings;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Runtimes.Loop.Event;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Rendering;

public sealed partial class Renderer
{
    private IShader _baseShader = default!;
    private ITextureHandle _baseTexture = default!;
    
    private const int MaxBatchVertices = 65532;
    private const int IndicesPerVertex = 6;
    private const int MaxBatchIndices = MaxBatchVertices * IndicesPerVertex;

    private readonly List<Batch> _batches = new();
    private readonly Vertex[] _batchVertices = new Vertex[MaxBatchVertices];
    private readonly uint[] _batchIndices = new uint[MaxBatchIndices];
    
    private int _batchVertexIndex;
    private int _batchIndexIndex; // Haha name it's fun
    
    private BufferObject _vbo = default!;
    private ArrayObject _vao = default!;
    private BufferObject _ebo = default!;
    
    private void OnLoad()
    {
        _baseShader = new Shader("/base", _resourceManager);
        _baseTexture = _textureManager.GetHandler("/icon.png", new Texture2DCreationSettings());
        _baseTexture.Bind();

        _cameraManager.SetMainCamera(_cameraManager.CreateCamera2D(MainWindow.Size));
        
        _vbo = new BufferObject(BufferTarget.ArrayBuffer);
        _ebo = new BufferObject(BufferTarget.ElementArrayBuffer);
        _vao = new ArrayObject();
        
        _vao.Bind();
        _vbo.SetData(_batchVertices);
        _ebo.SetData(_batchIndices);
        
        // aPos
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        // aColor
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        // aTexCoords
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.Size * sizeof(float), 7 * sizeof(float)); 
        GL.EnableVertexAttribArray(2);
        
        _logger.EngineInfo("Loaded");
    }

    private void OnFrameUpdate(ref UpdateFrameEvent args)
    {
#if DEBUG
        _windowManager.WindowSetTitle(MainWindow, $"FPS: {_timing.Fps} | RealTime: {_timing.RealTime} | cPos: {_cameraManager.MainCamera?.Position ?? null} | cRot: {_cameraManager.MainCamera?.Rotation ?? null}");
#endif
        _windowManager.PollEvents();
        _cameraManager.UpdateInput(_cameraManager.MainCamera, args.DeltaSeconds);
    }

    private void OnFrameRender(ref RenderFrameEvent args)
    {
        Render(MainWindow);
    }

    public void Render(WindowRegistration window)
    {
        Clear();

        GL.Viewport(window.Size);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var args = new RenderDrawingEvent();
        _eventBus.Raise(ref args);
        
        _vao.Bind();
        _vbo.SetData(_batchVertices);
        _ebo.SetData(_batchIndices);
        
        foreach (var batch in _batches)
        {
            Render(batch);
        }
        
        _vao.Unbind();
        _windowManager.WindowSwapBuffers(window);
    }
    
    public void Clear()
    {
        Array.Clear(_batchVertices, 0, _batchVertexIndex);
        Array.Clear(_batchIndices, 0, _batchIndexIndex);
        
        _batchVertexIndex = 0;
        _batchIndexIndex = 0;
        
        _batches.Clear();
    }

    private void Render(Batch batch)
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, batch.TextureHandle);
        
        _baseShader.Use();
        
        _baseShader.SetUniform("model", batch.Model);
        _baseShader.SetUniform("view", Matrix4X4.Identity);
        _baseShader.SetUniform("projection", _cameraManager.Projection);

        GL.DrawElements(batch.PrimitiveType, batch.Size, DrawElementsType.UnsignedInt, batch.Start * sizeof(uint));
        
        _baseShader.Stop();
    }
}