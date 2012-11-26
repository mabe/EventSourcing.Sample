using System;

namespace EventSourcing.Sample.Core.Events
{
    public class ProductRemovedFromCart
    {
        public ProductRemovedFromCart(Guid cartId, Guid productId)
        {
            ProductId = productId;
            CartId = cartId;
        }

        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
    }
}