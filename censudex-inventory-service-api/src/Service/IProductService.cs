using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;

namespace censudex_inventory_service_api.src.Service
{
    /// <summary>
    /// Interfaz del servicio de productos.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Agrega un nuevo producto.
        /// </summary>
        /// <param name="productDto">DTO del producto a agregar.</param>
        /// <returns>Producto agregado.</returns>
        public Task<ProductDto> AddProduct(ProductDto productDto);
        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        /// <returns>Lista de productos</returns>
        public Task<IEnumerable<ProductDto?>> GetAllProducts();
        /// <summary>
        /// Obtiene un producto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto a buscar.</param>
        /// <returns>Producto encontrado o null si no existe.</returns>
        public Task<ProductVisualizerDto?> GetProductById(Guid id);
        /// <summary>
        /// Actualiza el stock de un producto.
        /// </summary>
        /// <param name="productId">Identificador del producto a actualizar.</param>
        /// <param name="amount">Cantidad a incrementar o decrementar del stock.</param>
        /// <returns>Producto actualizado.</returns>
        public Task<ProductVisualizerDto> UpdateStock(Guid productId, int amount);
        /// <summary>
        /// Establece el stock mínimo de un producto.
        /// </summary>
        /// <param name="productId">Identificador del producto a actualizar.</param>
        /// <param name="minimumStock">Nueva cantidad mínima de stock.</param>
        /// <returns>Producto actualizado.</returns>
        public Task<ProductVisualizerDto> SetMinimumStock(Guid productId, int minimumStock);
    }
}