using NUnit.Framework;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;
using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductDBTests
    {
        ProductDB db;

        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData"; 
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps
            {
                ProductCode = "TEST", 
                Description = "Test Product", 
                UnitPrice = 19.99m, 
                OnHandQuantity = 100, 
                ConcurrencyID = 1 
            };
            db.Create(p); 
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductId); 
            Assert.AreEqual(p.Description, p2.Description);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(929); 
            Assert.AreEqual(929, p.ProductId); 
            Assert.AreEqual("Some Product", p.Description); 
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll(); 
            Assert.GreaterOrEqual(list.Count, 929); 
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve(929); 
            Assert.True(db.Delete(p)); 
            Assert.Throws<Exception>(() => db.Retrieve(929)); 
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(929); 
            p.Description = "Updated Product";
            Assert.True(db.Update(p)); 
            p = (ProductProps)db.Retrieve(929); 
            Assert.AreEqual("Updated Product", p.Description); 
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            ProductProps p = (ProductProps)db.Retrieve(929);
            p.Description = new string('A', 51); 
            Assert.Throws<MySqlException>(() => db.Update(p)); 
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            ProductProps p = new ProductProps
            {
                ProductId = 929, 
                ProductCode = "A4CS",
                Description = "Another Product",
                UnitPrice = 29.99m,
                OnHandQuantity = 50,
                ConcurrencyID = 1
            };
            Assert.Throws<MySqlException>(() => db.Create(p)); 
        }
    }
}
