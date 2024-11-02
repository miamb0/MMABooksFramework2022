using NUnit.Framework;
using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using System.Collections.Generic;
using System;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            // Reset the database to a known state before each test
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand
            {
                CommandText = "usp_testingResetProductData", // Ensure this stored procedure is implemented
                CommandType = CommandType.StoredProcedure
            };
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewProductConstructor()
        {
            // Test that a new Product object has default values
            Product product = new Product();
            Assert.AreEqual(0, product.ProductID);
            Assert.AreEqual(string.Empty, product.ProductCode);
            Assert.AreEqual(string.Empty, product.Description);
            Assert.AreEqual(0m, product.UnitPrice);
            Assert.AreEqual(0, product.OnHandQuantity);
            Assert.AreEqual(0, product.ConcurrencyID);
            Assert.IsTrue(product.IsNew);
            Assert.IsFalse(product.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreConstructor()
        {
            // Test retrieving an existing product from the database
            Product product = new Product(929);
            Assert.AreEqual("A4CS", product.ProductCode); 
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", product.Description); // Adjust based on your test data
            Assert.AreEqual(56.5000, product.UnitPrice);
            Assert.AreEqual(4637, product.OnHandQuantity); 
            Assert.IsFalse(product.IsNew);
            Assert.IsTrue(product.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            // Test saving a new product to the database
            Product product = new Product
            {
                ProductCode = "Test",
                Description = "New Product",
                UnitPrice = 50.5000m,
                OnHandQuantity = 50
            };
            product.Save();

            // Retrieve the product to confirm it was saved correctly
            Product savedProduct = new Product(product.ProductID);
            Assert.AreEqual(savedProduct.ProductCode, product.ProductCode);
            Assert.AreEqual(savedProduct.Description, product.Description);
            Assert.AreEqual(savedProduct.UnitPrice, product.UnitPrice);
            Assert.AreEqual(savedProduct.OnHandQuantity, product.OnHandQuantity);
        }

        [Test]
        public void TestUpdate()
        {
            // Test updating an existing product's details
            Product product = new Product(929); 
            product.Description = "Updated Product Description";
            product.UnitPrice = 50.99m;
            product.OnHandQuantity = 75;
            product.Save();

            // Retrieve the product again to confirm the updates
            Product updatedProduct = new Product(929);
            Assert.AreEqual(updatedProduct.Description, product.Description);
            Assert.AreEqual(updatedProduct.UnitPrice, product.UnitPrice);
            Assert.AreEqual(updatedProduct.OnHandQuantity, product.OnHandQuantity);
        }

        [Test]
        public void TestDelete()
        {
            // Test deleting an existing product
            Product product = new Product(929); 
            product.Delete();
            Assert.Throws<Exception>(() => new Product(929)); // Should throw an exception if deleted
        }

        [Test]
        public void TestGetList()
        {
            // Test retrieving the list of products
            Product product = new Product();
            List<Product> products = (List<Product>)product.GetList();
            Assert.IsTrue(products.Count > 0); 
            Assert.AreEqual("P001", products[0].ProductCode); 
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // Test saving a product without required properties
            Product product = new Product();
            Assert.Throws<Exception>(() => product.Save()); 
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // Test saving with some required properties missing
            Product product = new Product
            {
                ProductCode = "929" 
            };
            Assert.Throws<Exception>(() => product.Save()); // Should throw an exception
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            // Test setting an invalid unit price
            Product product = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => product.UnitPrice = -10.00m); // Negative unit price should be invalid
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            // Test for concurrency issues when updating the same product
            Product product1 = new Product(929); 
            Product product2 = new Product(929); 

            product1.UnitPrice = 20.00m;
            product1.Save();

            product2.UnitPrice = 22.00m;
            Assert.Throws<Exception>(() => product2.Save()); 
        }
    }
}
