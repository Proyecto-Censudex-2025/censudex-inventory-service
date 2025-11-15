using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Helper.Exception;
using censudex_inventory_service_api.src.Helper.Mapper;
using censudex_inventory_service_api.src.Service;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InventoryService.Grpc;
namespace censudex_inventory_service_api.src.Controller
{
    public class ProductGrpcService : Inventory.InventoryBase
    {
        private readonly IProductService _productService;
        public ProductGrpcService(IProductService productService)
        {
            _productService = productService;
        }
        public override async Task<AddProductResponse> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Product == null)
                {
                    throw new ArgumentNullException("The product cannot be null");
                }
                var productDto = ProductMapper.toDto(request.Product);
                var addedProduct = await _productService.AddProduct(productDto);
                var response = new AddProductResponse
                {
                    Product = ProductMapper.toMessage(addedProduct),
                    Message = "Product added successfully"
                };
                return response;
            }
            catch (ArgumentNullException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (ArgumentException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
        public override async Task<GetAllProductsResponse> GetAllProducts(Empty request, ServerCallContext context)
        {
            try
            {
                var products = await _productService.GetAllProducts();
                var response = new GetAllProductsResponse();
                response.Products.AddRange(products.Select(p => ProductMapper.toMessage(p)));
                return response;
            }
            catch (ProductNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "A unexpected error occurred while retrieving products."));
            }
        }
        public override async Task<GetProductByIdResponse> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.ProductId, out Guid productId))
                {
                    throw new ArgumentException("Invalid product ID format.");
                }
                var product = await _productService.GetProductById(productId);
                var response = new GetProductByIdResponse
                {
                    Product = ProductMapper.toVisualizerMessage(product)
                };
                return response;
            }
            catch (ArgumentException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (ProductNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "A unexpected error occurred while retrieving the product."));
            }
        }
        public override async Task<UpdateStockResponse> UpdateStock(UpdateStockRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.ProductId, out Guid productId))
                {
                    throw new ArgumentException("Invalid product ID format.");
                }
                var updatedProduct = await _productService.UpdateStock(productId, request.Amount);
                var response = new UpdateStockResponse
                {
                    Product = ProductMapper.toVisualizerMessage(updatedProduct),
                    Message = "Product stock updated successfully"
                };
                return response;
            }
            catch (ArgumentException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (ProductNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "A unexpected error occurred while updating the product."));
            }
        }
        public override async Task<SetMinimumStockResponse> SetMinimumStock(SetMinimumStockRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.ProductId, out Guid productId))
                {
                    throw new ArgumentException("Invalid product ID format.");
                }
                var updatedProduct = await _productService.SetMinimumStock(productId, request.MinimumStock);
                var response = new SetMinimumStockResponse
                {
                    Product = ProductMapper.toVisualizerMessage(updatedProduct),
                    Message = "Minimum stock set successfully"
                };
                return response;
            }
            catch (ArgumentException ex)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (ProductNotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "A unexpected error occurred while setting the minimum stock."));
            }
        }
    }
}