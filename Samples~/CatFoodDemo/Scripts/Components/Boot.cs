using Rossoforge.Events.Service;
using Rossoforge.Services;
using UnityEngine;

namespace Rossoforge.Events.Samples.CatFood
{
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.SetLocator(new DefaultServiceLocator());

            var eventService = new EventService();

            ServiceLocator.Register<IEventService>(eventService);

            ServiceLocator.Initialize();
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.Unregister<IEventService>();
        }
    }
}