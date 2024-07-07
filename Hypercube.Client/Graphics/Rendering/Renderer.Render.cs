using System.Drawing;
using System.Text;
using Hypercube.Client.Graphics.Shading;
using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Viewports;
using Hypercube.Shared.Runtimes.Loop.Event;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics.Rendering;

public sealed partial class Renderer
{
    private readonly HashSet<Viewport> _viewports = new();
    
    private IShader _baseShader = default!;
    private ITextureHandle _baseTexture = default!;
    
    private readonly float[] _vertices = {
         // positions         // colors (rgba)         // texture coords
         0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 1.0f, 1.0f,  1.0f, 1.0f,   // top right
         0.5f, -0.5f, 0.0f,   1.0f, 1.0f, 1.0f, 1.0f,  1.0f, 0.0f,   // bottom right
        -0.5f, -0.5f, 0.0f,   1.0f, 1.0f, 1.0f, 1.0f,  0.0f, 0.0f,   // bottom left
        -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 1.0f, 1.0f,  0.0f, 1.0f    // top left 
    };
    
    private readonly uint[] _indices = {  // note that we start from 0!
        0, 1, 3, // first triangle
        1, 2, 3  // second triangle
    };  

    private int _vbo;
    private int _vao;
    private int _ebo;
    
    private void OnLoad()
    {
        _baseShader = new Shader("Resources/Shaders/base");
        _baseTexture = _textureManager.CreateHandler("Resources/Textures/opengl_logo.png");
        
        _viewports.Add(new Viewport());

        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();
        _vao = GL.GenVertexArray();

        GL.BindTexture(TextureTarget.Texture2D, _baseTexture.Handle);
        GL.BindVertexArray(_vao);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(float), _indices, BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        
        GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 9 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);
        
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 9 * sizeof(float), 7 * sizeof(float)); 
        GL.EnableVertexAttribArray(2);
        
        _logger.EngineInfo("Loaded");
    }

    private void OnFrameUpdate(UpdateFrameEvent args)
    {
        _windowManager.PollEvents();
    }

    private void OnFrameRender(RenderFrameEvent args)
    {
        var window = MainWindow;

        GL.Viewport(window.Size);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        GL.Enable(EnableCap.Blend);
        GL.ClearColor(0, 0, 0, 0);

        foreach (var viewport in _viewports)
        {
            RenderEntities(viewport);
        }

        _baseShader.Use();
        //_baseShader.SetUniform("uTexture", _baseTexture.Handle);
        
        GL.BindVertexArray(_vao);
        GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
  
        _windowManager.WindowSwapBuffers(MainWindow);
    }

    private void RenderEntities(Viewport viewport)
    {
        
    }
}