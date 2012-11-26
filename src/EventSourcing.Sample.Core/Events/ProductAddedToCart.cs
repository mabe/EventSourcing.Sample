using System;

namespace EventSourcing.Sample.Core.Events
{
    public class ProductAddedToCart
    {
        public ProductAddedToCart(Guid cartId, Guid productId)
        {
            ProductId = productId;
            CartId = cartId;
        }

        public Guid CartId { get; private set; }
        public Guid ProductId { get; private set; }
    }
}