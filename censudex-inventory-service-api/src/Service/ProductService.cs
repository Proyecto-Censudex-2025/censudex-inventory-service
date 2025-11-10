using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;
using censudex_inventory_service_api.src.Repository;
using censudex_inventory_service_api.src.Helper.Mapper;
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
                throw new ArgumentNullException(nameof(productDto));
            }
            if (productDto.stock < 0)
            {
                throw new ArgumentException("El Stock debe ser un valor positivo");
            }
            var product = ProductMapper.ToModel(productDto);
            return productRepository.AddProduct(product);
        }

        public Task<IEnumerable<ProductDto>?> GetAllProducts()
        {
            var products = productRepository.GetAllProducts();
            var productDtos = products.Result?.Select(p => ProductMapper.ToDto(p));
            return Task.FromResult(productDtos);
        }

        public Task<ProductDto?> GetProductById(Guid id)
        {
            var product = productRepository.GetProductById(id);
            var productDto = product.Result != null ? ProductMapper.ToDto(product.Result) : null;
            return Task.FromResult(productDto);
        }
        public async Task IncrementStock(Guid productId, int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("La cantidad a incrementar debe ser un valor positivo y diferente de 0");
            }
            var product = await productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            product.stock += amount;
            if (product.stock > product.minimum_stock)
            {
                //TODO ENVIAR ALERTA DE UMBRAL MINIMO SUPERADO
            }
            await productRepository.UpdateStock(productId, product.stock);
        }
        public async Task DecrementStock(Guid productId, int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("La cantidad a decrementar debe ser un valor positivo y diferente de 0");
            }
            var product = await productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            product.stock -= amount;
            if (product.stock < 0)
            {
                throw new InvalidOperationException("No quedan suficientes unidades en stock");
            }
            if (product.stock <= product.minimum_stock)
            {
                //TODO ENVIAR ALERTA DE UMBRAL MINIMO NO SUPERADO
            }
            await productRepository.UpdateStock(productId, product.stock);
        }
        public async Task SetMinimumStock(Guid productId, int minimumStock)
        {
            if (minimumStock < 0)
            {
                throw new ArgumentException("El stock minimo debe ser un valor positivo y diferente de 0");
            }
            var product = await productRepository.GetProductById(productId);
            if (product == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }
            product.minimum_stock = minimumStock;
            await productRepository.UpdateMinimumStock(productId, product.minimum_stock);
        }

    }
}