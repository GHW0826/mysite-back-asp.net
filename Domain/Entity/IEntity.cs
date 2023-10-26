using System.Collections.Concurrent;

namespace Domain.Entity;

public interface IEntity
{
    IProducerConsumerCollection<IEvent> Events { get; }
}
