using Hypercube.Windowing.Backend;
using Hypercube.Windowing.Core.Backend.Interaction.Commands;
using Hypercube.Windowing.Core.Backend.Interaction.Events;
using Hypercube.Windowing.Core.Windows;

namespace Hypercube.Windowing.Core.Backend;

public sealed class BackendProxy
{
   public event Action<IEvent>? OnEvent;
   
   private readonly IBackend _backend;

   public BackendProxy(IBackend backend)
   {
      _backend = backend;
      
      _backend.OnWindowClose += window =>
         OnEvent?.Invoke(new EventWindowClose(window));
      
      _backend.OnWindowFocus += (window, focused) =>
         OnEvent?.Invoke(new EventWindowFocus(window, focused));
      
      _backend.OnWindowPosition += (window, position) =>
         OnEvent?.Invoke(new EventWindowPosition(window, position));
      
      _backend.OnWindowSize += (window, size) =>
         OnEvent?.Invoke(new EventWindowSize(window, size));
   }

   public void Execute(ICommand raw)
   {
      switch (raw)
      {
         case CommandInitialize:
            _backend.Initialize();
            break;
         
         case CommandTerminate:
            _backend.Terminate();
            break;
         
         case CommandWindowDestroy cmd:
            _backend.WindowDestroy(cmd.Window);
            break;
         
         case CommandWindowSetTitle cmd:
            _backend.WindowSetTitle(cmd.Window, cmd.Title);
            break;
         
         case CommandWindowSetPosition cmd:
            _backend.WindowSetPosition(cmd.Window, cmd.Position);
            break;
         
         case CommandWindowSetSize cmd:
            _backend.WindowSetSize(cmd.Window, cmd.Size);
            break;
         
         case CommandWindowSetIcon cmd:
            _backend.WindowSetIcon(cmd.Window, cmd.Icon);
            break;
      }
   }

   public WindowHandle Execute(ICommand<WindowHandle> raw)
   {
      switch (raw)
      {
         case CommandWindowCreate cmd:
            return _backend.WindowCreate(cmd.Settings);
      }
      
      throw new ArgumentOutOfRangeException();
   }

   public void PollEvents()
   {
      _backend.PollEvents();
   }

   public void WaitEvents()
   {
      _backend.WaitEvents();
   }

   public void PostEmptyEvent()
   {
      _backend.PostEmptyEvent();
   }
   
   public nint GetProcAddress(string name)
   {
      return _backend.GetProcAddress(name);
   }
}
