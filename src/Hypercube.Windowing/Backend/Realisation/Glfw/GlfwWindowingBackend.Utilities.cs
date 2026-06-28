using Hypercube.Windowing.Core.Monitors;
using Hypercube.Windowing.Core.Windows;
using Hypercube.Windowing.Input;
using Hypercube.Windowing.Types;

// Silk redefine for reduce type/namespace collisions
using SilkMonitor = Silk.NET.GLFW.Monitor;
using SilkWindow = Silk.NET.GLFW.WindowHandle;
using SilkErrorCode = Silk.NET.GLFW.ErrorCode;
using SilkConnected = Silk.NET.GLFW.ConnectedState;
using SilkKeys = Silk.NET.GLFW.Keys;
using SilkKeyModifiers = Silk.NET.GLFW.KeyModifiers;
using SilkInputAction = Silk.NET.GLFW.InputAction;
using SilkMouseButton = Silk.NET.GLFW.MouseButton;

namespace Hypercube.Windowing.Backend.Realisation.Glfw;

public sealed unsafe partial class GlfwBackend
{
    private static MonitorHandle Translate(SilkMonitor* monitor)
    {
        return new MonitorHandle((nint) monitor);
    }
 
    private static WindowHandle Translate(SilkWindow* window)
    {
        return new WindowHandle((nint) window);
    }

    private static Key Translate(SilkKeys keys)
    {
        // Both types provide same values from ASCII table
        return (Key) keys;
    }
    
    private static MouseButton Translate(SilkMouseButton button)
    {
        return button switch
        {
            SilkMouseButton.Left    => MouseButton.Left,
            SilkMouseButton.Right   => MouseButton.Right,
            SilkMouseButton.Middle  => MouseButton.Middle,
            SilkMouseButton.Button4 => MouseButton.Button4,
            SilkMouseButton.Button5 => MouseButton.Button5,
            SilkMouseButton.Button6 => MouseButton.Button6,
            SilkMouseButton.Button7 => MouseButton.Button7,
            SilkMouseButton.Button8 => MouseButton.Button8,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button, null)
        };
    }

    private static KeyState Translate(SilkInputAction action)
    {
        return action switch
        {
            SilkInputAction.Press   => KeyState.Pressed,
            SilkInputAction.Release => KeyState.Released,
            SilkInputAction.Repeat  => KeyState.Held,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }

    private static KeyModifiers Translate(SilkKeyModifiers modifiers)
    {
        return modifiers switch
        {
            SilkKeyModifiers.Shift   => KeyModifiers.Shift,
            SilkKeyModifiers.Control => KeyModifiers.Control,
            SilkKeyModifiers.Alt     => KeyModifiers.Alt,
            SilkKeyModifiers.Super   => KeyModifiers.Super,
            _ => throw new ArgumentOutOfRangeException(nameof(modifiers), modifiers, null)
        };
    }

    private static ErrorCode Translate(SilkErrorCode code)
    {
        return code switch
        {
            SilkErrorCode.NoError            => ErrorCode.NoError,
            SilkErrorCode.NotInitialized     => ErrorCode.NotInitialized,
            SilkErrorCode.NoContext          => ErrorCode.NoContext,
            SilkErrorCode.InvalidEnum        => ErrorCode.InvalidEnum,
            SilkErrorCode.InvalidValue       => ErrorCode.InvalidValue,
            SilkErrorCode.OutOfMemory        => ErrorCode.OutOfMemory,
            SilkErrorCode.ApiUnavailable     => ErrorCode.ApiUnavailable,
            SilkErrorCode.VersionUnavailable => ErrorCode.VersionUnavailable,
            SilkErrorCode.PlatformError      => ErrorCode.PlatformError,
            SilkErrorCode.FormatUnavailable  => ErrorCode.FormatUnavailable,
            SilkErrorCode.NoWindowContext    => ErrorCode.NoWindowContext,
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, null)
        };
    }

    private static ConnectedState Translate(SilkConnected state)
    {
        return state switch
        {
            SilkConnected.Connected    => ConnectedState.Connected,
            SilkConnected.Disconnected => ConnectedState.Disconnected,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
    }
}
