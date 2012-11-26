using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Sample.Core.Infrastructure;

namespace EventSourcing.Sample.Core.Model
{
    public abstract class Aggregate : IAggregate
    {
        private readonly IList<object> _uncommittedEvents = new List<object>();
        private readonly IList<IEventHandler> _eventHandlers = new List<IEventHandler>();

        protected Aggregate(Guid id)
            : this(id, new ConventionalEventHandlerMappingStrategy())
        {

        }

        protected Aggregate(Guid id, IEventHandlerMappingStrategy eventHandlerMappingStrategy)
        {
            Id = id;
            EventHandlerMappingStrategy = eventHandlerMappingStrategy;

            InitializeEventHandlers();
        }

        public Guid Id { get; private set; }
        public int Revision { get; private set; }
        protected IEventHandlerMappingStrategy EventHandlerMappingStrategy { get; private set; }

        public virtual void ApplyEvent(object evt)
        {
            _uncommittedEvents.Add(evt);
            HandleEvent(evt);
        }

        public virtual void BuildFromHistory(IEventStream eventStream)
        {
            if (_uncommittedEvents.Count > 0) throw new InvalidOperationException("Cannot apply history when instance has uncommitted changes.");

            foreach (var evnt in eventStream.Events)
                HandleEvent(evnt);

            Revision = eventStream.Revision;
        }

        protected void HandleEvent(object evnt)
        {
            var handlers = new List<IEventHandler>(_eventHandlers);

            handlers.Aggregate(false, (current, handler) => current | handler.Handle(evnt));
        }

        public IEventStream GetUncommittedChanges()
        {
            return new EventStream(new List<object>(_uncommittedEvents), Revision + 1);
        }

        public void ClearUncommittedChanges()
        {
            _uncommittedEvents.Clear();
        }

        public void AddEventHandler(IEventHandler eventHandler)
        {
            _eventHandlers.Add(eventHandler);
        }

        protected void InitializeEventHandlers()
        {
            foreach (var eventHandler in EventHandlerMappingStrategy.GetEventHandlers(this))
                AddEventHandler(eventHandler);
        }
    }
}