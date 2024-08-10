namespace Hypercube.Shared.Network;

public abstract class Buffer
{
    protected byte[] Data;

    public byte[] GetData()
    {
        return Data;
    }
}