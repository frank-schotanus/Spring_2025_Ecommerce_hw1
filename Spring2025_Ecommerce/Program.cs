using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Amazon!");

            ShowOptions();

            var inventoryList = ProductServiceProxy.Current.Products;
            var userCart = new Cart();

            char userInput;

            do
            {
                string? rawInput = Console.ReadLine();
                userInput = char.ToLowerInvariant(rawInput?[0] ?? ' ');

                switch (userInput)
                {
                    case 'c':
                        Console.WriteLine("Provide product name, price, and stock (newline separated):");
                        var newProduct = new Product
                        {
                            Name = Console.ReadLine(),
                            Price = Convert.ToDouble(Console.ReadLine()),
                            Quantity = Convert.ToInt32(Console.ReadLine())
                        };
                        ProductServiceProxy.Current.AddOrUpdate(newProduct);
                        break;

                    case 'r':
                        inventoryList.ForEach(Console.WriteLine);
                        break;

                    case 'u':
                        inventoryList.ForEach(Console.WriteLine);
                        Console.WriteLine("Enter ID of product to modify:");
                        int idToEdit = int.Parse(Console.ReadLine() ?? "-1");
                        var item = inventoryList.FirstOrDefault(p => p.Id == idToEdit);

                        if (item != null)
                        {
                            Console.WriteLine("Select field to update:\n1. Name\n2. Price\n3. Quantity");
                            int field = int.Parse(Console.ReadLine() ?? "0");

                            switch (field)
                            {
                                case 1:
                                    Console.Write("New Name: ");
                                    item.Name = Console.ReadLine() ?? "N/A";
                                    break;
                                case 2:
                                    Console.Write("New Price: ");
                                    item.Price = double.Parse(Console.ReadLine() ?? "0");
                                    break;
                                case 3:
                                    Console.Write("New Quantity: ");
                                    item.Quantity = int.Parse(Console.ReadLine() ?? "0");
                                    break;
                                default:
                                    Console.WriteLine("Invalid selection. Going back to menu.");
                                    ShowOptions();
                                    break;
                            }

                            ProductServiceProxy.Current.AddOrUpdate(item);
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                        break;

                    case 'd':
                        Console.WriteLine("Enter ID of product to delete:");
                        int targetId = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(targetId);
                        break;

                    case 'a':
                        Console.WriteLine("Select a product by ID to add to cart:");
                        inventoryList.ForEach(Console.WriteLine);
                        int chosenId = int.Parse(Console.ReadLine() ?? "-1");
                        var chosenProduct = inventoryList.FirstOrDefault(p => p.Id == chosenId);

                        if (chosenProduct == null)
                        {
                            Console.WriteLine("Item does not exist.");
                        }
                        else
                        {
                            Console.WriteLine("Enter quantity:");
                            int qty = int.Parse(Console.ReadLine() ?? "0");
                            userCart.AddToCart(chosenId, qty);
                        }
                        break;

                    case 'x':
                        if (userCart.IsEmpty())
                        {
                            Console.WriteLine("Cart is currently empty.");
                            break;
                        }

                        Console.WriteLine("Enter ID of product to remove:");
                        userCart.ShowCart();
                        int itemToRemove = int.Parse(Console.ReadLine() ?? "-1");

                        if (!userCart.Contents.ContainsKey(itemToRemove))
                        {
                            Console.WriteLine("Invalid ID.");
                        }
                        else
                        {
                            Console.WriteLine("Enter quantity to remove:");
                            int removeQty = int.Parse(Console.ReadLine() ?? "0");
                            userCart.RemoveItem(itemToRemove, removeQty);
                        }
                        break;

                    case 'p':
                        userCart.ShowCart();
                        break;

                    case 'l':
                        userCart.Checkout();
                        break;

                    case 'q':
                        break;

                    default:
                        Console.WriteLine("Unrecognized command.");
                        break;
                }

                Console.WriteLine();
                ShowOptions();

            } while (userInput != 'q');

            Console.WriteLine("Thanks for shopping with us. See you next time!");
        }

        static void ShowOptions()
        {
            Console.WriteLine("\n===== MENU =====");
            Console.WriteLine("C - Create New Product");
            Console.WriteLine("R - View All Products");
            Console.WriteLine("U - Update Existing Product");
            Console.WriteLine("D - Delete Product");
            Console.WriteLine("A - Add Item to Cart");
            Console.WriteLine("P - View Cart");
            Console.WriteLine("X - Remove Item from Cart");
            Console.WriteLine("L - Checkout");
            Console.WriteLine("Q - Quit");
            Console.WriteLine("================");
        }
    }
}
