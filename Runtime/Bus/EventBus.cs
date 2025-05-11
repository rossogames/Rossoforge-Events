using System.Collections.Generic;
using UnityEngine;

namespace RossoForge.Events.Bus
{
    public class EventBus<T> where T : struct, IEvent
    {
        private readonly HashSet<IEventListener<T>> eventListeners = new();

        public void Raise(T value)
        {
#if UNITY_EDITOR
            Debug.Log($"Event Raised: {typeof(T).Name}");
#endif

            if (eventListeners.Count == 0)
                return;

            var listeners = new HashSet<IEventListener<T>>(eventListeners); // clone to avoid error when unregist listener
            foreach (var listener in listeners)
            {
                listener.OnEventInvoked(value);
            }
        }
        public void RegisterListener(IEventListener<T> listener)
        {
            eventListeners.Add(listener);
        }

        public void UnregisterListener(IEventListener<T> listener)
        {
            eventListeners.Remove(listener);
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