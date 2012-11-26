namespace EventSourcing.Sample.Core.Infrastructure
{
    public interface IEventHandler
    {
        bool Handle(object evnt);
    }
}