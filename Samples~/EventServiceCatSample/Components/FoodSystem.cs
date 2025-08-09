using Rossoforge.Core.Events;
using Rossoforge.Events.CatFoodSample.Events;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Events.CatFoodSample.Components
{
    public class FoodSystem : MonoBehaviour
    {
        private IEventService _eventService;
        private int _foodRemainingAmount;

        public int FoodRemainingAmount
        {
            get
            {
                return _foodRemainingAmount;
            }
            set
            {
                _foodRemainingAmount = value;
                _eventService.Raise(new FoodAmountChangedEvent(_foodRemainingAmount));
            }
        }

        private void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
        }

        public void GiveFood()
        {
            _eventService.Raise<CanOpenedEvent>();
            FoodRemainingAmount += 5;
        }

        public void EatFood()
        {
            FoodRemainingAmount--;
        }
    }
}
