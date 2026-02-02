namespace Fleet.Messaging.Abstractions
{
    public interface IEventHandler<T>
    {
        Task HandleAsync(T @event);
    }
}
