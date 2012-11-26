using System;

namespace EventSourcing.Sample.Core.Commands
{
    public class AddProductToCart
    {
        public AddProductToCart(Guid cartId, Guid productId)
        {
            ProductId = productId;
            CartId = cartId;
        }

        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
    }
}