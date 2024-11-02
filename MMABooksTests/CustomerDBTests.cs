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
    internal class CustomerDBTests
    {
        CustomerDB db;

        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Minnie Mouse";
            p.Address = "123 Main Street";
            p.City = "Orlando";
            p.State = "FL";
            p.ZipCode = "10001";
            db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerId);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1); 
            Assert.AreEqual(1, p.CustomerId);
            Assert.AreEqual("Molunguri, A", p.Name); 
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.GreaterOrEqual(list.Count, 1); 
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "Newname"; 
            Assert.True(db.Update(p)); 
            p = (CustomerProps)db.Retrieve(1); 
            Assert.AreEqual("Newname", p.Name); 
        }


        [Test]
        public void TestUpdateFieldTooLong()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "A name totally can not possibly be this long.";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            CustomerProps p = new CustomerProps
            {
                CustomerId = 1, 
                Name = "Duplicate Name",
                Address = "123 Duplicate St.",
                City = "Duplication",
                State = "DP",
                ZipCode = "10001"
            };
            Assert.Throws<MySqlException>(() => db.Create(p)); // Ensure it throws for primary key violation
        }
    }
}
