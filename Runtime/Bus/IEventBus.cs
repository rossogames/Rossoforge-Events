using System;

namespace RossoForge.Events.Bus
{
    public interface IEventBus
    {
#if UNITY_EDITOR
        void CheckListeners();
        Type[] GetListenersType();
#endif
    }
}