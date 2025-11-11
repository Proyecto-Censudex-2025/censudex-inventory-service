using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;
using censudex_inventory_service_api.src.Repository;
using censudex_inventory_service_api.src.Helper.Mapper;
using censudex_inventory_service_api.src.Helper.Exception;
namespace censudex_inventory_service_api.src.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        
        public Task AddProduct(ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException("The product cannot be null");
            }
            if (productDto.stock < 0)
            {
                throw new ArgumentException("The stock must be a positive value");
            }
            var product = ProductMapper.ToModel(productDto);
            return productRepository.AddProduct(product);
        }

        public Task<IEnumerable<ProductDto?>> GetAllProducts()
        {
            var products = productRepository.GetAllProducts();
            var productDtos = products.Result?.Select(p => ProductMapper.ToDto(p));
            if (productDtos == null)
            {
                throw new ProductNotFoundException("No products found");
            }
            return Task.FromResult<IEnumerable<ProductDto?>>(productDtos);
        }

        public Task<ProductDto?> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("The product ID cannot be empty");
            }
            var product = productRepository.GetProductById(id);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }
            var productDto = product.Result != null ? ProductMapper.ToDto(product.Result) : null;
            return Task.FromResult(productDto);
        }
        public async Task SetMinimumStock(Guid productId, int minimumStock)
        {
            if (minimumStock < 0)
            {
                throw new ArgumentException("The minimum stock must be a positive value and different from 0");
            }
            var product = await productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }
            product.minimum_stock = minimumStock;
            await productRepository.UpdateMinimumStock(productId, product.minimum_stock);
        }
        public async Task UpdateStock (Guid productId, int amount)
        {
            if (amount == 0)
            {
                throw new ArgumentException("The amount to update must be different from 0");
            }

            var product = await productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }

            product.stock += amount;

            if (product.stock < 0)
            {
                throw new InvalidOperationException("Insufficient stock available");
            }
            if (product.stock <= product.minimum_stock)
            {
                //TODO ENVIAR ALERTA DE UMBRAL MINIMO NO SUPERADO
            }
            if (product.stock > product.minimum_stock)
            {
                //TODO ENVIAR ALERTA DE UMBRAL MINIMO SUPERADO
            }
            await productRepository.UpdateStock(productId, product.stock);
        }
    }
}