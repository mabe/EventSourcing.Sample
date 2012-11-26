using System;

namespace EventSourcing.Sample.Core.Events
{
    public class QuantityChangedForProduct
    {
        public QuantityChangedForProduct(Guid productId, int quantity)
        {
            Quantity = quantity;
            ProductId = productId;
        }

        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
    }
}