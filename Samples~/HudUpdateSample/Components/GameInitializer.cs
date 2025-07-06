using Rossoforge.Core.Events;
using Rossoforge.Events.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Events.Samples.HpHud
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());
            ServiceLocator.Register<IEventService>(new EventService());
            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IEventService>();
        }
    }
}