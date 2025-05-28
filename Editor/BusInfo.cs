using System;

namespace RossoForge.Events.Editor
{
    public class BusInfo
    {
        public Type EventType;
        public Type[] ListenersType;

        public int ListenerCount => ListenersType?.Length ?? 0;
    }
}
