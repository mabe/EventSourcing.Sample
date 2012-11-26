using System;
using EventSourcing.Sample.Core.Model;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public interface IInstantiateAggregate
    {
        T Instantiate<T>(Guid id) where T : IAggregate;
    }
}