using RossoForge.Events.Bus;
using RossoForge.Service;
using UnityEngine;

namespace RossoForge.Events.Service
{
    public abstract class EventRegister<T> : MonoBehaviour where T : struct, IEvent
    {
        [SerializeField] private RegisterMethod _registerMethod;

        private IEventService _eventService;
        private IEventListener<T> _listener;

        private void Awake()
        {
            _eventService = ServiceLocator.Get<IEventService>();

            if (!TryGetComponent(out _listener))
                Debug.LogError($"The object {gameObject.name} doesn't implement the event listener {typeof(T)}");

            if (_registerMethod == RegisterMethod.AwakeDestroy)
                RegisterListener();
        }
        private void OnEnable()
        {
            if (_registerMethod == RegisterMethod.EnableDisable)
                RegisterListener();
        }
        private void OnDisable()
        {
            if (_registerMethod == RegisterMethod.EnableDisable)
                Unregister();
        }
        private void OnDestroy()
        {
            if (_registerMethod == RegisterMethod.AwakeDestroy)
                Unregister();
        }

        private void RegisterListener() => _eventService.RegisterListener(_listener);
        private void Unregister() => _eventService.UnregisterListener(_listener);

        public enum RegisterMethod
        {
            EnableDisable,
            AwakeDestroy
        }
    }
}