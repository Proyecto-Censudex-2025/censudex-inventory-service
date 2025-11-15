using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Messages
{
    public class OrderCreatedMessage
    {
        public Guid orderId { get; set; }
        public Dictionary<Guid, int> products { get; set; }
    }
}