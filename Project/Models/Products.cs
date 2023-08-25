#nullable disable
using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Products
    {
        public int Id { get; set; }
        public string ProductsName { get; set; }
        public int CurrentPrice { get; set; }
        public string ProdType { get; set; }
    }
}