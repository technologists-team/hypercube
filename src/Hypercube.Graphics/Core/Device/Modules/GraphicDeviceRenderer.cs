using Hypercube.Graphics.Backend;
using Hypercube.Graphics.Backend.Commands;
using Hypercube.Graphics.Backend.Commands.Uploading;
using Hypercube.Graphics.Core.Batching;
using Hypercube.Graphics.Core.Types;
using Hypercube.Mathematics;
using Hypercube.Mathematics.Vectors;

namespace Hypercube.Graphics.Core.Device.Modules;

public sealed class GraphicDeviceRenderer : GraphicDeviceModule
{
    private GraphicDeviceBatcher Batcher => Device.Batcher;
    
    public GraphicDeviceRenderer(GraphicDevice device) : base(device)
    {
    }

    public unsafe void Render()
    {
        var flush = Batcher.Flush();
        var state = new RenderState();

        // ДАЛБАЕБ ТАК НЕЛЬЗЯ ДЕЛАТЬ БРО У ТЕБЯ КОМАНДНЫЙ БУФФЕР
        fixed (Vertex* pointer = flush.Vertices)
        {
            Device.Backend.CommandPush(new LowCommandUploadVertices
            {
                Count = flush.Vertices.Length,
                Data = pointer
            }, LowCommandType.UploadVertices);
        }
        
        fixed (uint* pointer = flush.Indices)
        {
            Device.Backend.CommandPush(new LowCommandUploadIndices
            {
                Count = flush.Indices.Length,
                Data = pointer
            }, LowCommandType.UploadIndices);
        }
        
        foreach (var group in flush.Groups)
        {
            if (state != group.State)
            {
                PushState(state,  group.State);
                state = group.State;
            }
            
            Device.Backend.CommandPush(new LowCommandDraw
            {
                Start = group.IndexStart,
                End = group.IndexEnd
            }, LowCommandType.Draw);
        }
    }

    public void PushState(RenderState state, RenderState newState)
    {
        // TODO
    }

    public void DrawRectangle(Vector2 positionA, Vector2 positionB, bool outline, Color color1, Color color2, Color color3, Color color4)
    {
        var left = positionA.X;
        var right = positionB.X;
        var top = positionA.Y;
        var bottom = positionB.Y;
        
        Batcher.State.PrimitiveType = PrimitiveType.TriangleStrip;
        Batcher.AddGeometry(
            [
                new Vertex(new Vector2(left, bottom), new Vector2(0, 1), color1),
                new Vertex(new Vector2(left, top),    new Vector2(0, 1), color2),
                new Vertex(new Vector2(right, top),   new Vector2(0, 1), color3),
                new Vertex(new Vector2(right, bottom),new Vector2(0, 1), color4)
            ], [
                0,
                1,
                3,
                1,
                2,
                3
            ]
        );
    }
}
