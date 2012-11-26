using System;
using EventSourcing.Sample.Core.Infrastructure;

namespace EventSourcing.Sample.Core.Model
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Revision { get; }

        void ApplyEvent(object evt);
        void BuildFromHistory(IEventStream eventStream);
        void AddEventHandler(IEventHandler eventHandler);
        IEventStream GetUncommittedChanges();
        void ClearUncommittedChanges();
    }
}