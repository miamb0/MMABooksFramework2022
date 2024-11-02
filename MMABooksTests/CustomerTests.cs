using NUnit.Framework;

using MMABooksBusiness;
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
    public class CustomerTests
    {

        [SetUp]
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetCustomerData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestNewCustomerConstructor()
        {
            
            Customer c = new Customer();
            Assert.AreEqual(string.Empty, c.Name);
            Assert.AreEqual(string.Empty, c.Address);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
           
            Customer c = new Customer(1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johann Bay Drive", c.Address);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.Name = "Mia Blan";
            c.Address = "123 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            c.Save();
            Customer c2 = new Customer(c.CustomerId);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer(1);
            c.Name = "Newname";
            c.Save();

            Customer c2 = new Customer(1);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(1);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(1));
        }

        [Test]
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(699, customers.Count); 
            Assert.AreEqual("Molunguri, A", customers[0].Name);
            Assert.AreEqual("1108 Johann Bay Drive", customers[0].Address);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            Customer c = new Customer();
            c.Name = "Sample Name";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.ZipCode = "123456789"); 
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1);
            Customer c2 = new Customer(1);

            c1.Name = "Updated by first instance";
            c1.Save();

            c2.Name = "Updated by second instance";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}