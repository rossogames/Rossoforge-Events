using RossoForge.Events.Bus;
using RossoForge.Events.Service;
using RossoForge.Service.Locator;
using UnityEngine;
using UnityEngine.UI;

namespace RossoForge.Events.Samples.HpHud
{
    [RequireComponent(typeof(Text))]
    public class HpLabel : MonoBehaviour, IEventListener<DamageEvent>
    {
        private IEventService _eventService;

        [SerializeField]
        private int _hp;

        private Text _label;

        private void Awake()
        {
            _label = GetComponent<Text>();
        }
        void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
            _eventService.RegisterListener(this);

            RefreshHP();
        }
        private void OnDestroy()
        {
            _eventService.UnregisterListener(this);
        }

        public void OnEventInvoked(DamageEvent eventArg)
        {
            _hp -= eventArg.Damage;
            RefreshHP();
        }

        private void RefreshHP()
        {
            _label.text = $"HP: {_hp}";
        }
    }
}