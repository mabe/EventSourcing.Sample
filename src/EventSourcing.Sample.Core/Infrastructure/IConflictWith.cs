using System.Collections.Generic;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public interface IConflictWith<in TEvent>
    {
        bool HasConflict(TEvent evnt, IEnumerable<object> committed);
    }
}