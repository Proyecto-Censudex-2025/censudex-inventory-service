using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Dto
{
    public class ProductDto
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public int stock { get; set; }
        public bool is_Active { get; set; }
        public int minimum_stock { get; set; }
    }
}