using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication.ApiServices.Concrete;
using WebApplication.CustomFilters;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [JwtAuthorize(Roles="Admin")]
        public async Task<IActionResult> Index()
        {
            var data = await _productService.GetAllAsync();


            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [JwtAuthorize(Roles="Admin")]
        public async Task<IActionResult> Create(ProductAdd productAdd)
        {
            if (!ModelState.IsValid) return View(productAdd);
            await _productService.AddProduct(productAdd);
            return RedirectToAction("Index", "Home");
        }
        [JwtAuthorize(Roles = "Member")]
        public async Task<IActionResult> Update(int id)
        {
            return View(await _productService.GetById(id));
        }

        [HttpPost]
        [JwtAuthorize(Roles="Member")]
        public async Task<IActionResult> Update(ProductList productList)
        {
            if (!ModelState.IsValid) return View(productList);
            await _productService.UpdateAsync(productList);
            return RedirectToAction(nameof(Index));
        }
        [JwtAuthorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction("Index","Home");
        }
        
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}