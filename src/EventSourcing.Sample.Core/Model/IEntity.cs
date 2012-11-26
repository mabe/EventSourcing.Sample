using System;

namespace EventSourcing.Sample.Core.Model
{
    public interface IEntity
    {
        Guid Id { get; }
        void ApplyEvent(object evnt);
    }
}