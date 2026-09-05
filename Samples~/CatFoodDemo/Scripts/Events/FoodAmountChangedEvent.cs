using Rossoforge.Events.Bus;

namespace Rossoforge.Events.Samples.CatFood
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