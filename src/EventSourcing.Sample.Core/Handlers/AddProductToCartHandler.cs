using EventSourcing.Sample.Core.Commands;
using EventSourcing.Sample.Core.Data;
using EventSourcing.Sample.Core.Model;

namespace EventSourcing.Sample.Core.Handlers
{
    public class AddProductToCartHandler
    {
        private readonly IAggregateRepository _repository;

        public AddProductToCartHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AddProductToCart message)
        {
            var cart = _repository.Load<ShoppingCart>(message.CartId);

            cart.AddProduct(message.ProductId);

            _repository.SaveChanges(cart);
        }
    }
}