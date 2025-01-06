

using ProductManagementModel.Models;
using System.Text.Json;


namespace ProductManagementServices.Services
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly string _jsonFilePath = Path.Combine( Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "ProductManagementData\\Data\\Products.json");

        
        public async Task<List<Product>> GetAllProducts()
        {
            if (!File.Exists(_jsonFilePath)) return new List<Product>();
            var json = await File.ReadAllTextAsync(_jsonFilePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        public async Task<Product> GetProductById(int id)
        {
            var products = await GetAllProducts();
            return products.FirstOrDefault(p => p.Id == id);  
        }

        public async Task AddProduct(Product product)
        {
            var products = await GetAllProducts();
            product.Id = products.Any() ? products.Max(i => i.Id) + 1 : 1;

            products.Add(product);
            await WriteToFileAsync(products);

        }

        public async Task UpdateProduct(Product product)
        {
            var items = await GetAllProducts();
            var index = items.FindIndex(i => i.Id == product.Id);
            if (index >= 0)
            {
                items[index] = product;
                await WriteToFileAsync(items);
            }
        }


        private async Task WriteToFileAsync(List<Product> products)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }

        public async Task DeleteProduct(int id)
        {
            var products = await GetAllProducts();
            var itemToRemove = products.FirstOrDefault(i => i.Id == id);
            if (itemToRemove != null)
            {
                products.Remove(itemToRemove);
                await File.WriteAllTextAsync(_jsonFilePath, System.Text.Json.JsonSerializer.Serialize(products));
            }
        }
    }

}
