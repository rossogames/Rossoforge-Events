using Rossoforge.Core.Events;
using Rossoforge.Events.CatFoodSample.Events;
using Rossoforge.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Rossoforge.Events.CatFoodSample.Components
{
    public class UI : MonoBehaviour,
        IEventListener<CanOpenedEvent>,
        IEventListener<FoodAmountChangedEvent>
    {
        [SerializeField]
        private Button _giveFoodButton;

        [SerializeField]
        private Button _eatButton;

        [SerializeField]
        private Text _foodText;

        private IEventService _eventService;

        void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
            _eventService.RegisterListener<CanOpenedEvent>(this);
            _eventService.RegisterListener<FoodAmountChangedEvent>(this);

            ActiveButtons(false);
            SetFoodAmountLabel(0);
        }
        private void OnDestroy()
        {
            _eventService.UnregisterListener<CanOpenedEvent>(this);
            _eventService.UnregisterListener<FoodAmountChangedEvent>(this);
        }

        private void ActiveButtons(bool existFood)
        {
            _giveFoodButton.gameObject.SetActive(!existFood);
            _eatButton.gameObject.SetActive(existFood);
        }

        private void SetFoodAmountLabel(int amount)
        {
            _foodText.text = amount.ToString();
        }

        public void OnEventInvoked(FoodAmountChangedEvent eventArg)
        {
            ActiveButtons(eventArg.RemainingAmount > 0);
            SetFoodAmountLabel(eventArg.RemainingAmount);
        }

        public void OnEventInvoked(CanOpenedEvent eventArg)
        {
            ActiveButtons(true);
        }
    }
}