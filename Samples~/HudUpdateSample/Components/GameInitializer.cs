using RossoForge.Events.Service;
using RossoForge.Services.Locator;
using UnityEngine;

namespace RossoForge.Events.Samples.HpHud
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