using Rossoforge.Core.Events;

namespace Rossoforge.Events.Samples.HpHud
{
    public readonly struct DamageEvent : IEvent
    {
        public readonly int Damage;

        public DamageEvent(int damage)
        {
            Damage = damage;
        }
    }
}