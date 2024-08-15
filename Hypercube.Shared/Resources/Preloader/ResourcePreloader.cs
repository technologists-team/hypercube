using System.Reflection;
using Hypercube.Dependencies;
using Hypercube.Shared.EventBus;
using Hypercube.Shared.EventBus.Events;
using Hypercube.Utilities.Helpers;

namespace Hypercube.Shared.Resources.Preloader;

public sealed class ResourcePreloader : IResourcePreloader, IEventSubscriber, IPostInject
{
    [Dependency] private readonly IEventBus _eventBus = default!;
    
    private readonly HashSet<PreloadInfo> _preloadInfos;
    
    public ResourcePreloader()
    {
        var preloadInfos = new HashSet<PreloadInfo>();
        foreach (var preload in GetPreloads())
        {
            if (preload.Event is null)
            {
                preload.Invoke();
                continue;
            }

            preloadInfos.Add(preload);
        }

        _preloadInfos = preloadInfos;
    }
    
    public void PostInject()
    {
        foreach (var preloadInfo in _preloadInfos)
        {
            if (preloadInfo.Event is null)
                continue;
            
            var method = GetType().GetMethod(nameof(Subscribe), BindingFlags.NonPublic | BindingFlags.Instance);
            var generic = method?.MakeGenericMethod(preloadInfo.Event) ?? throw new InvalidOperationException();
            generic.Invoke(this, new object?[] { preloadInfo });
        }
    }

    private void Subscribe<T>(PreloadInfo info)
        where T : IEventArgs
    {
        _eventBus.Subscribe(this, (ref T _) =>
        {
            info.Invoke();

            _preloadInfos.Remove(info);
            _eventBus.Unsubscribe<T>(this);
        });
    }

    private HashSet<PreloadInfo> GetPreloads()
    {
        var result = new HashSet<PreloadInfo>();
        foreach (var type in ReflectionHelper.GetAllInstantiableSubclassOf(typeof(IPreloader)))
        {
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            if (constructors.Length == 0)
                throw new InvalidOperationException($"Attribute {nameof(PreloadingAttribute)} can't be applied to {type.Name} without public constructors.");
            
            var constructor = constructors[0];
            if (constructor.GetParameters().Length != 0)
                throw new InvalidOperationException($"Attribute {nameof(PreloadingAttribute)} can't be applied to {type.Name} with public constructor accepting more than 0 arguments.");
            
            var instance = constructors[0].Invoke(null);
            DependencyManager.Inject(instance);

            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attribute = ReflectionHelper.GetAttribute<PreloadingAttribute>(method);
                if (attribute is null)
                    continue;
                
                result.Add(new PreloadInfo(instance, method, attribute.Event));
            }
        }

        return result;
    }

    private readonly record struct PreloadInfo(object Instance, MethodInfo MethodInfo, Type? Event)
    {
        public void Invoke()
        {
            MethodInfo.Invoke(Instance, null);
        }
    }
}