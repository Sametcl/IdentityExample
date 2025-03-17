using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using IdentitiyExample.DTOs;
using IdentitiyExample.Models;
using IdentitiyExample.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentitiyExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        public readonly IMapper _mapper;
        public AuthController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken token)
        {
            //AppUser user = new AppUser
            //{
            //    UserName = request.UserName,
            //    Email = request.Email,
            //    FirstName = request.FirstName,
            //    LastName = request.LastName,
            //};
            var validator = new RegisterDtoValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var user = _mapper.Map<AppUser>(request);//Mapping islemi
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return Ok(new { Message = "kullanici kaydedildi" });
            }
            return BadRequest(new { Message = "Kullanici kayit islemi basarisiz" });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto request, CancellationToken token)
        {
            var validator = new LoginDtoValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(p =>
            p.UserName == request.EmailOrUserName ||
            p.Email == request.EmailOrUserName);

            if (user == null)
            {
                return BadRequest(new { Message = "Kullanici bulunamadi" });
            }
            var result =await _userManager.CheckPasswordAsync(user, request.Password);
            if (result)
            {
                return Ok(new { Message = "giris basarili" });
            }
            return BadRequest(new { Message = "Paralo yanlis" });

        }
    }
}
