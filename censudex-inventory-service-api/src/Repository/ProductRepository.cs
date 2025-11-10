using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Model;

namespace censudex_inventory_service_api.src.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Supabase.Client supabase;
        public ProductRepository(Supabase.Client supabaseClient)
        {
            supabase = supabaseClient;
        }

        public async Task AddProduct(Product product)
        {
            await supabase.From<Product>().Insert(product);
        }

        public async Task<IEnumerable<Product>?> GetAllProducts()
        {
            var results = await supabase.From<Product>().Get();
            return results.Models;
        }

        public async Task<Product?> GetProductById(Guid id)
        {
            var results = await supabase.From<Product>().Where(p => p.id == id).Single();
            return results;
        }

        public async Task UpdateStock(Guid productId, int quantity)
        {
            var product = await supabase.From<Product>().Where(p => p.id == productId).Single();
            if (product == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            await supabase.From<Product>().Update(new Product
            {
                id = product.id,
                name = product.name,
                category = product.category,
                stock = quantity,
                is_active = product.is_active
            });
        }
    }
}