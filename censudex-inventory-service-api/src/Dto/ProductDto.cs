using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Dto
{
    /// <summary>
    /// Data Transfer Object (DTO) que representa un producto en el inventario.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public Guid id { get; set; }
        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Categoría del producto.
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// Stock disponible del producto.
        /// </summary>
        public int stock { get; set; }
        /// <summary>
        /// Indica si el producto está activo.
        /// </summary>
        public bool is_Active { get; set; }
        /// <summary>
        /// Stock mínimo del producto.
        /// </summary>
        public int minimum_stock { get; set; }
    }
}