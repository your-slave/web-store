using Store.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Domain.Abstract
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<bool> SaveProductAsync(Product product);///

        Task<Product> DeleteProductAsync(string productID);

        Task<Product> GetProductById(string productId);

        Task<IEnumerable<string>> GetCategories();

        Task<IEnumerable<Product>> GetProductsByCategory(string category);
    }
}