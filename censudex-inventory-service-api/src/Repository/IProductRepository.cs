using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Model;

namespace censudex_inventory_service_api.src.Repository
{
    /// <summary>
    /// Interfaz para el repositorio de productos.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Lista de productos</returns>
        Task<IEnumerable<Product>?> GetAllProducts();
        /// <summary>
        /// Obtiene un producto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Producto encontrado o null si no existe.</returns>
        Task<Product?> GetProductById(Guid id);
        /// <summary>
        /// Agrega un nuevo producto.
        /// </summary>
        /// <param name="product">Producto a agregar.</param>
        Task AddProduct(Product product);
        /// <summary>
        /// Actualiza el stock de un producto.
        /// </summary>
        /// <param name="productId">Identificador del producto.</param>
        /// <param name="newStock">Valor a incrementar o decrementar del stock.</param>
        Task UpdateStock(Guid productId, int newStock);
        /// <summary>
        /// Actualiza el stock mínimo de un producto.
        /// </summary>
        /// <param name="productId">Identificador del producto.</param>
        /// <param name="minimumStock">Nuevo valor de stock mínimo.</param>
        Task UpdateMinimumStock(Guid productId, int minimumStock);
    }
}