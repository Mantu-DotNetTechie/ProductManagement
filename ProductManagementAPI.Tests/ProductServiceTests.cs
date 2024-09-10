using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagementAPI.Data;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductManagementAPI.Tests
{
    public class ProductServiceTests
    {
        private ProductService _productService;
        private ApplicationDbContext _context;
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly Mock<ILogger<ProductService>> _mockLogger;

        public ProductServiceTests()
        {
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            _mockLogger = new Mock<ILogger<ProductService>>();

            _mockLoggerFactory.Setup(factory => factory.CreateLogger(It.IsAny<string>()))
                .Returns(_mockLogger.Object);
        }

        private void InitializeDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _productService = new ProductService(_context, _mockLoggerFactory.Object);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            InitializeDatabase();

            var product = new Product { Id = 1, Name = "Test Product", Category = "Test Category", Price = 10.0M };

            await _productService.AddProductAsync(product);

            var addedProduct = await _context.Products.FindAsync(1);
            Assert.NotNull(addedProduct);
            Assert.Equal("Test Product", addedProduct.Name);
        }

        [Fact]
        public async Task DeleteAllProductsAsync_ShouldDeleteAllProducts()
        {
            InitializeDatabase();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Test Product 1", Category = "Test Category 1", Price = 10.0M },
                new Product { Id = 2, Name = "Test Product 2", Category = "Test Category 2", Price = 20.0M }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            await _productService.DeleteAllProductsAsync();

            var result = await _context.Products.ToListAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            InitializeDatabase();

            var product = new Product { Id = 1, Name = "Test Product", Category = "Test Category", Price = 10.0M };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var result = await _productService.DeleteProductAsync(1);

            Assert.True(result);
            var deletedProduct = await _context.Products.FindAsync(1);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct()
        {
            InitializeDatabase();

            var product = new Product { Id = 1, Name = "Test Product", Category = "Test Category", Price = 10.0M };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var result = await _productService.GetProductByIdAsync(1);

            Assert.Equal(product, result);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnAllProducts()
        {
            InitializeDatabase();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Test Product 1", Category = "Test Category 1", Price = 10.0M },
                new Product { Id = 2, Name = "Test Product 2", Category = "Test Category 2", Price = 20.0M }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            var result = await _productService.GetProductsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductsByCategoryAsync_ShouldReturnProductsByCategory()
        {
            InitializeDatabase();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Test Product 1", Category = "Category 1", Price = 10.0M },
                new Product { Id = 2, Name = "Test Product 2", Category = "Category 2", Price = 20.0M }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            var result = await _productService.GetProductsByCategoryAsync("Category 1");

            Assert.Single(result);
            Assert.Equal("Category 1", result.First().Category);
        }

        [Fact]
        public async Task GetProductsByNameAsync_ShouldReturnProductsByName()
        {
            InitializeDatabase();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Category = "Category 1", Price = 10.0M },
                new Product { Id = 2, Name = "Product 2", Category = "Category 2", Price = 20.0M }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            var result = await _productService.GetProductsByNameAsync("Product 1");

            Assert.Single(result);
            Assert.Equal("Product 1", result.First().Name);
        }

        [Fact]
        public async Task GetSortedProductsAsync_ShouldReturnSortedProducts()
        {
            InitializeDatabase();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "B Product", Category = "Category 1", Price = 10.0M },
                new Product { Id = 2, Name = "A Product", Category = "Category 2", Price = 20.0M }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            var result = await _productService.GetSortedProductsAsync("name", true);

            Assert.Equal(2, result.Count());
            Assert.Equal("A Product", result.First().Name);
        }

        [Fact]
        public async Task GetTotalCountAsync_ShouldReturnTotalCount()
        {
            InitializeDatabase();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Category = "Category 1", Price = 10.0M },
                new Product { Id = 2, Name = "Product 2", Category = "Category 2", Price = 20.0M }
            };

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();

            var result = await _productService.GetTotalCountAsync();

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            InitializeDatabase();

            var product = new Product { Id = 1, Name = "Product 1", Category = "Category 1", Price = 10.0M };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            product.Name = "Updated Product";
            var result = await _productService.UpdateProductAsync(1, product);

            Assert.True(result);
            var updatedProduct = await _context.Products.FindAsync(1);
            Assert.Equal("Updated Product", updatedProduct?.Name);
        }
    }
}