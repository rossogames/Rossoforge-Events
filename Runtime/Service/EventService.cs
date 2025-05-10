using RossoForge.Events.Bus;
using System;
using System.Collections.Generic;

namespace RossoForge.Events.Service
{
    public class EventService : IEventService
    {
        private readonly Dictionary<Type, object> _busCollection = new();

        public void RegisterListener<T>(IEventListener<T> listener) where T : struct, IEvent => GetBus<T>().RegisterListener(listener);
        public void UnregisterListener<T>(IEventListener<T> listener) where T : struct, IEvent => GetBus<T>().UnregisterListener(listener);
        public void Raise<T>() where T : struct, IEvent => Raise<T>(default);
        public void Raise<T>(T eventArg) where T : struct, IEvent => GetBus<T>().Raise(eventArg);

        private EventBus<T> GetBus<T>() where T : struct, IEvent
        {
            if (!_busCollection.TryGetValue(typeof(T), out var bus))
            {
                var newBus = new EventBus<T>();
                _busCollection[typeof(T)] = newBus;
                return newBus;
            }

            return bus as EventBus<T>;
        }
    }
}