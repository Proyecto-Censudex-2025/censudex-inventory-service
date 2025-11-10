using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;
using censudex_inventory_service_api.src.Repository;

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
            var product = Helper.Mapper.ProductMapper.ToModel(productDto);
            return productRepository.AddProduct(product);
        }

        public Task<IEnumerable<ProductDto>?> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto?> GetProductById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStock(Guid productId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}