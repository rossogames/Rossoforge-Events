using System.Collections.Generic;
using UnityEngine;

namespace RossoForge.Events.Bus
{
    public class EventBus<T> : ScriptableObject where T : struct, IEvent
    {
        private readonly List<IEventListener<T>> eventListeners = new();

#if UNITY_EDITOR
        //[ReadOnly]
        public List<string> ActiveListeners = new();
#endif 
        public T Value { get; set; }

        //[Button("Raise Event")]
        public void Raise(T value)
        {
            this.Value = value;

            if (eventListeners.Count == 0)
                return;

            var listeners = eventListeners.ToArray(); // clone to avoid error when unregist listener

            for (int i = 0; i < listeners.Length; i++)
            {
                var listener = listeners[i];
                if (listener == null)
                    UnregisterListener(listener);
                else
                    listener.OnEventInvoked(value);
            }
        }
        public void RegisterListener(IEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);

#if UNITY_EDITOR
            RefreshListenerTypes();
#endif
        }

        public void UnregisterListener(IEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);

#if UNITY_EDITOR
            RefreshListenerTypes();
#endif
        }

#if UNITY_EDITOR
        public void CheckListeners()
        {
            foreach (var listener in eventListeners)
                Debug.LogWarning($"The listener {listener.GetType().Name} must be removed from the event bus {typeof(T)}");
        }
        private void RefreshListenerTypes()
        {
            ActiveListeners.Clear();
            foreach (var listener in eventListeners)
            {
                if (listener == null)
                    Debug.LogError($"Listener {typeof(T)} is null");
                else
                    ActiveListeners.Add(listener.ToString());
            }
        }
#endif
    }
}