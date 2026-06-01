using Hypercube.Graphics.Core;
using Hypercube.Graphics.Core.Backends;
using Hypercube.Graphics.Core.Commands.Implementations;
using Hypercube.Graphics.Core.Context;
using Silk.NET.OpenGL;

namespace Hypercube.Graphics.Backends.OpenGl;

public sealed class OpenGLBackend : IRenderBackend
{
    private GL? _gl;
    private IContextListener _listener = null!;
    
    public void Initialize(IGraphicsContext context)
    {
        _listener = context.Listener;
        
        _gl = GL.GetApi(context.Info.GetProcAddress);
        
        _gl.Enable(EnableCap.DebugOutput);
        _gl.Enable(EnableCap.DebugOutputSynchronous);
    }

    public void Execute(in TestCommand cmd)
    {
        _listener.Test();
    }

    public void Execute(in ScissorCommand cmd)
    {
        if (_gl is null)
        {
            _listener.Throw();
            return;
        }

        if (!cmd.Enabled)
        {
            _gl.Disable(EnableCap.ScissorTest);
            return;
        }

        _gl.Enable(EnableCap.ScissorTest);
        _gl.Scissor(cmd.Scissor.Left, cmd.Scissor.Top, (uint) cmd.Scissor.Width, (uint) cmd.Scissor.Height);
    }

    public void Execute(in ClearCommand cmd)
    {
        if (_gl is null)
        {
            _listener.Throw();
            return;
        }

        var vecColor = cmd.Color.Vec4;
        
        _gl.ClearColor(vecColor.X, vecColor.Y, vecColor.Z, vecColor.W);
        _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }
}
