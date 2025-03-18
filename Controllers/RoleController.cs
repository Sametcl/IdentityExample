using IdentitiyExample.Models;
using IdentitiyExample.Validators;
using IdentityExample.DTOs;
using IdentityExample.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateDto request,CancellationToken token)
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
            IdentityResult result=await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(new { Message=result.Errors});
            }
            return Ok(new {Message="Rol kayit islemi basarili"});   
        }
    }
}
