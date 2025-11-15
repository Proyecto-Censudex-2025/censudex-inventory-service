using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Messages;
using censudex_inventory_service_api.src.Service;
using MassTransit;

namespace censudex_inventory_service_api.src.Consumer
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedMessage>
    {
        private readonly IProductService _productService;
        public OrderCreatedConsumer(IProductService productService)
        {
            _productService = productService;
        }
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