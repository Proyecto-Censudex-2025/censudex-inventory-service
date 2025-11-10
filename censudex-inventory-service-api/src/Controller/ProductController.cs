using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using censudex_inventory_service_api.src.Service;
using Microsoft.AspNetCore.Mvc;

namespace censudex_inventory_service_api.src.Controller
{
    //TODO Mensajes de excepción
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
                Debug.WriteLine($"Error in GetAllProducts: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetProductById: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Dto.ProductDto productDto)
        {
            try
            {
                await productService.AddProduct(productDto);
                return CreatedAtAction(nameof(GetProductById), new { id = productDto.id }, productDto);
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Error in AddProduct: {ex.Message}");
                return BadRequest("Product data is null");
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"Error in AddProduct: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddProduct: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("{id}/incrementStock")]
        public async Task<IActionResult> IncrementStock(Guid id, [FromQuery] int amount)
        {
            try
            {
                await productService.IncrementStock(id, amount);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"Error in IncrementStock: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in IncrementStock: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("{id}/decrementStock")]
        public async Task<IActionResult> DecrementStock(Guid id, [FromQuery] int amount)
        {
            try
            {
                await productService.DecrementStock(id, amount);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"Error in DecrementStock: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in DecrementStock: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("{id}/setMinimumStock")]
        public async Task<IActionResult> SetMinimumStock(Guid id, [FromQuery] int minimumStock)
        {
            try
            {
                await productService.SetMinimumStock(id, minimumStock);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"Error in SetMinimumStock: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SetMinimumStock: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}