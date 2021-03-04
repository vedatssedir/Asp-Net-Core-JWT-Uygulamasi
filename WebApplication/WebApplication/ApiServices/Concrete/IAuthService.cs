using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.ApiServices.Concrete
{
    public interface IAuthService
    {
        Task<bool> Login(AppUserLogin appUserLogin);
        void LogOut();
    }
}