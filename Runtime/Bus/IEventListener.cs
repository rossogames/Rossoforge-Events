namespace RossoForge.Events.Bus
{
    public interface IEventListener<T> where T : struct, IEvent
    {
        void OnEventInvoked(T eventArg);
    }
}