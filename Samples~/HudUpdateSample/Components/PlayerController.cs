using Rossoforge.Core.Events;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Events.Samples.HpHud
{
    public class PlayerController : MonoBehaviour
    {
        private IEventService _eventService;

        [SerializeField]
        private int _atk;

        private void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
        }

        public void ApplyDamage()
        {
            _eventService.Raise<DamageEvent>(new DamageEvent(_atk));
        }
    }
}