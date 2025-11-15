using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;

namespace censudex_inventory_service_api.src.Service
{
    public interface IProductService
    {
        public Task<ProductDto> AddProduct(ProductDto productDto);
        public Task<IEnumerable<ProductDto?>> GetAllProducts();
        public Task<ProductVisualizerDto?> GetProductById(Guid id);
        public Task<ProductVisualizerDto> UpdateStock(Guid productId, int amount);
        public Task<ProductVisualizerDto> SetMinimumStock(Guid productId, int minimumStock);
    }
}