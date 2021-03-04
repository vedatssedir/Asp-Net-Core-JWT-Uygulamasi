using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YSKProje.UdemyJwtProje.Business.Interfaces;
using YSKProje.UdemyJwtProje.Business.StringInfos;
using YSKProje.UdemyJwtProje.Entities.Concrete;
using YSKProje.UdemyJwtProje.Entities.Dtos.AppUserDtos;
using YSKProje.UdemyJwtProje.Entities.Token;
using YSKProje.UdemyJwtProje.WebApi.CustomFilters;

namespace YSKProje.UdemyJwtProje.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;
        public AuthController(IJwtService jwtService, IAppUserService appUserService, IMapper mapper)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _appUserService = appUserService;
        }

        [HttpPost("[action]")]
        [ValidModel]
        public async Task<IActionResult> SignIn(AppUserLoginDto appUserLoginDto)
        {
            var appUser = await _appUserService.FindByUserName(appUserLoginDto.UserName);
            if (appUser == null)
            {
                return BadRequest("kullanıcı adı veya şifre hatalı");
            }

            if (!await _appUserService.CheckPassword(appUserLoginDto))
                return BadRequest("kullanıcı adı veya şifre hatalı");
            var roles = await _appUserService.GetRolesByUserName(appUserLoginDto.UserName);
            var token = _jwtService.GenerateJwt(appUser, roles);
            var jwtAccessToken = new JwtAccessToken {Token = token};
            return Created("", jwtAccessToken);
        }

        [HttpPost("[action]")]
        [ValidModel]
        public async Task<IActionResult> SignUp(
            AppUserAddDto appUserAddDto,
            [FromServices] IAppUserRoleService appUserRoleService, 
            [FromServices] IAppRoleService appRoleService)
        {
            var appUser = await _appUserService.FindByUserName(appUserAddDto.UserName);
            if (appUser != null)
                return BadRequest($"{appUserAddDto.UserName} zaten alınmış");

            await _appUserService.Add(_mapper.Map<AppUser>(appUserAddDto));

            var user = await _appUserService.FindByUserName(appUserAddDto.UserName);
            var role = await appRoleService.FindByName(RoleInfo.Member);

            await appUserRoleService.Add(new AppUserRole
            {
                AppRoleId = role.Id,
                AppUserId = user.Id
            });
            return Created("", appUserAddDto);
        }



        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> ActiveUser()
        {
            var user = await _appUserService.FindByUserName(User.Identity.Name);
            var roles = await _appUserService.GetRolesByUserName(User.Identity.Name);

            var appUserDto = new AppUserDto
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Roles = roles.Select(I => I.Name).ToList()
            };
            return Ok(appUserDto);
        }

    }
}