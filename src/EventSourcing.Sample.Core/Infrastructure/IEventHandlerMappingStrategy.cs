using System.Collections.Generic;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public interface IEventHandlerMappingStrategy
    {
        IEnumerable<IEventHandler> GetEventHandlers(object target);
    }
}