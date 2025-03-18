using AutoMapper;
using IdentitiyExample.Models;
using IdentitiyExample.Validators;
using IdentityExample.DTOs;
using IdentityExample.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public RoleController(RoleManager<AppRole> roleManager, IMapper mapper, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var map = _mapper.Map<List<GetAllRoleDtos>>(roles);
            return Ok(map);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var findrole = await _roleManager.FindByIdAsync(id);
            if (findrole == null)
            {
                return BadRequest(new { Message = "Bu id e ait rol bulunamadi!" });

            }
            IdentityResult result = await _roleManager.DeleteAsync(findrole);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "silinme isleminde bir sorun olustu tekrar deneyiniz!" });
            }
            return Ok(new { Message = "Rol basariyla silindi" });
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateDto request, CancellationToken token)
        {
            var validator = new RoleCreateValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors.Select(p => p.ErrorMessage) });
            }
            AppRole role = new AppRole
            {
                Name = request.roleName,
            };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Errors });
            }
            return Ok(new { Message = "Rol kayit islemi basarili" });
        }




        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new { Message = "Kullici bulunamadi " });
            }
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest(new { Message = "Role adi  bulunamadi " });
            }
            IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "kullaniciya rol atanamadi" });
            }
            return Ok(new { Message = "Rol atama islemi basarili" });
        }
    }
}
