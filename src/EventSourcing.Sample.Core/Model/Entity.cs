using System;
using EventSourcing.Sample.Core.Infrastructure;

namespace EventSourcing.Sample.Core.Model
{
    public abstract class Entity<TAggregate> : IEntity where TAggregate : IAggregate
    {
        protected readonly TAggregate Aggregate;

        protected Entity(Guid id, TAggregate aggregate, IEventHandlerMappingStrategy eventHandlerMappingStrategy)
        {
            Aggregate = aggregate;
            Id = id;
            EventHandlerMappingStrategy = eventHandlerMappingStrategy;

            InitializeEventHandlers();
        }

        public Guid Id { get; private set; }
        protected IEventHandlerMappingStrategy EventHandlerMappingStrategy { get; private set; }

        public void ApplyEvent(object evnt)
        {
            Aggregate.ApplyEvent(evnt);
        }

        protected void AddEventHandler(IEventHandler eventHandler)
        {
            Aggregate.AddEventHandler(eventHandler);
        }

        protected void InitializeEventHandlers()
        {
            foreach (var eventHandler in EventHandlerMappingStrategy.GetEventHandlers(this))
                AddEventHandler(eventHandler);
        }
    }
}