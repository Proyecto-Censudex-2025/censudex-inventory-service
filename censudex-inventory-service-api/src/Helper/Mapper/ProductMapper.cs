using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;
using censudex_inventory_service_api.src.Model;
using InventoryService.Grpc;

namespace censudex_inventory_service_api.src.Helper.Mapper
{
    /// <summary>
    /// Clase encargada de mapear entre diferentes representaciones de productos.
    /// </summary>
    public class ProductMapper
    {
        /// <summary>
        /// Convierte un modelo de producto a un DTO de producto.
        /// </summary>
        /// <param name="product">Producto a convertir.</param>
        /// <returns>DTO del producto.</returns>
        public static ProductDto ToDto(Product product)
        {
            return new ProductDto
            {
                id = product.id,
                name = product.name,
                category = product.category,
                stock = product.stock,
                is_Active = product.is_active,
                minimum_stock = product.minimum_stock 

            };
        }
        /// <summary>
        /// Convierte un DTO de producto a un modelo de producto.
        /// </summary>
        /// <param name="productDto">DTO de un producto a convertir.</param>
        /// <returns>Modelo del producto.</returns>
        public static Product ToModel(ProductDto productDto)
        {
            return new Product
            {
                id = productDto.id,
                name = productDto.name,
                category = productDto.category,
                stock = productDto.stock,
                is_active = productDto.is_Active,
                minimum_stock = productDto.minimum_stock
            };
        }
        /// <summary>
        /// Convierte un modelo de producto a un DTO para visualización.
        /// </summary>
        /// <param name="product">Producto a convertir.</param>
        /// <returns>DTO para visualización del producto.</returns>
        public static ProductVisualizerDto toVisualizer(Product product)
        {
            return new ProductVisualizerDto
            {
                name = product.name,
                stock = product.stock,
                minimum_stock = product.minimum_stock,
                is_Active = product.is_active
            };
        }
        /// <summary>
        /// Convierte un mensaje de producto a un DTO de producto.
        /// </summary>
        /// <param name="productMessage">Mensaje gRPC de producto.</param>
        /// <returns>DTO del producto.</returns>
        public static ProductDto toDto(ProductMessage productMessage)
        {
            return new ProductDto
            {
                id = Guid.Parse(productMessage.Id),
                name = productMessage.Name,
                category = productMessage.Category,
                stock = productMessage.Stock,
                is_Active = productMessage.IsActive,
                minimum_stock = productMessage.MinimumStock
            };
        }
        /// <summary>
        /// Convierte un DTO de producto a un mensaje gRPC de producto.
        /// </summary>
        /// <param name="productDto">DTO de producto.</param>
        /// <returns>Mensaje gRPC de producto.</returns>
        public static ProductMessage toMessage(ProductDto productDto)
        {
            return new ProductMessage
            {
                Id = productDto.id.ToString(),
                Name = productDto.name,
                Category = productDto.category,
                Stock = productDto.stock,
                IsActive = productDto.is_Active,
                MinimumStock = productDto.minimum_stock
            };
        }
        /// <summary>
        /// Convierte un DTO de visualización de producto a un mensaje gRPC de visualización de producto.
        /// </summary>
        /// <param name="product">DTO de visualización de producto.</param>
        /// <returns>Mensaje gRPC de visualización de producto.</returns>
        public static ProductVisualizerMessage toVisualizerMessage(ProductVisualizerDto product)
        {
            return new ProductVisualizerMessage
            {
                Name = product.name,
                Stock = product.stock,
                MinimumStock = product.minimum_stock,
                IsActive = product.is_Active
            };
        }
    }
}