using System.Collections.Generic;
using StructureMap;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public class ConflictDetector : IConflictDetector
    {
        private readonly IContainer _container;

        public ConflictDetector(IContainer container)
        {
            _container = container;
        }

        public bool HasConflicts(IEnumerable<object> uncommitted, IEnumerable<object> committed)
        {
            foreach (var evnt in uncommitted)
            {
                var confliceDetector = _container.GetInstance(typeof(IConflictWith<>).MakeGenericType(evnt.GetType()));

                if (confliceDetector == null) continue;

                if ((bool)confliceDetector.GetType().GetMethod("HasConflict", new[]
                                                           {
                                                               evnt.GetType(),
                                                               typeof(IEnumerable<object>)
                                                           }).Invoke(confliceDetector, new[]
                                                                                           {
                                                                                               evnt,
                                                                                               committed
                                                                                           }))
                    return true;
            }

            return false;
        }
    }
}