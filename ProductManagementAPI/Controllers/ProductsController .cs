using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController (ILoggerFactory loggerFactory, IProductService productService)
        {
            _logger = loggerFactory.CreateLogger<ProductsController>();
            _productService = productService;

            _logger.LogInformation("ProductsController initialized");
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);          
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName([FromQuery] string name)
        {
            var products = await _productService.GetProductsByNameAsync(name);
            return Ok(products);
        }

        [HttpGet("total-count")]
        public async Task<ActionResult<int>> GetTotalCount()
        {
            var count = await _productService.GetTotalCountAsync();
            return Ok(count);
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var products = await _productService.GetProductsByCategoryAsync(category);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Product>>> GetSortedProducts([FromQuery] string sortBy, [FromQuery] bool isAscending)
        {
            var products = await _productService.GetSortedProductsAsync(sortBy, isAscending);
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (await _productService.UpdateProductAsync(id, product))
                return NoContent();

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (await _productService.DeleteProductAsync(id))
                return NoContent();

            return NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllProducts()
        {
            await _productService.DeleteAllProductsAsync();
            return NoContent();
        }
    }
}