using Silk.NET.Vulkan;

// Silk redefine for reduce type/namespace collisions
using VkDevice = Silk.NET.Vulkan.Device;

namespace Hypercube.Graphics.Backend.Realisation.Vulkan;

public sealed unsafe class VulkanBackendFence : IBackendFence
{
    private readonly VulkanBackend _backend;
    private Fence _fence;
    
    public Vk Vk => _backend.Vk;
    public VkDevice Device { get; private set; }
    
    public VulkanBackendFence(VulkanBackend backend)
    {
        _backend = backend;
    }
    
    public void Reset()
    {
        if (_fence.Handle == 0)
            return;

        Vk.DestroyFence(_backend.Device, _fence, null);
        
        _fence = new Fence();
    }

    public void Wait()
    {
        if (_fence.Handle == 0)
            return;
        
        var result = Vk.WaitForFences(_backend.Device, 1, in _fence, true, ulong.MaxValue);
        if (result == Result.Success)
            return;
        
        throw new Exception($"Failed to wait for fence {_fence.Handle}, result {result}");
    }

    public void Signal()
    {
        if (_fence.Handle != 0)
            return;
        
        var fenceCreateInfo = new FenceCreateInfo
        {
            SType = StructureType.FenceCreateInfo,
            Flags = 0
        };

        var result = Vk.CreateFence(_backend.Device, in fenceCreateInfo, null, out _fence);
        if (result == Result.Success)
            return;
        
        throw new Exception($"Failed to create fence: {result}");
    }

    public void Dispose()
    {
        Reset();
    }
}