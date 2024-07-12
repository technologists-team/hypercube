using Hypercube.Client.Graphics.Shading;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Shared.Math;
using Hypercube.Shared.Math.Box;
using Hypercube.Shared.Math.Matrix;
using Hypercube.Shared.Math.Vector;
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
    
    private readonly Vertex[] _batchVertices = new Vertex[MaxBatchVertices];
    private readonly float[] _batchVerticesRaw = new float[MaxBatchVertices * Vertex.Size];
    private readonly uint[] _batchIndices = new uint[MaxBatchIndices];
    
    private uint _batchVertexIndex;
    private uint _batchIndexIndex; // Haha name it's fun
    
    private BufferObject _vbo = default!;
    private ArrayObject _vao = default!;
    private BufferObject _ebo = default!;
    
    private void OnLoad()
    {
        _baseShader = new Shader("/base", _resourceManager);
        _baseTexture = _textureManager.CreateHandler("/icon.png");
        _baseTexture.Bind();

        _cameraManager.SetMainCamera(_cameraManager.CreateCamera2D(MainWindow.Size));
        
        _vbo = new BufferObject(BufferTarget.ArrayBuffer);
        _ebo = new BufferObject(BufferTarget.ElementArrayBuffer);
        _vao = new ArrayObject();

        BatchUpdate();
        
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

    private void OnFrameUpdate(UpdateFrameEvent args)
    {
#if DEBUG
        _windowManager.WindowSetTitle(MainWindow, $"FPS: {_timing.Fps} | RealTime: {_timing.RealTime} | cPos: {_cameraManager.MainCamera?.Position ?? null} | cRot: {_cameraManager.MainCamera?.Rotation ?? null}");
#endif
        _windowManager.PollEvents();
        _cameraManager.UpdateInput(_cameraManager.MainCamera, args.DeltaSeconds);
    }

    private void OnFrameRender(RenderFrameEvent args)
    {
        BatchClear();
        
        var window = MainWindow;

        GL.Viewport(window.Size);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var sin = (float)Math.Abs(Math.Sin(_timing.RealTime.TotalMilliseconds / 1000f));
        var colorR = new Color(sin, 0f, 0f);
        var colorG = new Color(0f, sin, 0f);
        var colorB = new Color(0f, 0f, sin);
        
        DrawTexture(_baseTexture, _baseTexture.Texture.QuadCrateTranslated(-Vector2.UnitY * 60f), new Box2(0.0f, 1.0f, 1.0f, 0.0f), Color.White);
        DrawTexture(_baseTexture, _baseTexture.Texture.QuadCrateTranslated(Vector2.UnitX * 60f), new Box2(0.0f, 1.0f, 1.0f, 0.0f), colorR);
        DrawTexture(_baseTexture, _baseTexture.Texture.QuadCrateTranslated(-Vector2.UnitX * 60f), new Box2(0.0f, 1.0f, 1.0f, 0.0f), colorG);
        DrawTexture(_baseTexture, _baseTexture.Texture.QuadCrateTranslated(Vector2.UnitY * 60f), new Box2(0.0f, 1.0f, 1.0f, 0.0f), colorB);

        BatchUpdate();

        var model = Matrix4X4.CreateTranslation(Vector2.Zero) * Matrix4X4.CreateRotationZ(0) * Matrix4X4.CreateScale(Vector2.One);
        var view = Matrix4X4.CreateTranslation(0.0f, 0.0f, -3.0f);
        
        _baseShader.Use();
        _baseShader.SetUniform("model", model);
        _baseShader.SetUniform("view", view);
        _baseShader.SetUniform("projection", _cameraManager.Projection);
        
        _vao.Bind();
        GL.DrawElements(BeginMode.Triangles, (int) _batchIndexIndex, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();
  
        _windowManager.WindowSwapBuffers(MainWindow);
    }

    private void RenderEntities(ICamera camera)
    {
        
    }

    private void BatchUpdate()
    {
        _vao.Bind();
        
        BatchConvert();
        
        _vbo.SetData(_batchVerticesRaw);
        _ebo.SetData(_batchIndices);
    }
    
    private void BatchConvert()
    {
        var indexRaw = 0;
        for (var i = 0; i < _batchVertexIndex; i++)
        {
            var vertex = _batchVertices[i];
            foreach (var vertexValue in vertex.ToVertices())
            {
                _batchVerticesRaw[indexRaw++] = vertexValue;
            }
        }
    }
    
    private void BatchClear()
    {
        _batchVertexIndex = 0;
        _batchIndexIndex = 0;
        
        Array.Clear(_batchVertices, 0, _batchVertices.Length);
        Array.Clear(_batchVerticesRaw, 0, _batchVerticesRaw.Length);
        Array.Clear(_batchIndices, 0, _batchIndices.Length);
    }
}