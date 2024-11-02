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
    public class CustomerPropsTests
    {
        CustomerProps props; 

        [SetUp]
        public void Setup()
        {
            
            props = new CustomerProps();
            props.CustomerId = 1;
            props.Name = "John Doe";
            props.Address = "123 Main Street";
            props.City = "Sacramento";
            props.State = "CA";
            props.ZipCode = "95814";
        }

        [Test]
        public void TestGetState()
        {
            
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.Address.ToString())); 
            Assert.IsTrue(jsonString.Contains(props.Name));                
        }

        [Test]
        public void TestSetState()
        {
            
            string jsonString = props.GetState();
            CustomerProps newProps = new CustomerProps();
            newProps.SetState(jsonString);

            
            Assert.AreEqual(props.CustomerId, newProps.CustomerId);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestClone()
        {
           
            CustomerProps newProps = (CustomerProps)props.Clone();
            Assert.AreEqual(props.CustomerId, newProps.CustomerId);
            Assert.AreEqual(props.Name, newProps.Name);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}