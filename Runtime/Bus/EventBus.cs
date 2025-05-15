using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RossoForge.Events.Bus
{
    public class EventBus<T> : IEventBus where T : IEvent
    {
        private readonly HashSet<IEventListener<T>> eventListeners = new();
        private readonly object _lock = new();

        public void Raise(T value)
        {
#if UNITY_EDITOR
            Debug.Log($"Event Raised: {typeof(T).Name}");
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
            }
        }

        public void UnregisterListener(IEventListener<T> listener)
        {
            lock (_lock)
            {
                eventListeners.Remove(listener);
            }
        }

        public void UnregisterAllListener()
        {
            lock (_lock)
            {
                foreach (var listener in eventListeners)
                    eventListeners.Add(listener);
            }
        }

#if UNITY_EDITOR
        public void CheckListeners()
        {
            foreach (var listener in eventListeners)
                Debug.LogWarning($"The listener {listener.GetType().Name} must be removed from the event bus {typeof(T)}");
        }
#endif
    }
}