using Hypercube.Graphics.Core.Attributes;
using Hypercube.Graphics.Device;
using Hypercube.Graphics.Types;
using Hypercube.Utilities.Commander;
using Silk.NET.Vulkan;

// Silk redefine for reduce type/namespace collisions
using VkDevice = Silk.NET.Vulkan.Device;

namespace Hypercube.Graphics.Backend.Realisation.Vulkan;

[Backend(BackendType.Vulkan)]
public sealed unsafe class VulkanBackend : IBackend
{
    public Vk Vk { get; private set; } = null!;
    public VkDevice Device { get; private set; }

    private Instance _instance;
    private PhysicalDevice _physicalDevice;
    
    public void Initialize(in GraphicsDeviceSettings settings)
    {
        const int maxStringLen = 128;
        const int maxDevices = 32;
        
        // 1. Access the Vulkan API
        Vk = Vk.GetApi();

        // 2. Create a Vulkan instance

        var appName = stackalloc byte[maxStringLen];
        var appVer = settings.Application.Version;
        Utilities.AsciiToPtr(settings.Application.Name, appName, maxStringLen);
        
        var engineName = stackalloc byte[maxStringLen];
        var engineVer = settings.Engine.Version;
        Utilities.AsciiToPtr(settings.Engine.Name, engineName, maxStringLen);
        
        var appInfo = new ApplicationInfo
        {
            SType = StructureType.ApplicationInfo,
            PApplicationName = appName,
            ApplicationVersion = Vk.MakeVersion(appVer.Major, appVer.Minor, appVer.Minor),
            PEngineName = engineName,
            EngineVersion = Vk.MakeVersion(engineVer.Major, engineVer.Minor, engineVer.Minor),
            ApiVersion = Vk.Version12
        };
        
        var instanceCreateInfo = new InstanceCreateInfo
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo
        };

        if (Vk.CreateInstance(in instanceCreateInfo, null, out _instance) != Result.Success)
            throw new Exception("Failed to create instance");
        
        // 3. Selecting a Physical Device (GPU)
        var physicalDeviceCount = 0u;
        
        if (Vk.EnumeratePhysicalDevices(_instance, &physicalDeviceCount, null) != Result.Success)
            throw new Exception("Failed to enumerate physical devices");

        if (physicalDeviceCount == 0)
            throw new Exception("No devices supporting Vulkan were found.");
        
        if (physicalDeviceCount > maxDevices)
            throw new Exception($"Bro are you serious? WHAT THE FUCKING {physicalDeviceCount}??!");
        
        var physicalDeviceCountSigned = (int) physicalDeviceCount;
        var physicalDevices = stackalloc PhysicalDevice[physicalDeviceCountSigned];
        if (Vk.EnumeratePhysicalDevices(_instance, &physicalDeviceCount, physicalDevices) != Result.Success)
            throw new Exception("Failed to enumerate physical devices");
        
        // For simplicity, we'll choose the first available device. 
        // TODO: evaluating GPU rating based on settings
        _physicalDevice = physicalDevices[0];
        
        // 4. Create a logical device (Device)
        var queueFamilyCount = 0u;
        Vk.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, &queueFamilyCount, null);
        
        var queueFamilyCountSigned = (int) queueFamilyCount;
        var queueFamilies = stackalloc QueueFamilyProperties[queueFamilyCountSigned];
        Vk.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, &queueFamilyCount, queueFamilies);

        // Find family that support graphical operations
        var graphicsQueueFamilyIndex = 0u;
        for (uint i = 0; i < queueFamilyCount; i++)
        {
            if ((queueFamilies[i].QueueFlags & QueueFlags.GraphicsBit) == 0)
                continue;
            
            graphicsQueueFamilyIndex = i;
            break;
        }
        
        var queuePriority = 1f;
        var deviceQueueCreateInfo = new DeviceQueueCreateInfo
        {
            SType = StructureType.DeviceQueueCreateInfo,
            QueueFamilyIndex = graphicsQueueFamilyIndex,
            QueueCount = 1,
            PQueuePriorities = &queuePriority
        };
        
        // TODO: PEnabledExtensionNames
        // TODO: PEnabledLayerNames
        var deviceCreateInfo = new DeviceCreateInfo
        {
            SType = StructureType.DeviceCreateInfo,
            QueueCreateInfoCount = 1,
            PQueueCreateInfos = &deviceQueueCreateInfo
        };
        
        if (Vk.CreateDevice(_physicalDevice, in deviceCreateInfo, null, out var localDevice) != Result.Success)
            throw new Exception("Failed to create device");

        Device = localDevice;
    }

    public void Terminate()
    {
        throw new NotImplementedException();
    }

    public void ExecuteCommands(IUnsafeCommandBuffer commandBuffer)
    {
        throw new NotImplementedException();
    }

    public IBackendFence CreateFence()
    {
        throw new NotImplementedException();
    }
}