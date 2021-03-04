﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YSKProje.UdemyJwtProje.Business.Interfaces;
using YSKProje.UdemyJwtProje.Business.StringInfos;
using YSKProje.UdemyJwtProje.Entities.Concrete;
using YSKProje.UdemyJwtProje.Entities.Dtos.ProductDtos;
using YSKProje.UdemyJwtProje.WebApi.CustomFilters;

namespace YSKProje.UdemyJwtProje.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _mapper = mapper;
            _productService = productService;
        }

        //api/products
        [HttpGet]
        [Authorize(Roles = RoleInfo.Admin + "," + RoleInfo.Member)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        //api/products/3
        //ValidProductId ValidAppUserId  ValidId<Product> ValidId<AppUser>
        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidId<Product>))]
        [Authorize(Roles=RoleInfo.Admin)]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = RoleInfo.Admin)]
        [ValidModel]
        public async Task<IActionResult> Add(ProductAddDto productAddDto)
        {
            await _productService.Add(_mapper.Map<Product>(productAddDto));
            return Created("", productAddDto);
        }

        [HttpPut]
        [Authorize(Roles = RoleInfo.Admin)]
        [ValidModel]      
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            await _productService.Update(_mapper.Map<Product>(productUpdateDto));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleInfo.Admin)]
        [ServiceFilter(typeof(ValidId<Product>))]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Remove(new Product() { Id = id });
            return NoContent();
        }

        [HttpGet("test/{id}")]
        [ServiceFilter(typeof(ValidId<AppUser>))]
        public IActionResult Test(int id)
        {
            return Ok();
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            var errorInfo = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //loglama


            return Problem(detail: "api da bir hata olustu, en kisa zamanda düzeltilecek");
        }
    }
}