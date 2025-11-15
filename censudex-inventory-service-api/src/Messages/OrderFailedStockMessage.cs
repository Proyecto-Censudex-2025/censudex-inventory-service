using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Messages
{
    public class OrderFailedStockMessage
    {
        public Guid orderId { get; set; }
        public string reason { get; set; } = "Insufficient stock";
        public bool errored { get; set; } = true;
        public DateTime reportedAt { get; set; } = DateTime.UtcNow;
    }
}