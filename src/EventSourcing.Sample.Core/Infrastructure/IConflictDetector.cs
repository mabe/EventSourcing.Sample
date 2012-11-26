using System.Collections.Generic;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public interface IConflictDetector
    {
        bool HasConflicts(IEnumerable<object> uncommitted, IEnumerable<object> committed);
    }
}