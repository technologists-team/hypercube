using System.Runtime.InteropServices;

namespace Hypercube.Shared.EventBus;

public readonly struct Unit;

[StructLayout(LayoutKind.Sequential)]
public sealed class UnitBox
{
    public Unit Value;
}