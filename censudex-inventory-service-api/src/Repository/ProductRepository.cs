using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Model;

namespace censudex_inventory_service_api.src.Repository
{
    /// <summary>
    /// Implementación del repositorio de productos.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// Cliente de Supabase para operaciones de base de datos.
        /// </summary>
        private readonly Supabase.Client supabase;
        /// <summary>
        /// Constructor que inicializa el repositorio con el cliente de Supabase.
        /// </summary>
        /// <param name="supabaseClient">Cliente de Supabase para operaciones de base de datos.</param>
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
        public async Task UpdateStock(Guid productId, int newStock)
        {
            var product = await GetProductById(productId);
            if (product != null)
            {
                product.stock = newStock;
                await supabase.From<Product>().Update(product);
            }
        }
        public async Task UpdateMinimumStock(Guid productId, int minimumStock)
        {
            var product = await GetProductById(productId);
            if (product != null)
            {
                product.minimum_stock = minimumStock;
                await supabase.From<Product>().Update(product);
            }
        }
    }
}