using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class CartServiceProxy
    {
        private CartServiceProxy()
        {
            CartItems = new List<CartItem>();
        }

        private static CartServiceProxy? instance;
        private static object instanceLock = new object();
        public static CartServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CartServiceProxy();
                    }
                }

                return instance;
            }
        }

        public List<CartItem> CartItems { get; private set; }

        public CartItem? AddToCart(Product product, int quantity)
        {
            if (product == null || quantity <= 0 || product.Quantity < quantity)
                return null;

            // Update product quantity
            product.Quantity -= quantity;

            // Update or add cart item
            var existingItem = CartItems.FirstOrDefault(c => c.Product?.Id == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                return existingItem;
            }
            else
            {
                var newItem = new CartItem
                {
                    Id = CartItems.Count + 1,
                    Product = product,
                    Quantity = quantity
                };

                CartItems.Add(newItem);
                return newItem;
            }
        }

        public bool UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            var cartItem = CartItems.FirstOrDefault(c => c.Id == cartItemId);
            if (cartItem == null) return false;

            if (newQuantity <= 0)
            {
                // Return all items to inventory and remove from cart
                if (cartItem.Product != null)
                {
                    cartItem.Product.Quantity += cartItem.Quantity;
                }
                CartItems.Remove(cartItem);
                return true;
            }

            int quantityDifference = newQuantity - cartItem.Quantity;

            // Need to take more from inventory
            if (quantityDifference > 0)
            {
                if (cartItem.Product?.Quantity < quantityDifference)
                    return false; // Not enough in inventory

                if (cartItem.Product != null)
                {
                    cartItem.Product.Quantity -= quantityDifference;
                }
                cartItem.Quantity = newQuantity;
            }
            // Return some to inventory
            else if (quantityDifference < 0)
            {
                if (cartItem.Product != null)
                {
                    cartItem.Product.Quantity += Math.Abs(quantityDifference);
                }
                cartItem.Quantity = newQuantity;
            }

            return true;
        }

        public bool RemoveFromCart(int cartItemId)
        {
            var cartItem = CartItems.FirstOrDefault(c => c.Id == cartItemId);
            if (cartItem == null) return false;

            // Return items to inventory
            if (cartItem.Product != null)
            {
                cartItem.Product.Quantity += cartItem.Quantity;
            }

            CartItems.Remove(cartItem);
            return true;
        }

        public decimal GetSubtotal()
        {
            return CartItems.Sum(item => item.ItemTotal);
        }

        public decimal GetTax(decimal rate = 0.07m)
        {
            return GetSubtotal() * rate;
        }

        public decimal GetTotal(decimal taxRate = 0.07m)
        {
            return GetSubtotal() + GetTax(taxRate);
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }
    }
}