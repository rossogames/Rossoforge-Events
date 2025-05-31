using System;

namespace RossoForge.Events.Bus
{
    public interface IEventBus
    {
#if UNITY_EDITOR
        int Calls { get; }
        void CheckListeners();
        Type[] GetListenersType();
#endif
    }
}