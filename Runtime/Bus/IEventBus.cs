namespace RossoForge.Events.Bus
{
    public interface IEventBus
    {
#if UNITY_EDITOR
        void CheckListeners();
        BusEditorInfo GetBusEditorInfo();
        void Raise(object instance);
#endif
    }
}