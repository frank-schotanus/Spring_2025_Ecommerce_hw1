using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Services;

namespace Library.eCommerce.Models
{
    //here
    public class ShoppingCart
    {
        public Dictionary<int, int> ItemsInCart
        {
            get;
            set;
        }
        public ShoppingCart()
        {
            ItemsInCart = new Dictionary<int, int>();
        }

        public bool AddItem(int itemID, int itemQuantity)
        {
            var product = ProductServiceProxy.Current.Products.FirstOrDefault(P => P.Id == itemID);

            if (product == null)
            {
                Console.WriteLine("This product is not in our inventory.");
                return false;
            }

            if (itemQuantity <= 0 || itemQuantity > product.Quantity)
            {
                Console.WriteLine("There is not enough of the item in stock.");
                return false;
            }

            product.Quantity = product.Quantity - itemQuantity;

            if (ItemsInCart.ContainsKey(itemID))
            {
                ItemsInCart[itemID] = ItemsInCart[itemID] + itemQuantity;
            }
            else
            {
                ItemsInCart[itemID] = itemQuantity;
            }

            Console.WriteLine($"{itemQuantity} of {product.Name} added to the cart.");
            return true;

        }

        public string? Display
        {
            get
            {
                return $"Item ID: {ItemsInCart.Keys}, Item Quantity: {ItemsInCart.Values}";
            }
        }

        public bool removeFromCart(int itemID, int itemQuantity)
        {
            if(ItemsInCart.ContainsKey(itemID) == false)
            {
                Console.WriteLine("The specified item is not in your cart.");
                return false;
            }

            if (itemQuantity <= 0)
            {
                Console.WriteLine("Please enter a positive amount of items.");
                return false;
            }

            if (itemQuantity > ItemsInCart[itemID])
            {
                Console.WriteLine($"You are unable to remove more than {ItemsInCart[itemID]} items.");
                return false;
            }

            var product = ProductServiceProxy.Current.GetById(itemID);
            if(product == null)
            {
                Console.WriteLine("Product appears to be null, ERROR.");
                return false;
            }

            ItemsInCart[itemID] -= itemQuantity;
            product.Quantity += itemQuantity;
            ProductServiceProxy.Current.AddOrUpdate(product);

            if (ItemsInCart[itemID] == 0)
            {
                ItemsInCart.Remove(itemID);
                Console.WriteLine($" Product ID: {itemID} has been removed from the cart.");
            }

            return true;
        }

        public void PrintCart()
        {
            if (ItemsInCart.Any() == false)
            {
                Console.WriteLine("Your cart is empty.");
                return;
            }

            Console.WriteLine("-------Cart Contents-------");

            foreach (var x in ItemsInCart)
            {
                var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == x.Key);
                if (product != null)
                {
                    Console.WriteLine($"ID: {x.Key} \t Name: {product.Name} \t Quantity: {x.Value} \t Price: {product.Price} \t ");
                }

            }
        }

        public bool cartIsEmpty()
        {
            if(ItemsInCart.Any() == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Shoping cart<dictionary> = ID and Quantity
        //ProductServiceProxy = name and prive
        public void printReceipt()
        {
            Console.WriteLine("-----Your Receipt-----");
            double total = 0;
            foreach (var x in ItemsInCart)
            {
                var product = ProductServiceProxy.Current.Products.FirstOrDefault(p => p.Id == x.Key);
                if (product != null)
                {
                    Console.WriteLine($"ID: {x.Key} \t Name: {product.Name} \t Quantity: {x.Value} \t Price: {product.Price} \t ");
                    total += (x.Value * product.Price);
                }
            }

            Console.WriteLine($"Total(sales tax included): {total * 1.07}");
        }
        public override string ToString()
        {
            return Display ?? string.Empty;
        }

    }
}