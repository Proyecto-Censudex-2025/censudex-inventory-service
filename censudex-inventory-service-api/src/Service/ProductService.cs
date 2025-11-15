using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;
using censudex_inventory_service_api.src.Repository;
using censudex_inventory_service_api.src.Helper.Mapper;
using censudex_inventory_service_api.src.Helper.Exception;
using MassTransit;
using censudex_inventory_service_api.src.Messages;
namespace censudex_inventory_service_api.src.Service
{
    /// <summary>
    /// Implementación del servicio de productos.
    /// </summary>
    public class ProductService : IProductService
    {
        /// <summary>
        /// Repositorio de productos para operaciones de datos.
        /// </summary>
        private readonly IProductRepository productRepository;
        /// <summary>
        /// Endpoint de publicación para enviar mensajes con RabbitMQ.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;
        /// <summary>
        /// Constructor que inicializa el servicio con el repositorio y el endpoint de publicación.
        /// </summary>
        /// <param name="productRepository">Repositorio de productos.</param>
        /// <param name="publishEndpoint">Endpoint de publicación para enviar mensajes con RabbitMQ.</param>
        public ProductService(IProductRepository productRepository, IPublishEndpoint publishEndpoint)
        {
            this.productRepository = productRepository;
            _publishEndpoint = publishEndpoint;
        }
        
        public async Task<ProductDto> AddProduct(ProductDto productDto)
        {
            if (productDto == null)
            {
                throw new ArgumentNullException("The product cannot be null");
            }
            if (productDto.id == Guid.Empty 
                || string.IsNullOrWhiteSpace(productDto.name) 
                || string.IsNullOrWhiteSpace(productDto.category)
                || string.IsNullOrWhiteSpace(productDto.stock.ToString()) 
                || string.IsNullOrWhiteSpace(productDto.is_Active.ToString())
                || string.IsNullOrWhiteSpace(productDto.minimum_stock.ToString()))
            {
                throw new ArgumentException("All fields must be filled");
            }
            if (productDto.stock < 0)
            {
                throw new ArgumentException("The stock must be a positive value");
            }
            var product = ProductMapper.ToModel(productDto);
            await productRepository.AddProduct(product);
            return productDto;
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

        public async Task<ProductVisualizerDto?> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("The product ID cannot be empty");
            }
            var product = await productRepository.GetProductById(id);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found");
            }
            var productVisualizer = ProductMapper.toVisualizer(product);
            return productVisualizer;
        }
        public async Task<ProductVisualizerDto> SetMinimumStock(Guid productId, int minimumStock)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("The product ID cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(minimumStock.ToString()))
            {
                throw new ArgumentException("The minimum stock cannot be null or empty");
            }
            if (minimumStock <= 0)
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
            
            if (product.stock <= product.minimum_stock)
            {
                var stockLowMessage = new StockLowMessage
                {
                    productId = product.id,
                    productName = product.name,
                    currentStock = product.stock,
                    minimumStock = product.minimum_stock,
                    reportedAt = DateTime.UtcNow
                };
                await _publishEndpoint.Publish(stockLowMessage);
            }
            
            var productVisualizer = ProductMapper.toVisualizer(product);
            return productVisualizer;
        }
        public async Task<ProductVisualizerDto> UpdateStock (Guid productId, int amount)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("The product ID cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(amount.ToString()))
            {
                throw new ArgumentException("The amount to update cannot be null or empty");
            }
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
                var orderFailedMessage = new OrderFailedStockMessage
                {
                    orderId = product.id,
                    reason = "Insufficient stock",
                    errored = true,
                    reportedAt = DateTime.UtcNow
                };
                await _publishEndpoint.Publish(orderFailedMessage);
                throw new InvalidOperationException("Insufficient stock available");
            }
            if (product.stock <= product.minimum_stock)
            {
                var stockLowMessage = new StockLowMessage
                {
                    productId = product.id,
                    productName = product.name,
                    currentStock = product.stock,
                    minimumStock = product.minimum_stock,
                    reportedAt = DateTime.UtcNow
                };
                await _publishEndpoint.Publish(stockLowMessage);
            }
            await productRepository.UpdateStock(productId, product.stock);
            var productVisualizer = ProductMapper.toVisualizer(product);
            return productVisualizer;
        }
    }
}