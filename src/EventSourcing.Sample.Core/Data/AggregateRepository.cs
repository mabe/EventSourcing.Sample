using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing.Sample.Core.Infrastructure;
using EventSourcing.Sample.Core.Model;
using EventStore;
using IEventStream = EventStore.IEventStream;

namespace EventSourcing.Sample.Core.Data
{
    public class AggregateRepository : IAggregateRepository
    {
        private readonly IInstantiateAggregate _instantiateAggregate;
        private readonly IStoreEvents _storeEvents;
        private readonly IConflictDetector _conflictDetector;

        private readonly IDictionary<Guid, IEventStream> _streams = new Dictionary<Guid, IEventStream>();

        public AggregateRepository(IInstantiateAggregate instantiateAggregate, IStoreEvents storeEvents, IConflictDetector conflictDetector)
        {
            _instantiateAggregate = instantiateAggregate;
            _conflictDetector = conflictDetector;
            _storeEvents = storeEvents;
        }

        public TAggregate Load<TAggregate>(Guid id)
            where TAggregate : class, IAggregate
        {
            var stream = OpenStream(id, int.MaxValue);
            var aggregate = BuildAggregate<TAggregate>(stream);

            return aggregate;
        }

        public void SaveChanges(IAggregate aggregate)
        {
            var uncommitedChanges = aggregate.GetUncommittedChanges();

            if (!uncommitedChanges.Events.Any()) return;

            while (true)
            {
                var stream = OpenStream(aggregate.Id, int.MaxValue);
                var commitEventCount = stream.CommittedEvents.Count;

                foreach (var @event in uncommitedChanges.Events)
                {
                    var evnt = @event;

                    var eventMessage = new EventMessage
                    {
                        Body = evnt
                    };

                    stream.Add(eventMessage);
                }

                try
                {
                    stream.CommitChanges(Guid.NewGuid());
                    aggregate.ClearUncommittedChanges();
                    return;
                }
                catch (DuplicateCommitException)
                {
                    stream.ClearChanges();
                    return;
                }
                catch (ConcurrencyException ex)
                {
                    if (ThrowOnConflict(stream, commitEventCount))
                        throw new ConflictingCommandException(ex.Message, ex);

                    stream.ClearChanges();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            lock (_streams)
            {
                foreach (var stream in _streams)
                    stream.Value.Dispose();

                _streams.Clear();
            }
        }

        private IEventStream OpenStream(Guid id, int version)
        {
            IEventStream stream;
            if (_streams.TryGetValue(id, out stream))
                return stream;

            _storeEvents.OpenStream(id, 0, version);

            return _streams[id] = stream;
        }

        private TAggregate BuildAggregate<TAggregate>(IEventStream eventStream) where TAggregate : IAggregate
        {
            var aggregate = _instantiateAggregate.Instantiate<TAggregate>(eventStream.StreamId);

            aggregate.BuildFromHistory(new EventStream(eventStream.CommittedEvents.Select(x => x.Body), eventStream.StreamRevision));

            return aggregate;
        }

        private bool ThrowOnConflict(IEventStream stream, int skip)
        {
            var committed = stream.CommittedEvents.Skip(skip).Select(x => x.Body);
            var uncommitted = stream.UncommittedEvents.Select(x => x.Body);
            return _conflictDetector.HasConflicts(uncommitted, committed);
        }
    }
}