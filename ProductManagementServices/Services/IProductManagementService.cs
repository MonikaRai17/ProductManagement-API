
using ProductManagementModel.Models;

namespace ProductManagementServices.Services
{
    public interface IProductManagementService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);


    }
}
