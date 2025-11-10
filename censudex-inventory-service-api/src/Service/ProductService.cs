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

        public Task UpdateStock(Guid productId, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("La cantidad debe ser un valor positivo");
            }
            
            return productRepository.UpdateStock(productId, quantity);
        }
    }
}