using Rossoforge.Core.Events;

namespace Rossoforge.Events.CatFoodSample.Events
{
    public readonly struct FoodAmountChangedEvent : IEvent
    {
        public readonly int RemainingAmount;

        public FoodAmountChangedEvent(int remainingAmount)
        {
            RemainingAmount = remainingAmount;
        }
    }
}