using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Core
{
    public class Cart
    {
        /// <summary>
        /// Représente un item du caddie
        /// </summary>
        public class CartItem
        {
            public DataServiceClient.PRODUIT Product { get; set; }
            public int Amount { get; set; }
            public decimal Total { get; set; }
            public bool IsConnected { get; set; }

            public CartItem(
                DataServiceClient.PRODUIT prod,
                int amount)
            {
                Product = prod;
                Amount = amount;
                Total = prod.prix * amount;
            }
        }

        public List<CartItem> CartItems { get; set; }
        private decimal total;

        /// <summary>
        /// Constructeur
        /// </summary>
        public Cart()
        {
            this.total = 0;
            this.CartItems = new List<CartItem>();
        }

        /// <summary>
        /// Ajoute un élement dans le panier
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void Add(DataServiceClient.PRODUIT product, int quantity)
        {
            var qry = from CartItem i in this.CartItems
                      where i.Product.id == product.id
                      select i;

            if (qry.Count() > 0)
            {
                CartItem cart = qry.First<CartItem>();
                this.Update(cart, quantity + cart.Amount);
            }
            else
            {
                CartItem item = new CartItem(product, quantity);
                this.CartItems.Add(item);
                this.total += item.Total;
            }
        }

        /// <summary>
        /// Supprime complétement un élément du panier
        /// </summary>
        /// <param name="itemToRemove"></param>
        public void Remove(CartItem itemToRemove)
        {
            decimal toMinus = itemToRemove.Total;
            this.CartItems.Remove(itemToRemove);
            this.total -= toMinus;
        }

        /// <summary>
        /// Met à jour un article du panier
        /// </summary>
        /// <param name="itemToUpdate"></param>
        /// <param name="newQuantity"></param>
        public void Update(CartItem itemToUpdate, int newQuantity)
        {
            decimal prevTotal = itemToUpdate.Total;
            itemToUpdate.Amount = newQuantity;
            itemToUpdate.Total = itemToUpdate.Amount * itemToUpdate.Product.prix;

            decimal d = itemToUpdate.Total - prevTotal;
            this.total += d;
        }
    }
}
