using IdentitiyExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var email = User.Identity!.Name;
            var user = await _userManager.FindByNameAsync(email!);
            if (user == null) return NotFound();

            return Ok(new { user.Email, user.Id });
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var user = await _userManager.Users.Select(u => new { u.Id, u.Email ,u.FirstName,u.LastName}).ToListAsync();
            return Ok(user);    
        }
    }
}
