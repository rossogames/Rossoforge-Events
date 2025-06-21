using RossoForge.Core.Events;
using RossoForge.Events.Service;
using RossoForge.Services;
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