using EventSourcing.Sample.Core.Commands;
using EventSourcing.Sample.Core.Data;
using EventSourcing.Sample.Core.Model;

namespace EventSourcing.Sample.Core.Handlers
{
    public class RemoveProductFromCartHandler
    {
        private readonly IAggregateRepository _repository;

        public RemoveProductFromCartHandler(IAggregateRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RemoveProductFromCart removeProductFromCart)
        {
            var cart = _repository.Load<ShoppingCart>(removeProductFromCart.CartId);

            cart.RemoveProduct(removeProductFromCart.ProductId);

            _repository.SaveChanges(cart);
        }
    }
}