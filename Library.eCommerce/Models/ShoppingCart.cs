using System;
using System.Collections.Generic;
using System.Linq;
using Library.eCommerce.Services;

namespace Library.eCommerce.Models
{
    public class Cart
    {
        public Dictionary<int, int> Contents { get; set; }

        public Cart()
        {
            Contents = new();
        }

        public bool AddToCart(int productId, int quantity)
        {
            var inventoryItem = ProductServiceProxy.Current.Products.FirstOrDefault(item => item?.Id == productId);

            if (inventoryItem is null)
            {
                Console.WriteLine("Product not found in inventory.");
                return false;
            }

            if (quantity <= 0 || quantity > inventoryItem.Quantity)
            {
                Console.WriteLine("Invalid quantity. Not enough stock.");
                return false;
            }

            inventoryItem.Quantity -= quantity;

            if (Contents.ContainsKey(productId))
            {
                Contents[productId] += quantity;
            }
            else
            {
                Contents.Add(productId, quantity);
            }

            Console.WriteLine($"{quantity} x {inventoryItem.Name} added to cart.");
            return true;
        }

        public string? Summary => $"Cart Items: [{string.Join(", ", Contents.Select(kvp => $"ID {kvp.Key} → Qty {kvp.Value}"))}]";

        public bool RemoveItem(int productId, int quantity)
        {
            if (!Contents.ContainsKey(productId))
            {
                Console.WriteLine("Item not found in cart.");
                return false;
            }

            if (quantity <= 0)
            {
                Console.WriteLine("Quantity must be positive.");
                return false;
            }

            int existing = Contents[productId];

            if (quantity > existing)
            {
                Console.WriteLine($"Cannot remove more than present: {existing} items.");
                return false;
            }

            var product = ProductServiceProxy.Current.GetById(productId);
            if (product == null)
            {
                Console.WriteLine("Error: Corresponding product is null.");
                return false;
            }

            Contents[productId] -= quantity;
            product.Quantity += quantity;
            ProductServiceProxy.Current.AddOrUpdate(product);

            if (Contents[productId] == 0)
            {
                Contents.Remove(productId);
                Console.WriteLine($"Removed item ID {productId} from cart.");
            }

            return true;
        }

        public void ShowCart()
        {
            if (!Contents.Any())
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            Console.WriteLine("===Your Cart===");
            foreach (var (key, value) in Contents)
            {
                var item = ProductServiceProxy.Current.Products.FirstOrDefault(p => p?.Id == key);
                if (item != null)
                {
                    Console.WriteLine($"[ID: {key}] {item.Name} x{value}, ${item.Price}");
                }
            }
        }

        public bool IsEmpty() => !Contents.Any();

        public void Checkout()
        {
            Console.WriteLine("===Receipt===");
            double total = 0.0;

            foreach (var (id, qty) in Contents)
            {
                var item = ProductServiceProxy.Current.Products.FirstOrDefault(p => p?.Id == id);
                if (item != null)
                {
                    Console.WriteLine($"{item.Name} x{qty} = ${item.Price * qty}");
                    total += item.Price * qty;
                }
            }

            var taxRate = 0.07;
            var finalTotal = total * (1 + taxRate);
            Console.WriteLine($"Total (includiong 7% tax): ${finalTotal:F2}");
        }

        public override string ToString() => Summary ?? "No items in cart.";
    }
}
