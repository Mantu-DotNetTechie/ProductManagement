using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<int> GetTotalCountAsync();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetSortedProductsAsync(string sortBy, bool isAscending); 
        Task<bool> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task DeleteAllProductsAsync();
    }
}