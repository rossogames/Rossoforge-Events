using RossoForge.Events.Bus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RossoForge.Events.Service
{
    public class EventService : IEventService, IDisposable
    {
        private readonly Dictionary<Type, object> _busCollection = new();

        public void RegisterListener<T>(IEventListener<T> listener) where T : struct, IEvent => GetBus<T>().RegisterListener(listener);
        public void UnregisterListener<T>(IEventListener<T> listener) where T : struct, IEvent => GetBus<T>().UnregisterListener(listener);
        public void Raise<T>() where T : struct, IEvent => Raise<T>(default);
        public void Raise<T>(T eventArg) where T : struct, IEvent => GetBus<T>().Raise(eventArg);

        public async void Dispose()
        {
#if UNITY_EDITOR
            await CheckListeners();
#endif
        }

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

#if UNITY_EDITOR
        private async Task CheckListeners()
        {
            await Task.Delay(1000);
            foreach (var item in _busCollection)
            {
                var eventBus = _busCollection[item.Key];
                var checkFunction = eventBus.GetType().GetMethod("CheckListeners");

                checkFunction.Invoke(eventBus, null);
            }
        }
#endif
    }
}