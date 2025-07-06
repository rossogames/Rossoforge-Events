using Rossoforge.Core.Events;
using Rossoforge.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rossoforge.Events.Service
{
    public class EventService : IEventService, IDisposable
    {
        private readonly Dictionary<Type, IEventBus> _busCollection = new();

        public void RegisterListener<T>(IEventListener<T> listener) where T : IEvent => GetBus<T>().RegisterListener(listener);
        public void UnregisterListener<T>(IEventListener<T> listener) where T : IEvent => GetBus<T>().UnregisterListener(listener);
        public void Raise<T>() where T : IEvent => Raise<T>(default);
        public void Raise<T>(T eventArg) where T : IEvent => GetBus<T>().Raise(eventArg);

        public async void Dispose()
        {
#if UNITY_EDITOR
            await CheckListeners();
#endif
        }

#if UNITY_EDITOR
        public IEventBus[] GetAllBuses()
        {
            return _busCollection.Values.ToArray();
        }
#endif

        private EventBus<T> GetBus<T>() where T : IEvent
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
                var eventBus = _busCollection[item.Key] as IEventBus;
                eventBus?.CheckListeners();
            }
        }
#endif
    }
}