using DotNetDrinks.Controllers;
using DotNetDrinks.Data;
using DotNetDrinks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDrinksTests
{
    [TestClass]
    public class ProductsControllerTests
    {
        private ApplicationDbContext _context;
        // empty list of products
        List<Product> products = new List<Product>();
        // declare the controller that will be tested
        ProductsController controller;

        // How do I fill _context with data? or when?
        // Create a constructor?? Rather, create an Initialize method
        [TestInitialize]
        public void TestInitialize()
        {

            
            // instantiate in-memory db > similar to startup.cs
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

           
            //create mock data in this database 
         
            var category = new Category
            {
                Id = 100,
                Name = "Test Category"
            };

            _context.Categories.Add(category);

            _context.SaveChanges();

            

            // Create 1 brand
            var brand = new Brand { Id = 100, Name = "No Name" };
            _context.Brands.Add(brand);
            _context.SaveChanges();


            // Create 3 products
            products.Add(new Product { Id = 101, Name = "Product", Price = 11, Category = category, Brand = brand });
            products.Add(new Product { Id = 102, Name = "Another Product", Price = 12, Category = category, Brand = brand });
            products.Add(new Product { Id = 103, Name = "Extra Product", Price = 13, Category = category, Brand = brand });

         
            foreach (var p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();


            //set controller
            // instanciate the controller class with mock db context
            controller = new ProductsController(_context);

        }


        [TestMethod]
        public void GetDeleteReturn()

        {
            var id = 101;


            var result = controller.Delete(id);
            var viewResult = (ViewResult)result.Result;

            var model = (Product)viewResult.Model;

            var product = products.Find(e => e.Id == id);


            Assert.AreEqual(model, product);


        }

        [TestMethod]

        public void DeleteComfirmPost() {

            var id = 103;
            
            // Asynch method returns wrapper around result object
            var result = controller.DeleteConfirmed(id); 
            // Assert that product is in DB
            // Select by name
            var prod = _context.Products
                .Where(p => p.Id==id)
                .FirstOrDefault();
            // if found then prod is not null
            Assert.IsNull(prod);

        }

    }
}
