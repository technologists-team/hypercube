using Hypercube.Graphics.Windowing.Settings;
using Silk.NET.GLFW;
using ConnectedState = Hypercube.Graphics.Windowing.Api.Enums.ConnectedState;
using SilkConnectedState = Silk.NET.GLFW.ConnectedState;
using ContextApi = Hypercube.Graphics.Windowing.Settings.ContextApi;

namespace Hypercube.Graphics.Windowing.Api.Realisations.Glfw;

public sealed partial class GlfwWindowingApi
{
    private static ClientApi ToClientApi(ContextApi api)
    {
        return api switch
        {
            ContextApi.None => ClientApi.NoApi,
            ContextApi.Vulkan => ClientApi.NoApi,
            ContextApi.OpenGl => ClientApi.OpenGL,
            ContextApi.OpenGles => ClientApi.OpenGLES,
            _ => throw new ArgumentOutOfRangeException(nameof(api), api, null)
        };
    }

    private static OpenGlProfile ToGlProfile(ContextProfile profile)
    {
        return profile switch
        {
            ContextProfile.Compatability => OpenGlProfile.Compat,
            ContextProfile.Core => OpenGlProfile.Core,
            _ => throw new ArgumentOutOfRangeException(nameof(profile), profile, null)
        };
    }

    private static ConnectedState FromConnectedState(SilkConnectedState state)
    {
        return state switch
        {
            SilkConnectedState.Connected => ConnectedState.Connected,
            SilkConnectedState.Disconnected => ConnectedState.Disconnected,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}