using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Traplin.Core.Entities;
using Traplin.Infrastructure.Data;

namespace Traplin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public UsersController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/users/profile
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.DisplayName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.University,
                user.GraduationYear,
                user.Resume,
                user.PortfolioLinks,
                user.PrivacyLevel,
                user.IsVerified
            });
        }

        // PUT: api/users/profile
        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] AppUser updated)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.DisplayName = updated.DisplayName;
            user.FirstName = updated.FirstName;
            user.LastName = updated.LastName;
            user.University = updated.University;
            user.GraduationYear = updated.GraduationYear;
            user.Resume = updated.Resume;
            user.PortfolioLinks = updated.PortfolioLinks;
            user.PrivacyLevel = updated.PrivacyLevel;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}