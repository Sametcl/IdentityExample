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
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentitiyExample.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public readonly IMapper _mapper;
        public AuthController(UserManager<AppUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
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
            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Kullanici kayit islemi basarisiz" });
            }
            return Ok(new { Message = "kullanici kaydedildi" });
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
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return BadRequest(new { Message = "Girilen bilgiler uyusmuyor" });
            }
            //kullanici rollerini getirelim 
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
                   {
                       new Claim(ClaimTypes.Name,user.UserName!),
                       new Claim(ClaimTypes.Email,user.Email!)
                   };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperLongSecureSecretKeyForJwtSigning12345"));   
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwttoken = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(15),
                        signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwttoken) });

        }
    }
}
