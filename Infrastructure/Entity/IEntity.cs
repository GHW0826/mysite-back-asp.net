using System.Collections.Concurrent;

namespace Infrastructure.Entity;

public interface IEntity
{
    IProducerConsumerCollection<IEvent> Events { get; }
}
