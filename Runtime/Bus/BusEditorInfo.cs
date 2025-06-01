using RossoForge.Events.Bus;
using System;

namespace RossoForge.Events
{
    public class BusEditorInfo
    {
        public IEventBus EventBus;
        public int Calls;

        public Type EventType;
        public Type[] ListenersType;
        public object EventInstance;

        public int ListenerCount => ListenersType?.Length ?? 0;
    }
}
