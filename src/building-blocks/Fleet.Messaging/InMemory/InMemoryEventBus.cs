using System.Collections.Concurrent;
using Fleet.Messaging.Abstractions;

namespace Fleet.Messaging.InMemory
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _handlers = new();
        private readonly ConcurrentBag<object> _events = new();

        public Task PublishAsync<T>(T @event) where T : class
        {
            _events.Add(@event!);
            if (_handlers.TryGetValue(typeof(T), out var list))
            {
                var tasks = list.Select(h => h(@event!));
                return Task.WhenAll(tasks);
            }
            return Task.CompletedTask;
        }

        public void Publish<T>(T @event) where T : class
        {
            _events.Add(@event!);
            if (_handlers.TryGetValue(typeof(T), out var list))
            {
                foreach (var h in list)
                {
                    _ = h(@event!);
                }
            }
        }

        public void Subscribe<T>(Func<T, Task> handler) where T : class
        {
            var list = _handlers.GetOrAdd(typeof(T), _ => new List<Func<object, Task>>());
            Func<object, Task> wrapper = (o) => handler((T)o);
            list.Add(wrapper);
        }

        public IReadOnlyCollection<object> PublishedEvents => _events.ToArray();
    }
}
