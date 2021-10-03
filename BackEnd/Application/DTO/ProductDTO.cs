using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{
    public class ProductDTO
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int SearchWeight { get; set; }
        public int Order { get; set; }
    }
}
