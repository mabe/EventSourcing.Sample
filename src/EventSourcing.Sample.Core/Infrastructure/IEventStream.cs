using System.Collections.Generic;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public interface IEventStream
    {
        IEnumerable<object> Events { get; }
        int Revision { get; }
    }
}