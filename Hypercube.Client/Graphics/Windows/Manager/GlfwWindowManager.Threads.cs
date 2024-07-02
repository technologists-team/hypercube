using System.Threading.Channels;
using Hypercube.Shared.Utilities;

namespace Hypercube.Client.Graphics.Windows.Manager;

public sealed unsafe partial class GlfwWindowManager
{
    private readonly SimpleCallbackContainer<Cmd> _cmdContainer = new();
    
    private ChannelReader<Cmd> _cmdReader = default!;
    private ChannelWriter<Cmd> _cmdWriter = default!;
    
    private void InitChannels()
    {
        var cmdChannel = Channel.CreateUnbounded<Cmd>(new UnboundedChannelOptions
        {
            SingleReader = true
        });

        _cmdReader = cmdChannel;
        _cmdWriter = cmdChannel;
        
        // Register cmds
        _cmdContainer.Register<CmdCreateWindow>(CrateWindowThreadHandle);
    }

    private void SendCmd(Cmd cmd)
    {
        _cmdWriter.TryWrite(cmd);
    }

    private void ProcessCmd(Cmd cmd)
    {
        _cmdContainer.Invoke(cmd);
    }
    
    private abstract record Cmd;

    private record CmdTerminateWindow : Cmd;
    
    private record CmdCreateWindow(ContextInfo? Context, WindowCreateSettings Settings,
        WindowRegistration? ContextShare) : Cmd;
}