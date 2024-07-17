using Hypercube.Client.Graphics.Drawing;
using Hypercube.Client.Graphics.Events;
using Hypercube.Client.Graphics.Shaders;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Windows;
using Hypercube.Client.Resources.Caching;
using Hypercube.Math;
using Hypercube.Math.Matrixs;
using Hypercube.Shared.Runtimes.Loop.Event;
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
    
    private Matrix4X4 _view = Matrix4X4.CreateScale(32, 32, 1);
    
    private int _batchVertexIndex;
    private int _batchIndexIndex; // Haha name it's fun
    
    private BufferObject _vbo = default!;
    private ArrayObject _vao = default!;
    private BufferObject _ebo = default!;
    
    private void OnLoad()
    {
        _primitiveShaderProgram = _resourceCacher.GetResource<ShaderSourceResource>("/Shaders/base_primitive").ShaderProgram;
        _texturingShaderProgram = _resourceCacher.GetResource<ShaderSourceResource>("/Shaders/base_texturing").ShaderProgram;

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
        var cameraTitle = string.Empty;
        if (_cameraManager.MainCamera is not null)
        {
            cameraTitle = $"| cPos: {_cameraManager.MainCamera.Position} | cRot: {_cameraManager.MainCamera.Rotation * HyperMathF.RadiansToDegrees}";
        }
        
        _windowManager.WindowSetTitle(MainWindow, $"FPS: {_timing.Fps} | RealTime: {_timing.RealTime} {cameraTitle}");
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
        var shader = _primitiveShaderProgram;
        if (batch.TextureHandle is not null)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, batch.TextureHandle.Value);
            shader = _texturingShaderProgram;
        }
        
        shader.Use();
        shader.SetUniform("model", batch.Model);
        shader.SetUniform("view", _view);
        shader.SetUniform("projection", _cameraManager.Projection);

        GL.DrawElements(batch.PrimitiveType, batch.Size, DrawElementsType.UnsignedInt, batch.Start * sizeof(uint));
        
        shader.Stop();
    }
}