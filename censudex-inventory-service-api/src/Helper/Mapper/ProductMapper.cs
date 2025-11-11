using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Dto;
using censudex_inventory_service_api.src.Model;

namespace censudex_inventory_service_api.src.Helper.Mapper
{
    public class ProductMapper
    {
        public static ProductDto ToDto(Product product)
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
        public static Product ToModel(ProductDto productDto)
        {
            return new Product
            {
                id = productDto.id,
                name = productDto.name,
                category = productDto.category,
                stock = productDto.stock,
                is_active = productDto.is_Active,
                minimum_stock = productDto.minimum_stock
            };
        }

        public static getProductDto toVisualizer(Product product)
        {
            return new getProductDto
            {
                name = product.name,
                stock = product.stock,
                minimum_stock = product.minimum_stock,
                is_Active = product.is_active
            };
        }
    }
}