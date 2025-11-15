using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Messages;
using censudex_inventory_service_api.src.Service;
using MassTransit;

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
            foreach (var item in orderEvent.products)
            {
                var productId = item.Key;
                var quantity = item.Value;
                await _productService.UpdateStock(productId, -quantity);
            }
            return;
        }
    }
}