namespace RossoForge.Events.Bus
{
    public interface IEventListener<T> where T : IEvent
    {
        void OnEventInvoked(T eventArg);
    }
}