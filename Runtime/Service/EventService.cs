using RossoForge.Events.Bus;
using RossoForge.Service;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RossoForge.Events.Service
{
    public class EventService : IEventService, IInitializable, IDisposable
    {
        private readonly Dictionary<Type, object> _busCollection = new();

        public void Initialize()
        {
            LoadEventBus();
        }

        public async void Dispose()
        {
#if UNITY_EDITOR
            await CheckListeners();
#endif
        }

        public void RegisterListener<T>(IEventListener<T> listener) where T : struct, IEvent => GetBus<T>().RegisterListener(listener);
        public void UnregisterListener<T>(IEventListener<T> listener) where T : struct, IEvent => GetBus<T>().UnregisterListener(listener);
        public void Raise<T>() where T : struct, IEvent => GetBus<T>().Raise(default);
        public void Raise<T>(T eventArg) where T : struct, IEvent => GetBus<T>().Raise(eventArg);

        private EventBus<T> GetBus<T>() where T : struct, IEvent => _busCollection[typeof(T)] as EventBus<T>;
        private void LoadEventBus()
        {
            UnityEngine.Object[] busses = Resources.LoadAll("EventBus");
            LoadEventBusDictionary(busses);
        }
        private void LoadEventBusDictionary(UnityEngine.Object[] busses)
        {
            foreach (var bus in busses)
            {
                Type busType = bus.GetType();
                Type busGenericType = busType.BaseType;
                Type eventType = busGenericType.GetGenericArguments()[0];

                _busCollection.Add(eventType, bus);
            }
        }

#if UNITY_EDITOR
        private async Awaitable CheckListeners()
        {
            await Awaitable.WaitForSecondsAsync(1);
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