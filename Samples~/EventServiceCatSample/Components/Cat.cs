using Rossoforge.Core.Events;
using Rossoforge.Events.CatFoodSample.Events;
using Rossoforge.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Rossoforge.Events.CatFoodSample.Components
{
    public class Cat : MonoBehaviour,
        IEventListener<CanOpenedEvent>,
        IEventListener<FoodAmountChangedEvent>
    {
        private IEventService _eventService;

        [SerializeField]
        private Image _imageCat;

        [SerializeField]
        private Material _grayMaterial;

        private void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
            _eventService.RegisterListener<CanOpenedEvent>(this);
            _eventService.RegisterListener<FoodAmountChangedEvent>(this);
        }
        private void OnDestroy()
        {
            _eventService.UnregisterListener<CanOpenedEvent>(this);
            _eventService.UnregisterListener<FoodAmountChangedEvent>(this);
        }

        public void OnEventInvoked(CanOpenedEvent eventArg)
        {
            _imageCat.material = null;
        }

        public void OnEventInvoked(FoodAmountChangedEvent eventArg)
        {
            if (eventArg.RemainingAmount <= 0)
                _imageCat.material = _grayMaterial;
        }
    }
}