using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Helper.Exception;
using censudex_inventory_service_api.src.Messages;
using censudex_inventory_service_api.src.Service;
using MassTransit;
using MassTransit.SagaStateMachine;

namespace censudex_inventory_service_api.src.Consumer
{
    /// <summary>
    /// Consumidor de eventos para la creación de órdenes.
    /// </summary>
    public class OrderCreatedConsumer : IConsumer<OrderCreatedMessage>
    {
        /// <summary>
        /// Servicio de productos utilizado por el consumidor.
        /// </summary>
        private readonly IProductService _productService;
        /// <summary>
        /// Constructor del consumidor de eventos de orden creada.
        /// </summary>
        /// <param name="productService">Servicio de productos.</param>
        public OrderCreatedConsumer(IProductService productService)
        {
            _productService = productService;
        }
        /// <summary>
        /// Método que consume el evento de orden creada.
        /// </summary>
        /// <param name="context">Contexto del consumidor que contiene el mensaje de orden creada.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<OrderCreatedMessage> context)
        {
            var orderEvent = context.Message;
            var orderId = orderEvent.orderId;

            try
            {
                foreach (var item in orderEvent.products)
                {
                var productId = item.Key;
                var quantity = item.Value;
                await _productService.UpdateStock(productId, -quantity, orderId);
                }
            }
            catch (ProductNotFoundException ex)
            {
                var orderFailedMessage = new OrderFailedStockMessage
                {
                    orderId = orderId,
                    reason = ex.Message,
                    errored = true,
                    reportedAt = DateTime.UtcNow
                };
                Console.WriteLine($"Stock update failed for order {orderId}: {ex.Message}");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine($"Failed to process order {orderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing order {orderId}: {ex.Message}");
                throw;
            }
            
            return;
        }
    }
}