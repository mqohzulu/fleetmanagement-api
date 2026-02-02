namespace Fleet.Messaging.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event) where T : class;
        void Publish<T>(T @event) where T : class;
    }
}
