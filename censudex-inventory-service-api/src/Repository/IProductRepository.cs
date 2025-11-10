using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Model;

namespace censudex_inventory_service_api.src.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>?> GetAllProducts();
        Task<Product?> GetProductById(Guid id);
        Task AddProduct(Product product);
        Task UpdateStock(Guid productId, int newStock);
        Task UpdateMinimumStock(Guid productId, int minimumStock);
    }
}