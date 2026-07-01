namespace Hypercube.Graphics.Core.Device.Modules;

public sealed class GraphicDeviceStatistic
{
    private readonly GraphicDevice _device;

    public GraphicDeviceStatistic(GraphicDevice device)
    {
        _device = device;
    }

    public int BackendDrawCalls;
    public int RenderingDrawCalls;

    public int LargestBatchVertices;
    public int LargestBatchIndices;

    // Resource allocations
    public int TextureFrameAllocations;
    public int TextureFrameAllocationsLargest;
    public int TextureFrameAllocationsMemory; // in bytes
    public int TextureFrameAllocationsMemoryLargest;

    public void EnsureTexture(int length)
    {
        TextureFrameAllocations++;
        TextureFrameAllocationsMemory += length;
    }

    public void Clear()
    {
        ClearTexture();
    }

    private void ClearTexture()
    {
        TextureFrameAllocationsLargest = int.Max(TextureFrameAllocationsLargest, TextureFrameAllocations);
        TextureFrameAllocationsMemoryLargest = int.Max(TextureFrameAllocationsMemoryLargest, TextureFrameAllocationsMemory);;
        
        TextureFrameAllocations = 0;
        TextureFrameAllocationsMemory = 0;
    }
}
