using System;
using EventSourcing.Sample.Core.Model;
using StructureMap;

namespace EventSourcing.Sample.Core.Infrastructure
{
    public class InstantiateAggregate : IInstantiateAggregate
    {
        private readonly IContainer _container;

        public InstantiateAggregate(IContainer container)
        {
            _container = container;
        }

        public T Instantiate<T>(Guid id) where T : IAggregate
        {
            using (var nestedContainer = _container.GetNestedContainer())
            {
                nestedContainer.Configure(x => x.For<Guid>().Use(id));

                return _container.GetInstance<T>();
            }
        }
    }
}