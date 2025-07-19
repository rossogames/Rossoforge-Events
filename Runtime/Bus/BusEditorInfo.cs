using Rossoforge.Core.Events;
using System;

namespace Rossoforge.Events
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
