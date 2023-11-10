using System.Collections.Concurrent;

namespace Domain.Entity.Common;

public interface IEntity
{
    IProducerConsumerCollection<IEvent> Events { get; }
}
