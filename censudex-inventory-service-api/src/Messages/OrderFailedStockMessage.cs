using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Messages
{
    /// <summary>
    /// Mensaje que representa el fallo de stock en una orden.
    /// </summary>
    public class OrderFailedStockMessage
    {
        /// <summary>
        /// Identificador único de la orden.
        /// </summary>
        public Guid orderId { get; set; }
        /// <summary>
        /// Razón del fallo de stock.
        /// </summary>
        public string reason { get; set; } = "Insufficient stock";
        /// <summary>
        /// Indica si hubo un error.
        /// </summary>
        public bool errored { get; set; } = true;
        /// <summary>
        /// Fecha y hora en que se reportó el fallo.
        /// </summary>
        public DateTime reportedAt { get; set; } = DateTime.UtcNow;
    }
}