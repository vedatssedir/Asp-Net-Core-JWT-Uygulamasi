using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ApiServices.Concrete
{
    public interface IProductService
    {
        Task<List<ProductList>> GetAllAsync();
        Task AddProduct(ProductAdd productAdd);
        Task<ProductList> GetById(int id);
        Task UpdateAsync(ProductList productList);
        Task DeleteAsync(int id);
    }
}