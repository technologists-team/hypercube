using Hypercube.Graphics.Rendering.Api;
using Hypercube.Graphics.Rendering.Api.Realisations.Headless;
using Hypercube.Graphics.Rendering.Api.Realisations.OpenGl;
using Hypercube.Graphics.Windowing.Api;
using Hypercube.Graphics.Windowing.Api.Realisations.Glfw;
using Hypercube.Graphics.Windowing.Api.Realisations.Headless;

namespace Hypercube.Graphics;

public static class ApiFactory
{
    public static IWindowingApi Get(WindowingApi windowingApi)
    {
        return windowingApi switch
        {
            WindowingApi.Headless => new HeadlessWindowingApi(),
            WindowingApi.Glfw => new GlfwWindowingApi(),
            WindowingApi.Sdl => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };
    }
    
    public static IRenderingApi Get(RenderingApi renderingApi)
    {
        return renderingApi switch
        {
            RenderingApi.Headless => new HeadlessRenderingApi(),
            RenderingApi.OpenGl => new OpenGlRenderingApi(),
            RenderingApi.Vulkan => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };
    }
}