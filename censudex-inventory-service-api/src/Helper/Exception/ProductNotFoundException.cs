using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censudex_inventory_service_api.src.Helper.Exception
{
    /// <summary>
    /// Excepción lanzada cuando un producto no es encontrado.
    /// </summary>
    public class ProductNotFoundException : global::System.Exception
    {
        /// <summary>
        /// Constructor de la excepción ProductNotFoundException.
        /// </summary>
        /// <param name="message">Mensaje de error que describe la excepción.</param>
        public ProductNotFoundException(string message) : base(message) {}
    }
}