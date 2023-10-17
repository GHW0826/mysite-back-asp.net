using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace mysite_back_asp.net.Entity
{
    public abstract class Entity : IEntity
    {
        [NotMapped]
        private readonly ConcurrentQueue<IEvent> _events = new ConcurrentQueue<IEvent>();

        [NotMapped]
        public IProducerConsumerCollection<IEvent> Events => _events;

        protected void AddEvent(IEvent @event)
        {
            _events.Enqueue(@event);
        }
    }
}
