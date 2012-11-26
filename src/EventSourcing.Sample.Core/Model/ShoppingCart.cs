using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EventSourcing.Sample.Core.Events;

namespace EventSourcing.Sample.Core.Model
{
    public class ShoppingCart : Aggregate
    {
        private readonly ICollection<Guid> _products = new Collection<Guid>();

        public ShoppingCart(Guid id) : base(id)
        {
        }

        public void AddProduct(Guid productId)
        {
            if(_products.Contains(productId))
                throw new InvalidOperationException("You can't add a product twice to the same shoppingcart.");

            ApplyEvent(new ProductAddedToCart(Id, productId));
        }

        public void RemoveProduct(Guid productId)
        {
            if(!_products.Contains(productId))
                throw new InvalidOperationException(string.Format("You are trying to remove a product with id: {0} from cart with id: {1}. This product was never added to this cart.", Id, productId));

            ApplyEvent(new ProductRemovedFromCart(Id, productId));
        }

        protected void OnProductAddedToCart(ProductAddedToCart productAddedToCart)
        {
            _products.Add(productAddedToCart.ProductId);
        }

        protected void OnProductRemovedFromCart(ProductRemovedFromCart productRemovedFromCart)
        {
            _products.Remove(productRemovedFromCart.ProductId);
        }
    }
}