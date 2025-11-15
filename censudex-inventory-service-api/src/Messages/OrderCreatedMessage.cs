using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Messages
{
    /// <summary>
    /// Mensaje que representa la creación de una orden.
    /// </summary>
    public class OrderCreatedMessage
    {
        /// <summary>
        /// Identificador único de la orden.
        /// </summary>
        public Guid orderId { get; set; }
        /// <summary>
        /// Diccionario que mapea los identificadores de productos a sus cantidades.
        /// </summary>
        public Dictionary<Guid, int> products { get; set; }
    }
}