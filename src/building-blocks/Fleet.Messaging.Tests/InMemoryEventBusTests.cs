using System.Threading.Tasks;
using Fleet.Messaging.InMemory;
using Xunit;

namespace Fleet.Messaging.Tests
{
    public class InMemoryEventBusTests
    {
        private record TestEvent(string Message);

        [Fact]
        public async Task PublishAsync_invokes_subscribed_handler()
        {
            var bus = new InMemoryEventBus();
            TestEvent? received = null;
            bus.Subscribe<TestEvent>(e =>
            {
                received = e;
                return Task.CompletedTask;
            });

            await bus.PublishAsync(new TestEvent("hello"));

            Assert.NotNull(received);
            Assert.Equal("hello", received!.Message);
        }
    }
}
