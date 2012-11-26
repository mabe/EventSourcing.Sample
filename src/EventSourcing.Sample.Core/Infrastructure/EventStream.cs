using System.Collections.Generic;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public class EventStream : IEventStream
    {
        public EventStream(IEnumerable<object> events, int revision)
        {
            Events = events;
            Revision = revision;
        }

        public IEnumerable<object> Events { get; private set; }
        public int Revision { get; private set; }
    }
}