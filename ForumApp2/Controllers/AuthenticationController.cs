using ForumApp2.DTOs;
using ForumApp2.Models;
using ForumApp2.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace ForumApp2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(IConfiguration configuration, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Adminname))
            {
                return BadRequest(new { code = "InvalidUserName", description = "Username cannot be empty." });
            }

            if (!request.Adminname.All(char.IsLetterOrDigit))
            {
                return BadRequest(new { code = "InvalidUserName", description = "Username can only contain letters or digits." });
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var admin = new ApplicationUser
            {
                UserName = request.Adminname,
                Email = request.AdminEmail,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
        };

            var result = await _userManager.CreateAsync(admin, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!roleResult.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating role");
            }

            await _userManager.AddToRoleAsync(admin, "Admin");

            return Ok(admin);
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto request)
        {
            // Validate the input
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return BadRequest(new { code = "InvalidUserName", description = "Username cannot be empty." });
            }

            if (!request.Username.All(char.IsLetterOrDigit))
            {
                return BadRequest(new { code = "InvalidUserName", description = "Username can only contain letters or digits." });
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.UserEmail,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
        };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                if (!roleResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error creating role");
                }
            }

            await _userManager.AddToRoleAsync(user, "User");
            //var rawUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            //rawUser.LockoutEnabled = false;
            //_dbContext.SaveChanges();

            return Ok(user);
        }


        [HttpPost("loginAdmin")]
        public async Task<IActionResult> LoginAdmin(LoginAdminDto request)
        {
            var admin = await _userManager.FindByNameAsync(request.Adminname);

            if (admin == null)
                return BadRequest("Admin not found");

            if (!VerifyPasswordHash(request.Password, admin.PasswordHash, admin.PasswordSalt))
                return BadRequest("Wrong password");

            string token = CreateToken(admin, "Admin");

            return Ok(new { Token = token });
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser(LoginUserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return BadRequest("User not found");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Wrong password");

            string token = CreateToken(user, "User");

            return Ok(new { Token = token });
        }

        private string CreateToken(ApplicationUser user, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };

            return GenerateToken(claims);
        }

        private string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60), // Adjust token expiration as needed
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(passwordHash);
            }
        }
    }
}
