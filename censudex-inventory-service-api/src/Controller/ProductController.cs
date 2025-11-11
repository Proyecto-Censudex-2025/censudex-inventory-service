using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Helper.Exception;
using censudex_inventory_service_api.src.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace censudex_inventory_service_api.src.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                if (ex is ProductNotFoundException)
                {
                    return StatusCode(404, new { ex.Message });
                }
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await productService.GetProductById(id);
                if (product == null)
                {
                    return StatusCode(404, new { Message = "Product not found" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                if (ex is ProductNotFoundException)
                {
                    return StatusCode(404, new { ex.Message });
                }
                else if (ex is ArgumentException)
                {
                    return BadRequest(new { ex.Message });
                }
                else
                {
                    return StatusCode(500, "An error occurred while retrieving the product");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Dto.ProductDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    return StatusCode(400, new { Message = "Product data is required" });
                }
                if (string.IsNullOrEmpty(productDto.name) || string.IsNullOrEmpty(productDto.category) ||
                    string.IsNullOrEmpty(productDto.id.ToString()) || string.IsNullOrEmpty(productDto.stock.ToString()) ||
                    string.IsNullOrEmpty(productDto.minimum_stock.ToString()) || string.IsNullOrEmpty(productDto.is_Active.ToString()))
                {
                    return StatusCode(400, new { Message = "All product fields must be provided" });
                }
                await productService.AddProduct(productDto);
                return CreatedAtAction(nameof(GetProductById), new { id = productDto.id }, productDto);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(new { ex.Message });
                }
                else if (ex is ArgumentNullException)
                {
                    return BadRequest(new { ex.Message });
                }
                else
                {
                    return StatusCode(500, new { Message = "An error occurred while adding the product" });
                }
            }
        }
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(Guid id, [FromQuery] int amount)
        {
            try
            {
                await productService.UpdateStock(id, amount);
                return Ok("Stock updated successfully by " + Math.Abs(amount));
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(new { ex.Message });
                }
                else if (ex is ProductNotFoundException)
                {
                    return StatusCode(404, new { ex.Message });
                }
                else if (ex is InvalidOperationException)
                {
                    return BadRequest(new { ex.Message });
                }
                else
                {
                    return StatusCode(500, new { Message = "An error occurred while updating the stock" });
                }
            }
        }
        [HttpPatch("{id}/minStock")]
        public async Task<IActionResult> SetMinimumStock(Guid id, [FromQuery] int minimumStock)
        {
            try
            {
                await productService.SetMinimumStock(id, minimumStock);
                return Ok("Minimum stock set successfully to " + minimumStock);
            }

            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return BadRequest(new { ex.Message });
                }
                else if (ex is ProductNotFoundException)
                {
                    return StatusCode(404, new { ex.Message });
                }
                else
                {
                    return StatusCode(500, new { Message = "An error occurred while setting the minimum stock" });
                }
            }
        }
    }
}