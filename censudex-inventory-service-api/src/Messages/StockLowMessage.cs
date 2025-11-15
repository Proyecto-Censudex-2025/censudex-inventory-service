using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Messages
{
    public class StockLowMessage
    {
        public Guid productId { get; set; }
        public string productName { get; set; }
        public int currentStock { get; set; }
        public int minimumStock { get; set; }
        public DateTime reportedAt { get; set; } = DateTime.UtcNow;
    }
}