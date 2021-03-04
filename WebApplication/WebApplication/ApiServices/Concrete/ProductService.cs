using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApplication.Models;

namespace WebApplication.ApiServices.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ProductList>> GetAllAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            List<ProductList> productLists = null;
            switch (string.IsNullOrWhiteSpace(token))
            {
                case false:
                {
                    using var httClient = new HttpClient();
                    httClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = await httClient.GetAsync("http://localhost:53308/api/products");
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        productLists =
                            JsonConvert.DeserializeObject<List<ProductList>>(await responseMessage.Content
                                .ReadAsStringAsync());
                    }

                    break;
                }
                case true:
                {
                    break;
                }
            }

            return productLists;
        }

        public async Task AddProduct(ProductAdd productAdd)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            switch (!string.IsNullOrWhiteSpace(token))
            {
                case true:
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsonData = JsonConvert.SerializeObject(productAdd);
                    var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var responseMessage =
                        await httpClient.PostAsync("http://localhost:53308/api/products", stringContent);
                    break;
                }
                case false:
                {
                    break;
                }
            }
        }

        public async Task<ProductList> GetById(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            ProductList productList = null;
            switch (!string.IsNullOrWhiteSpace(token))
            {
                case true:
                {
                    using var httClient = new HttpClient();
                    httClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var responseMessage = await httClient.GetAsync($"http://localhost:53308/api/products/{id}");
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        productList =
                            JsonConvert.DeserializeObject<ProductList>(
                                await responseMessage.Content.ReadAsStringAsync());
                    }

                    break;
                }
                case false:
                {
                    break;
                }
            }
            return productList;
        }
        public async Task UpdateAsync(ProductList productList)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            switch (!string.IsNullOrWhiteSpace(token))
            {
                case true:
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var jsonData = JsonConvert.SerializeObject(productList);
                    var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var responseMessage =
                        await httpClient.PutAsync("http://localhost:53308/api/products", stringContent);
                    break;
                }
                case false:
                {
                    break;
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("token");
            switch (!string.IsNullOrWhiteSpace(token))
            {
                case true:
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    await httpClient.DeleteAsync($"http://localhost:53308/api/products/{id}");
                    break;
                }
                case false:
                {
                    break;
                }
            }
        }
    }
}