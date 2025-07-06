using Rossoforge.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rossoforge.Events.Bus
{
    public class EventBus<T> : IEventBus where T : IEvent
    {
        private readonly HashSet<IEventListener<T>> eventListeners = new();
        private readonly object _lock = new();

#if UNITY_EDITOR
        private BusEditorInfo _busEditorInfo;

        public EventBus()
        {
            _busEditorInfo = new BusEditorInfo
            {
                EventBus = this,
                EventType = typeof(T),
                ListenersType = new Type[0],
                EventInstance = Activator.CreateInstance<T>(),
            };
        }
#endif

        public void Raise(T value)
        {
#if UNITY_EDITOR
            Debug.Log($"Event Raised: {typeof(T).Name}");
            _busEditorInfo.Calls++;
#endif

            if (eventListeners.Count == 0)
                return;

            List<IEventListener<T>> listeners = null;
            lock (_lock)
            {
                listeners = eventListeners.ToList();  // clone to avoid error when unregist listener
            }

            foreach (var listener in listeners)
                listener.OnEventInvoked(value);
        }
        public void RegisterListener(IEventListener<T> listener)
        {
            lock (_lock)
            {
                eventListeners.Add(listener);
#if UNITY_EDITOR
                RefreshBusEditorInfo();
#endif
            }
        }

        public void UnregisterListener(IEventListener<T> listener)
        {
            lock (_lock)
            {
                eventListeners.Remove(listener);
#if UNITY_EDITOR
                RefreshBusEditorInfo();
#endif
            }
        }

        public void UnregisterAllListener()
        {
            lock (_lock)
            {
                foreach (var listener in eventListeners)
                    eventListeners.Add(listener);

#if UNITY_EDITOR
                RefreshBusEditorInfo();
#endif
            }
        }

#if UNITY_EDITOR
        public IBusEditorInfo GetBusEditorInfo()
        {
            return _busEditorInfo;
        }
        public void Raise(object instance)
        {
            Raise((T)instance);
        }
        public void CheckListeners()
        {
            foreach (var listener in eventListeners)
                Debug.LogWarning($"The listener {listener.GetType().Name} must be removed from the event bus {typeof(T)}");
        }

        private void RefreshBusEditorInfo()
        {
            List<IEventListener<T>> listeners = null;
            lock (_lock)
            {
                listeners = eventListeners.ToList();
            }

            _busEditorInfo.EventBus = this;
            _busEditorInfo.EventType = typeof(T);
            _busEditorInfo.ListenersType = listeners.Select(l => l.GetType()).ToArray();
        }
#endif
    }
}