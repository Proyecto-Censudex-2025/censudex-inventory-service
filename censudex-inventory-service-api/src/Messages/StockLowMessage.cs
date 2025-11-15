using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Messages
{
    /// <summary>
    /// Mensaje que representa una alerta de stock bajo.
    /// </summary>
    public class StockLowMessage
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid productId { get; set; }
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// Stock actual del producto.
        /// </summary>
        public int currentStock { get; set; }
        /// <summary>
        /// Stock mínimo del producto.
        /// </summary>
        public int minimumStock { get; set; }
        /// <summary>
        /// Fecha y hora en que se reportó la alerta.
        /// </summary>
        public DateTime reportedAt { get; set; } = DateTime.UtcNow;
    }
}