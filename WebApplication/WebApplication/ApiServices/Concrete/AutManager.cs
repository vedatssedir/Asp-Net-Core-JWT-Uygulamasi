using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApplication.Models;

namespace WebApplication.ApiServices.Concrete
{
    public class AutManager : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AutManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Login(AppUserLogin appUserLogin)
        {
            var jsonData = JsonConvert.SerializeObject(appUserLogin);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            using var httClient = new HttpClient();
            var responseMessage = await httClient.PostAsync("http://localhost:53308/api/Auth/SignIn", stringContent);
            var result = responseMessage.IsSuccessStatusCode;
            switch (result)
            {
                case true:
                {
                    var token = JsonConvert.DeserializeObject<AccessToken>(await responseMessage.Content
                        .ReadAsStringAsync());
                    _httpContextAccessor.HttpContext?.Session.SetString("token", token.Token);
                    break;
                }
                case false:
                {
                    break;
                }
            }

            return result;
        }

        public void LogOut()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("token");
        }
    }
}