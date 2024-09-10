using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    /// <summary>
    /// Represents a service for managing products.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public ProductService(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<ProductService>();
        }

        /// <summary>
        /// Adds a new product asynchronously.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Product added successfully.");
        }

        /// <summary>
        /// Deletes all products asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            _logger.LogInformation("All products deleted successfully.");
        }

        /// <summary>
        /// Deletes a product by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a value indicating whether the product was deleted successfully.</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                _logger.LogInformation($"Product with ID {id} not found.");
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Product with ID {id} deleted successfully.");
            return true;
        }

        /// <summary>
        /// Gets a product by ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to get.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the product with the specified ID, or null if not found.</returns>
        public async Task<Product> GetProductByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching product with ID {id}.");
            return await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Gets all products asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of all products.</returns>
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            _logger.LogInformation("Fetching all products.");
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Gets products by category asynchronously.
        /// </summary>
        /// <param name="category">The category of the products to get.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of products with the specified category.</returns>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            _logger.LogInformation($"Fetching products by category: {category}.");
            return await _context.Products.Where(p => p.Category == category).ToListAsync();
        }

        /// <summary>
        /// Gets products by name asynchronously.
        /// </summary>
        /// <param name="name">The name of the products to get.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of products with the specified name.</returns>
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            _logger.LogInformation($"Fetching products by name: {name}.");
            return await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
        }

        /// <summary>
        /// Gets sorted products asynchronously.
        /// </summary>
        /// <param name="sortBy">The property to sort by.</param>
        /// <param name="isAscending">A value indicating whether to sort in ascending order.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of sorted products.</returns>
        public async Task<IEnumerable<Product>> GetSortedProductsAsync(string sortBy, bool isAscending)
        {
            _logger.LogInformation($"Fetching sorted products by {sortBy} in {(isAscending ? "ascending" : "descending")} order.");
            var products = _context.Products.AsQueryable();
            if (isAscending)
            {
                products = sortBy switch
                {
                    "name" => products.OrderBy(p => p.Name),
                    "category" => products.OrderBy(p => p.Category),
                    "price" => products.OrderBy(p => p.Price),
                    _ => products
                };
            }
            else
            {
                products = sortBy switch
                {
                    "name" => products.OrderByDescending(p => p.Name),
                    "category" => products.OrderByDescending(p => p.Category),
                    "price" => products.OrderByDescending(p => p.Price),
                    _ => products
                };
            }
            return await products.ToListAsync();
        }

        /// <summary>
        /// Gets the total count of products asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains the total count of products.</returns>
        public async Task<int> GetTotalCountAsync()
        {
            _logger.LogInformation("Fetching total count of products.");
            return await _context.Products.CountAsync();
        }

        /// <summary>
        /// Updates a product asynchronously.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="product">The updated product.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a value indicating whether the product was updated successfully.</returns>
        public async Task<bool> UpdateProductAsync(int id, Product product)
        {
            if (id != product.Id)
            {
                _logger.LogInformation($"Product ID {id} does not match the ID of the provided product.");
                return false;
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product with ID {id} updated successfully.");
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    _logger.LogInformation($"Product with ID {id} not found.");
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the product.");
                throw new Exception("An error occurred while updating the product.", ex);
            }
        }

        private bool ProductExists(int id) => _context.Products.Any(e => e.Id == id);
    }
}