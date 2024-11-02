using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MMABooksProps
{
    [Serializable()]
    public class CustomerProps : IBaseProps
    {
        #region Auto-implemented Properties
        public int CustomerId { get; set; } = 0; // Unique identifier/primary key for customer
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string ZipCode { get; set; } = "";

        /// <summary>
        /// ConcurrencyID. Don't manipulate directly.
        /// </summary>
        public int ConcurrencyID { get; set; } = 0;
        #endregion

        public object Clone()
        {
            CustomerProps p = new CustomerProps();
            p.CustomerId = this.CustomerId;
            p.Name = this.Name;
            p.Address = this.Address;
            p.City = this.City;
            p.State = this.State;
            p.ZipCode = this.ZipCode;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }

        public string GetState()
        {
            string jsonString;
            jsonString = JsonSerializer.Serialize(this);
            return jsonString;
        }

        public void SetState(string jsonString)
        {
            CustomerProps p = JsonSerializer.Deserialize<CustomerProps>(jsonString);
            this.CustomerId = p.CustomerId;
            this.Name = p.Name;
            this.Address = p.Address;
            this.City = p.City;
            this.State = p.State;
            this.ZipCode = p.ZipCode;
            this.ConcurrencyID = p.ConcurrencyID;
        }

        public void SetState(DBDataReader dr)
        {
            this.CustomerId = (int)dr["CustomerId"];
            this.Name = ((string)dr["Name"]).Trim();
            this.Address = ((string)dr["Address"]).Trim();
            this.City = ((string)dr["City"]).Trim();
            this.State = ((string)dr["State"]).Trim();
            this.ZipCode = ((string)dr["Zipcode"]).Trim();
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
        }
    }
}