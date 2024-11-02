using System;
using System.Collections.Generic;
using System.Text;

using MMABooksTools;
using DBDataReader = MySql.Data.MySqlClient.MySqlDataReader;

using System.Text.Json;
using System.Text.Json.Serialization;
using MySql.Data.MySqlClient;

namespace MMABooksProps
{
    [Serializable()]
    public class ProductProps : IBaseProps
    {
        #region Auto-implemented Properties
        public int ProductId { get; set; } = 0; // Unique identifier/primary key for product
        public string ProductCode { get; set; } = ""; // char(10)
        public string Description { get; set; } = ""; // varchar(50)
        public decimal UnitPrice { get; set; } = 0.0M; // decimal(10,4)
        public int OnHandQuantity { get; set; } = 0; // int
        public int ConcurrencyID { get; set; } = 0; // Concurrency control
        #endregion


        public object Clone()
        {
            ProductProps p = new ProductProps();
            p.ProductId = this.ProductId;
            p.ProductCode = this.ProductCode;
            p.Description = this.Description;
            p.UnitPrice = this.UnitPrice;
            p.OnHandQuantity = this.OnHandQuantity;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }

       
        public string GetState()
        {
            return JsonSerializer.Serialize(this);
        }

       
        public void SetState(string jsonString)
        {
            ProductProps p = JsonSerializer.Deserialize<ProductProps>(jsonString);
            this.ProductId = p.ProductId;
            this.ProductCode = p.ProductCode;
            this.Description = p.Description;
            this.UnitPrice = p.UnitPrice;
            this.OnHandQuantity = p.OnHandQuantity;
            this.ConcurrencyID = p.ConcurrencyID;
        }

      
        public void SetState(MySqlDataReader dr)
        {
            this.ProductId = (int)dr["ProductID"];
            this.ProductCode = ((string)dr["ProductCode"]).Trim();
            this.Description = ((string)dr["Description"]).Trim();
            this.UnitPrice = (decimal)dr["UnitPrice"];
            this.OnHandQuantity = (int)dr["OnHandQuantity"];
            this.ConcurrencyID = (int)dr["ConcurrencyID"];
        }
    }
}