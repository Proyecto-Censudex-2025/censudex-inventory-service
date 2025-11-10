using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;

namespace censudex_inventory_service_api.src.Service
{
    public interface IProductService
    {
        public Task AddProduct(ProductDto productDto);
        public Task<IEnumerable<ProductDto>?> GetAllProducts();
        public Task<ProductDto?> GetProductById(Guid id);
        public Task IncrementStock(Guid productId, int amount);
        public Task DecrementStock(Guid productId, int amount);
        public Task SetMinimumStock(Guid productId, int minimumStock);
        //TODO ALERTA DE UMBRAL MINIMO SUPERADO/NO ALCANZADO mediante un bool
    }
}