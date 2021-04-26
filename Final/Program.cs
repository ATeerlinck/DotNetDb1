﻿using System;
using NLog.Web;
using System.IO;
using System.Linq;
using Final.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Final
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {
                string choice;
                do
                {   
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Display Category and related products"); 
                    Console.WriteLine("4) Display all Categories and their related products");
                    Console.WriteLine("5) Delete Category");
                    Console.WriteLine("6) Add Product");
                    Console.WriteLine("7) Delete Product");
                    Console.WriteLine("8) Display Products");
                    Console.WriteLine("9) Display one Product");
                    Console.WriteLine("10) Edit Product");
                    Console.WriteLine("11) Display Category with its active products");
                    Console.WriteLine("12) Display all Categories with their active products");
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (choice == "2")
                    {
                        Categories category = new Categories();
                        Console.WriteLine("Enter Category Name:");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        category.Description = Console.ReadLine();

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new Northwind_DotNetDb_ABTContext();
                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddCategory(category);
                                logger.Info($"{category.CategoryName} category added. Id of {category.CategoryId}.");
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "3")
                    {
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Categories category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Products p in category.Products)
                        {
                            Console.WriteLine(p.ProductName);
                        }
                    }
                    else if (choice == "4")
                    {
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}");
                            foreach (Products p in item.Products)
                            {
                                Console.WriteLine($"\t{p.ProductName}");
                            }
                        }
                    }
                    else if(choice == "5"){
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Categories category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                        if(category.Products.Count > 0){
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("WARNING! You were about to remove a category that has products still in it. The products in this category must be removed first. ");
                            logger.Info("Category removal aborted");
                        }
                        else{
                            logger.Info($"CategoryId {id} removed");
                            db.DeleteCategory(category);
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if(choice == "6"){
                        var db = new Northwind_DotNetDb_ABTContext();
                        Products product = new Products();
                        Console.WriteLine("Enter product Name:");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter the Supplier Id:");
                        //display suppliers
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        var suppliers = db.Suppliers.OrderBy(s => s.SupplierId);
                        foreach (Suppliers s in suppliers){
                            Console.WriteLine($"{s.SupplierId}: {s.CompanyName}");
                        }
                        //select supplier
                        Console.ForegroundColor = ConsoleColor.White;
                        if (int.TryParse(Console.ReadLine(), out int SupplierId)){
                            Suppliers supplier = db.Suppliers.FirstOrDefault(s => s.SupplierId == SupplierId);
                            if(supplier != null){
                                product.SupplierId = supplier.SupplierId;
                                product.Supplier = supplier;
                                logger.Info($"SupplierId# {product.SupplierId} found.");
                            }
                            else{
                                logger.Error("Invalid Supplier Id. Supplier set to null");
                                product.SupplierId = null;
                                product.Supplier = null;
                            }
                        }
                        //display categories
                        var query = db.Categories.OrderBy(p => p.CategoryId);
                        Console.WriteLine("Select the category for this product:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        //select category
                        int id = int.Parse(Console.ReadLine());
                        logger.Info($"CategoryId {id} selected");
                        Categories category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                        if(category != null){
                                product.CategoryId = category.CategoryId;
                                product.Category = category;
                                logger.Info($"CategoryId# {product.CategoryId} found.");
                            }
                            else{
                                logger.Error("Invalid Category Id. Category set to null");
                                product.CategoryId = null;
                                product.Category = null;
                            }
                        //set the rest
                        Console.WriteLine("Enter the Quantity Per Unit:");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter the Unit Price:");
                        product.UnitPrice = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Units in Stock:");
                        product.UnitsInStock = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Units on Order:");
                        product.UnitsOnOrder = short.Parse(Console.ReadLine());
                        Console.WriteLine("Enter ReorderLevel:");
                        product.ReorderLevel = short.Parse(Console.ReadLine());
                        Console.WriteLine("Is the product Discontinued: y/n");
                        product.Discontinued = Console.ReadLine().ToLower() == "y" ? true : false;

                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if (isValid)
                        {
                            
                            // check for unique name
                            if (db.Products.Any(c => c.ProductName == product.ProductName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "ProductName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddProduct(product);
                                logger.Info($"{product.ProductName} added. Id of {product.ProductId}.");
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if(choice == "7"){
                        Console.WriteLine("Select the product you want to remove");
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Products.OrderBy(p => p.ProductId);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductId}) {item.ProductName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"ProductId {id} selected");
                        Products product = db.Products.FirstOrDefault(c => c.ProductId == id);
                        Console.WriteLine($"Are you sure you want to remove {product.ProductName}? y/n");
                        if(Console.ReadLine().ToLower()=="y"){
                            logger.Info($"ProductId {id} removed");
                            db.DeleteProduct(product);
                        }
                    }
                    else if(choice == "8"){
                        //selector
                        Console.WriteLine("1) Display all");
                        Console.WriteLine("2) Display active");
                        Console.WriteLine("3) Display discontinued");
                        choice = Console.ReadLine();
                        logger.Info($"Option {choice} selected");
                        //executor
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Products.OrderBy(p => p.ProductName);
                        if(choice == "1"){
                            query = db.Products.OrderBy(p => p.ProductName);
                        }
                        else if(choice == "2"){
                            query = db.Products.Where(p => p.Discontinued == false).OrderBy(p => p.ProductName);
                        }
                        else if(choice == "3"){
                            query = db.Products.Where(p => p.Discontinued == true).OrderBy(p => p.ProductName);
                        }
                        else{
                            logger.Error("invalid choice selected. defaulting to all.");
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{query.Count()} records returned");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if(choice == "9"){
                        Console.WriteLine("Select the product you want to see all details of");
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Products.OrderBy(p => p.ProductId);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductId}) {item.ProductName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"ProductId {id} selected");
                        Products product = db.Products.FirstOrDefault(c => c.ProductId == id);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine($"Name: {product.ProductName}");
                        Console.WriteLine($"Id: {product.ProductId}");
                        Console.WriteLine($"SupplierId: {product.SupplierId}");
                        Console.WriteLine($"CategoryId: {product.CategoryId}");
                        Console.WriteLine($"Unit Price: ${product.UnitPrice}");
                        Console.WriteLine($"Units In Stock: {product.UnitsInStock}");
                        Console.WriteLine($"Units On Order: {product.UnitsOnOrder}");
                        Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
                        Console.WriteLine($"Discontinued: {product.Discontinued}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if(choice == "10"){
                        Console.WriteLine("Select the product you want to edit");
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Products.OrderBy(p => p.ProductId);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductId}) {item.ProductName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"ProductId {id} selected");
                        Products product = db.Products.FirstOrDefault(c => c.ProductId == id);
                        while(choice != "0"){
                            Console.WriteLine("What do you want to edit?");
                            Console.WriteLine("1)Name \n2)Supplier \n3)Category \n4)Quantity per Unit \n5)Unit Price \n6)Units In Stock: \n7)Units On Order \n8)Reorder Level \n9)Discontinued \n0)End Editing");
                            choice = Console.ReadLine();
                            switch(choice){
                                case "1": Console.WriteLine("What is the product's new name");
                                product.ProductName = Console.ReadLine();
                                break;
                                case "2": Console.WriteLine("Select the products supplier:");
                                //display suppliers
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                var suppliers = db.Suppliers.OrderBy(s => s.SupplierId);
                                foreach (Suppliers s in suppliers){
                                    Console.WriteLine($"{s.SupplierId}: {s.CompanyName}");
                                }
                                //select supplier
                                Console.ForegroundColor = ConsoleColor.White;
                                if (int.TryParse(Console.ReadLine(), out int SupplierId)){
                                    Suppliers supplier = db.Suppliers.FirstOrDefault(s => s.SupplierId == SupplierId);
                                    if(supplier != null){
                                        product.SupplierId = supplier.SupplierId;
                                        product.Supplier = supplier;
                                        logger.Info($"SupplierId# {product.SupplierId} found.");
                                    }
                                    else{
                                        logger.Error("Invalid Supplier Id. Supplier set to null");
                                        product.SupplierId = null;
                                        product.Supplier = null;
                                    }
                                }
                                break;
                                case "3": Console.WriteLine("What is the product's new name");
                                //display categories
                                var categories = db.Categories.OrderBy(p => p.CategoryId);
                                Console.WriteLine("Select the category for this product:");
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                foreach (var item in categories)
                                {
                                    Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                //select category
                                id = int.Parse(Console.ReadLine());
                                logger.Info($"CategoryId {id} selected");
                                Categories category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                                if(category != null){
                                        product.CategoryId = category.CategoryId;
                                        product.Category = category;
                                        logger.Info($"CategoryId# {product.CategoryId} found.");
                                    }
                                    else{
                                        logger.Error("Invalid Category Id. Category set to null");
                                        product.CategoryId = null;
                                        product.Category = null;
                                    }
                                break;
                                case "4": Console.WriteLine("Enter the Quantity Per Unit:");
                                product.QuantityPerUnit = Console.ReadLine();
                                break;
                                case "5": Console.WriteLine("Enter the Unit Price:");
                                product.UnitPrice = decimal.Parse(Console.ReadLine());
                                break;
                                case "6": Console.WriteLine("Enter Units in Stock:");
                                product.UnitsInStock = short.Parse(Console.ReadLine());
                                break;
                                case "7": Console.WriteLine("Enter Units on Order:");
                                product.UnitsOnOrder = short.Parse(Console.ReadLine());
                                break;
                                case "8": Console.WriteLine("Enter ReorderLevel:");
                                product.ReorderLevel = short.Parse(Console.ReadLine());
                                break;
                                case "9": Console.WriteLine("Is the product Discontinued: y/n");
                                product.Discontinued = Console.ReadLine().ToLower() == "y" ? true : false;
                                break;
                                case "0":
                                default: break;
                            }
                        }
                        try{
                            db.EditProduct(product);
                            logger.Info($"{product.ProductName} was updated.");
                        }
                        catch(Exception ex){
                            logger.Error(ex.Message);
                        }
                    }
                    else if(choice =="11"){
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Categories category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Products p in category.Products)
                        {
                            if(p.Discontinued==false) Console.WriteLine(p.ProductName);
                        }
                    }
                    else if(choice =="12"){
                        var db = new Northwind_DotNetDb_ABTContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Categories category = db.Categories.Include("Products").FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Products p in category.Products)
                        {
                            if(p.Discontinued==false) Console.WriteLine(p.ProductName);
                        }
                    }
                    Console.WriteLine();

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
