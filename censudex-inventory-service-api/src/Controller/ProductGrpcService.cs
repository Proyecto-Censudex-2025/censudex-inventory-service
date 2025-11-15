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
    /// <summary>
    /// Controlador encargado de la gestión de productos a través de gRPC.
    /// </summary>
    public class ProductGrpcService : Inventory.InventoryBase
    {
        /// <summary>
        /// Servicio de productos utilizado por el controlador.
        /// </summary>
        private readonly IProductService _productService;
        /// <summary>
        /// Constructor del controlador de productos gRPC.
        /// </summary>
        /// <param name="productService"></param>
        public ProductGrpcService(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Endpoint para agregar un nuevo producto al inventario.
        /// </summary>
        /// <param name="request">Solicitud con los datos del producto recibidos</param>
        /// <param name="context"></param>
        /// <returns>Retorna la respuesta con el producto agregado</returns>
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
        /// <summary>
        /// Endpoint para obtener todos los productos del inventario.
        /// </summary>
        /// <param name="request">Solicitud vacía</param>
        /// <param name="context"></param>
        /// <returns>Retorna la respuesta con la lista de productos</returns>
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
        /// <summary>
        /// Endpoint para obtener un producto por su ID.
        /// </summary>
        /// <param name="request">Solicitud con el ID del producto</param>
        /// <param name="context"></param>
        /// <returns>Retorna la respuesta con el producto solicitado</returns>
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
        /// <summary>
        /// Endpoint para actualizar el stock de un producto.
        /// </summary>
        /// <param name="request">Solicitud con el ID del producto y la cantidad a actualizar</param>
        /// <param name="context"></param>
        /// <returns>Retorna la respuesta con el producto actualizado</returns>
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
        /// <summary>
        /// Endpoint para establecer el stock mínimo de un producto.
        /// </summary>
        /// <param name="request">Solicitud con el ID del producto y el stock mínimo a establecer</param>
        /// <param name="context"></param>
        /// <returns>Retorna la respuesta con el producto actualizado</returns>
        /// <exception cref="RpcException"></exception>
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