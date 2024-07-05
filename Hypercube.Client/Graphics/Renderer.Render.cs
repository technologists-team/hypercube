using System.Drawing;
using Hypercube.Client.Runtimes.Loop.Event;
using OpenToolkit.Graphics.OpenGL4;

namespace Hypercube.Client.Graphics;

public sealed partial class Renderer
{
    private readonly HashSet<Viewport> _viewports = new();
   
    private readonly float[] _vertices = {
         0.5f,  0.5f, 0.0f,  // top right
         0.5f, -0.5f, 0.0f,  // bottom right
        -0.5f, -0.5f, 0.0f,  // bottom left
        -0.5f,  0.5f, 0.0f   // top left 
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

        _viewports.Add(new Viewport());

        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();
        _vao = GL.GenVertexArray();

        GL.BindVertexArray(_vao);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(float), _indices, BufferUsageHint.StaticDraw);
        
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

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
        GL.ClearColor(Color.Chartreuse);

        foreach (var viewport in _viewports)
        {
            RenderEntities(viewport);
        }

        _baseShader.Use();
        
        GL.BindVertexArray(_vao);
        GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
  
        _windowManager.WindowSwapBuffers(MainWindow);
    }

    private void RenderEntities(Viewport viewport)
    {
        
    }
}