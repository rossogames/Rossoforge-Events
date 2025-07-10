# Rosso Games

<table>
  <tr>
    <td><img src="https://github.com/rossogames/Rossoforge-Events/blob/master/logo.png?raw=true" alt="Rossoforge" width="64"/></td>
    <td><h2>Rossoforge - Events</h2></td>
  </tr>
</table>

**Rossoforge-Events** is a lightweight and decoupled event system for Unity, built around generic interfaces and event buses. It allows different parts of your application to communicate through events without tight coupling or dependencies between components.

The following dependencies must be installed
* [[Rossoforge-core]](https://github.com/rossogames/Rossoforge-Core.git)
* [Rossoforge-Services](https://github.com/rossogames/Rossoforge-Services.git) (Opcional)

Watch the tutorial on [Pending]
#
```csharp
    // 1. Define your event
    public readonly struct PlayerDamagedEvent : IEvent
    {
        public readonly int Damage;

        public PlayerDamagedEvent(int damage)
        {
            Damage = damage;
        }
    }

    // 2. Register the listener
    public class DamageLogger : MonoBehaviour, IEventListener<PlayerDamagedEvent>
    {
        private IEventService _eventService;

        void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
            _eventService.RegisterListener(this);
        }
        private void OnDestroy()
        {
            _eventService.UnregisterListener(this);
        }

        public void OnEventInvoked(PlayerDamagedEvent eventArg)
        {
            Debug.Log($"Player took {eventArg.Damage} damage.");
        }
    }

    // 3. Raise the event
    public class GameLogic : MonoBehaviour
    {
        private IEventService _eventService;

        void Start()
        {
            _eventService = ServiceLocator.Get<IEventService>();
        }

        public void DoSomething()
        {
            // Raise event
            _eventService.Raise(new PlayerDamagedEvent(10));
        }
    }
```
#
This package is part of the **Rossoforge** suite, designed to streamline and enhance Unity development workflows.

Developed by Agustin Rosso
https://www.linkedin.com/in/rossoagustin/
