using Hypercube.Utilities.Attributes;

namespace Hypercube.Windowing.Core.Joystick;

[IdStruct(typeof(int))]
public readonly partial struct JoystickId
{
    public static readonly JoystickId Null = new(-1);
}
