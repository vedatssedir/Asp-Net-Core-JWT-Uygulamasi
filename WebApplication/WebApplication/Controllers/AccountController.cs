using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.ApiServices.Concrete;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AppUserLogin appUserLogin)
        {
            if (ModelState.IsValid)
            {
                var result =await _authService.Login(appUserLogin);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(appUserLogin);
        }
    }
}