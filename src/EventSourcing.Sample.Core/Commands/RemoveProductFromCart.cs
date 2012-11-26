using System;

namespace EventSourcing.Sample.Core.Commands
{
    public class RemoveProductFromCart
    {
        public RemoveProductFromCart(Guid cartId, Guid productId)
        {
            ProductId = productId;
            CartId = cartId;
        }

        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
    }
}