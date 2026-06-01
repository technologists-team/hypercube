using System.Diagnostics;
using Hypercube.Graphics.Core.Backends;
using Hypercube.Graphics.Core.Commands.Implementations;

namespace Hypercube.Graphics.Core.Commands;

public static class CommandExecution
{
    public static unsafe void Execute(CommandBuffer buffer, IRenderBackend backend)
    {
        while (buffer.TryGetNext(out var type, out var ptr))
        {
            switch (type)
            {
                case RenderCommandType.Test:
                {
                    backend.Execute(*(TestCommand*) ptr);
                    break;
                }
                
                case RenderCommandType.Clear:
                {
                    var cmd = *(ClearCommand*) ptr;
                    backend.Execute(cmd);
                    break;
                }
     
                case RenderCommandType.Scissor:
                {
                    var cmd = *(ScissorCommand*) ptr;
                    backend.Execute(cmd);
                    break;
                }
                
                case RenderCommandType.RenderTarget:
                    break;
                case RenderCommandType.Matrix:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}