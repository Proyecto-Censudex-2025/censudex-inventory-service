using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;

namespace censudex_inventory_service_api.src.Helper.Mapper
{
    public class ProductMapper
    {
        public static ProductDto ToDto(Model.Product product)
        {
            return new ProductDto
            {
                id = product.id,
                name = product.name,
                category = product.category,
                stock = product.stock,
                is_Active = product.is_active,
                minimum_stock = product.minimum_stock 

            };
        }
        public static Model.Product ToModel(ProductDto productDto)
        {
            return new Model.Product
            {
                id = productDto.id,
                name = productDto.name,
                category = productDto.category,
                stock = productDto.stock,
                is_active = productDto.is_Active,
                minimum_stock = productDto.minimum_stock
            };
        }
    }
}