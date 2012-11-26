using System;
using EventSourcing.Sample.Core.Model;

namespace EventSourcing.Sample.Core.Data
{
    public interface IAggregateRepository : IDisposable
    {
        T Load<T>(Guid id) where T : class, IAggregate;
        void SaveChanges(IAggregate aggregate);
    }
}
