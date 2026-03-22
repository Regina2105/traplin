using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Traplin.Core.Entities;
using Traplin.Infrastructure.Data;

namespace Traplin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context; // нужен для сохранения компании

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration configuration,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public class RegisterModel
        {
            public string Email { get; set; } = string.Empty;
            public string DisplayName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = "applicant"; // applicant, employer, curator
            // Для работодателя
            public string? CompanyName { get; set; }
            public string? CompanyDescription { get; set; }
            public string? CompanyIndustry { get; set; }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Проверяем, что роль существует
            if (!await _roleManager.RoleExistsAsync(model.Role))
                model.Role = "applicant";

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                DisplayName = model.DisplayName,
                IsVerified = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Добавляем роль
            await _userManager.AddToRoleAsync(user, model.Role);

            // Если работодатель – создаём компанию
            if (model.Role == "employer" && !string.IsNullOrWhiteSpace(model.CompanyName))
            {
                var company = new Company
                {
                    UserId = user.Id,
                    Name = model.CompanyName,
                    Description = model.CompanyDescription,
                    Industry = model.CompanyIndustry,
                    VerificationStatus = "pending"
                };
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "User registered successfully" });
        }

        public class LoginModel
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = GenerateJwtToken(user, roles);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        private string GenerateJwtToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.DisplayName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}