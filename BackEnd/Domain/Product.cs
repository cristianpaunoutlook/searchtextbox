using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int SearchWeight { get; set; }
    }
}
