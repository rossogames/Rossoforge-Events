using Rossoforge.Core.Events;
using Rossoforge.Events.CatFoodSample.Events;
using Rossoforge.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Rossoforge.Events.CatFoodSample.Components
{
    public class FoodCan : MonoBehaviour,
        IEventListener<CanOpenedEvent>,
        IEventListener<FoodAmountChangedEvent>
    {
        private IEventService _eventService;

        [SerializeField]
        private Image _imageFoodCan;

        private void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
            _eventService.RegisterListener<CanOpenedEvent>(this);
            _eventService.RegisterListener<FoodAmountChangedEvent>(this);

            _imageFoodCan.enabled = false;
        }
        private void OnDestroy()
        {
            _eventService.UnregisterListener<CanOpenedEvent>(this);
            _eventService.UnregisterListener<FoodAmountChangedEvent>(this);
        }

        public void OnEventInvoked(CanOpenedEvent eventArg)
        {
            _imageFoodCan.enabled = true;
        }

        public void OnEventInvoked(FoodAmountChangedEvent eventArg)
        {
            _imageFoodCan.enabled = eventArg.RemainingAmount > 0;
        }
    }
}