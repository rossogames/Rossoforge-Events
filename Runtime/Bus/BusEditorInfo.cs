using System;

namespace Rossoforge.Events.Bus
{
    public class BusEditorInfo : IBusEditorInfo
    {
        public IEventBus EventBus { get; set; }
        public int Calls { get; set; }

        public Type EventType { get; set; }
        public Type[] ListenersType { get; set; }
        public object EventInstance { get; set; }

        public int ListenerCount => ListenersType?.Length ?? 0;
    }
}
