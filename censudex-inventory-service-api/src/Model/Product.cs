using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace censudex_inventory_service_api.src.Model
{
    /// <summary>
    /// Modelo que representa un producto en el inventario.
    /// </summary>
    [Table("Product")]
    public class Product : BaseModel
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        [PrimaryKey("id",true)]
        public Guid id { get; set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        [Column("name")]
        public string name { get; set; }

        /// <summary>
        /// Categoría del producto.
        /// </summary>
        [Column("category")]
        public string category { get; set; }

        /// <summary>
        /// Stock actual del producto.
        /// </summary>
        [Column("stock")]
        public int stock { get; set; }

        /// <summary>
        /// Indica si el producto está activo.
        /// </summary>
        [Column("is_active")]
        public bool is_active { get; set; }
        
        /// <summary>
        /// Stock mínimo del producto.
        /// </summary>
        [Column("minimum_stock")]
        public int minimum_stock { get; set; }
    }
}