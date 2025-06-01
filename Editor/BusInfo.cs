using RossoForge.Events.Bus;
using System;

namespace RossoForge.Events.Editor
{
    public class BusInfo
    {
        public IEventBus EventBus;
        public int Calls;

        public Type EventType;
        public Type[] ListenersType;

        public int ListenerCount => ListenersType?.Length ?? 0;
    }
}
