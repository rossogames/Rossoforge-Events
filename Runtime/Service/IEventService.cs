using RossoForge.Events.Bus;
using RossoForge.Service;

namespace RossoForge.Events.Service
{
    public interface IEventService : IService
    {
        void RegisterListener<T>(IEventListener<T> listener) where T : IEvent;
        void UnregisterListener<T>(IEventListener<T> listener) where T : IEvent;
        void Raise<T>() where T :  IEvent;
        void Raise<T>(T eventArg) where T : IEvent;
    }
}
