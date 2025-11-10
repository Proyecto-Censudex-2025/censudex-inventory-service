using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Helper.Exception
{
    public class ProductNotFoundException : global::System.Exception
    {
        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}