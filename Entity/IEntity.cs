using System.Collections.Concurrent;

namespace mysite_back_asp.net.Entity
{
    public interface IEntity
    {
        IProducerConsumerCollection<IEvent> Events { get; }
    }
}
