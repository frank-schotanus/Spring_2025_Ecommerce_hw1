//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Ecommerce.Models;
using System;
using System.Xml.Serialization;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //here

            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");
            Console.WriteLine("A. Add an item to your shopping cart");
            Console.WriteLine("T. Remove an item from your shopping cart");
            Console.WriteLine("S. Show the contents of your shopping cart");
            Console.WriteLine("X. Checkout");
            Console.WriteLine("Q. Quit");

            List<Product?> list = ProductServiceProxy.Current.Products;

            ShoppingCart customerCart = new ShoppingCart();

            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                    case 'c':
                        Console.WriteLine("Please enter the name, price, and quantity of the product you want to add.");
                        ProductServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = Console.ReadLine(),
                            Price = double.Parse(Console.ReadLine()),
                            Quantity = int.Parse(Console.ReadLine())
                        });
                        break;
                    case 'R':
                    case 'r':

                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        //select one of the products
                        list.ForEach(Console.WriteLine);
                        Console.WriteLine("Enter the Id of the product you would like to update.");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            Console.WriteLine("How would you like to update the product?");
                            Console.WriteLine("1. Name");
                            Console.WriteLine("2. Price");
                            Console.WriteLine("3. Quantity");
                            Console.WriteLine("Enter your number choice, please.");

                            int updateChoice = int.Parse(Console.ReadLine() ?? "-1");

                            if(updateChoice == 1)
                            {
                                Console.WriteLine("Enter the updated name.");
                                selectedProd.Name = Console.ReadLine() ?? "ERROR";
                                ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                            }
                            else if(updateChoice == 2)
                            {
                                Console.WriteLine("Enter the updated price.");
                                selectedProd.Price = double.Parse(Console.ReadLine() ?? "-1");
                                ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                            }
                            else if(updateChoice == 3)
                            {
                                Console.WriteLine("Enter the updated quantity.");
                                selectedProd.Quantity = int.Parse(Console.ReadLine() ?? "-1");
                                ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice. Returning to the menu.");

                                Console.WriteLine("C. Create new inventory item");
                                Console.WriteLine("R. Read all inventory items");
                                Console.WriteLine("U. Update an inventory item");
                                Console.WriteLine("D. Delete an inventory item");
                                Console.WriteLine("A. Add an item to your shopping cart");
                                Console.WriteLine("T. Remove an item from your shopping cart");
                                Console.WriteLine("S. Show the contents of your shopping cart");
                                Console.WriteLine("X. Checkout");
                                Console.WriteLine("Q. Quit");
                            }

                        }
                        break;
                    case 'D':
                    case 'd':
                        //select one of the products
                        //throw it away
                        Console.WriteLine("Which product would you like to update?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;
                    case 'A':
                    case 'a':
                        Console.WriteLine("Choose which product you would like to add to your cart (Enter the ID Number): ");
                        list.ForEach(Console.WriteLine);
                        int addThisProduct = int.Parse(Console.ReadLine() ?? "-1");
                        var productSelected = list.FirstOrDefault(p => p.Id == addThisProduct);

                        if (productSelected == null)
                        {
                            Console.WriteLine("Null Product.");
                        }
                        else
                        {
                            Console.WriteLine("Please enter the quantity you would like: ");
                            int quantitySpecified = int.Parse(Console.ReadLine());
                            customerCart.AddItem(addThisProduct, quantitySpecified);
                        }
                        break;
                    case 'T':
                    case 't':
                        if(customerCart.cartIsEmpty() == true)
                        {
                            Console.WriteLine("Your cart is empty.");
                            break;
                        }

                        Console.WriteLine("Choose which product you would like to remove from the cart (Enter the ID Number): ");
                        customerCart.PrintCart();
                        int removeThisProduct = int.Parse(Console.ReadLine() ?? "-1");
                        Console.WriteLine($"Product to be removed ID: {removeThisProduct}");

                        if (customerCart.ItemsInCart.ContainsKey(removeThisProduct) == false)
                        {
                            Console.WriteLine("Null Product");
                        }
                        else
                        {
                            Console.WriteLine("Please enter the quantity you would like to remove: ");
                            int quantitySpecified = int.Parse(Console.ReadLine());

                            customerCart.removeFromCart(removeThisProduct, quantitySpecified);
                        }
                        break;
                    case 'S':
                    case 's':
                        customerCart.PrintCart();
                        break;
                    case 'X':
                    case 'x':
                        customerCart.printReceipt();
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;
                }
            } while (choice != 'Q' && choice != 'q');

            Console.ReadLine();
        }
    }


}
