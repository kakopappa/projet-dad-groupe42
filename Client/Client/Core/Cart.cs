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
        public bool IsConnected { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public Cart()
        {
            this.total = 0;
            this.CartItems = new List<CartItem>();
        }

        public void Clear()
        {
            this.CartItems.Clear();

            if (IsConnected)
            {
                MainWindow.GetMe().MeinService.RemoveCartContentClient(MainWindow.GetMe().SessionId);
            }
        }

        /// <summary>
        /// Ajoute un élement dans le panier
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public bool Add(DataServiceClient.PRODUIT product, int quantity)
        {
            var qry = from CartItem i in this.CartItems
                      where i.Product.id == product.id
                      select i;

            if (qry.Count() > 0)
            {
                CartItem cart = qry.First<CartItem>();
                if (this.Update(cart, quantity + cart.Amount))
                    return true;
                else
                    return false;
            }
            else
            {
                CartItem item = new CartItem(product, quantity);
                this.CartItems.Add(item);
                this.total += item.Total;

                if (IsConnected)
                {
                    Service.ItemState s = MainWindow.GetMe().MeinService.AddItemInCart(MainWindow.GetMe().SessionId,
                        product.id,
                        quantity);

                    if (s == Service.ItemState.OK)
                        return true;
                    else
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Supprime complétement un élément du panier
        /// </summary>
        /// <param name="itemToRemove"></param>
        public bool Remove(CartItem itemToRemove)
        {
            decimal toMinus = itemToRemove.Total;
            this.CartItems.Remove(itemToRemove);
            this.total -= toMinus;

            if (IsConnected)
            {
                if (MainWindow.GetMe().MeinService.RemoveItemInCart(MainWindow.GetMe().SessionId,
                    itemToRemove.Product.id))
                    return true;
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Met à jour un article du panier
        /// </summary>
        /// <param name="itemToUpdate"></param>
        /// <param name="newQuantity"></param>
        public bool Update(CartItem itemToUpdate, int newQuantity)
        {
            decimal prevTotal = itemToUpdate.Total;
            int prevQuantity = itemToUpdate.Amount;
            itemToUpdate.Amount = newQuantity;
            itemToUpdate.Total = itemToUpdate.Amount * itemToUpdate.Product.prix;

            decimal d = itemToUpdate.Total - prevTotal;
            this.total += d;

            if (IsConnected)
            {
                Service.ItemState state = MainWindow.GetMe().MeinService.AddItemInCart(MainWindow.GetMe().SessionId,
                        itemToUpdate.Product.id,
                        itemToUpdate.Amount - prevQuantity);

                if (state == Service.ItemState.OK)
                    return true;
                else
                    return false;
            }

            return true;
        }
    }
}
