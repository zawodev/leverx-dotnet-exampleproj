using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ComputerStore.Infrastructure.Data;
using ComputerStore.Infrastructure.Services;
using ComputerStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase {
        private readonly StoreContext _context;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            StoreContext context,
            IPasswordHasher hasher,
            ITokenService tokenService,
            IConfiguration config,
            ILogger<AuthController> logger) 
        {
            _context = context;
            _hasher = hasher;
            _tokenService = tokenService;
            _config = config;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user) {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return Conflict("Username is already taken.");

            user.PasswordHash = _hasher.Hash(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // assign default user role
            var defaultRole = await _context.Roles
                .SingleOrDefaultAsync(r => r.Name == "User");
            if (defaultRole != null) {
                _context.UserRoles.Add(new UserRole {
                    UserId = user.Id,
                    RoleId = defaultRole.Id
                });
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User creds) {

            _logger.LogInformation("Login attempt for {Username}", creds.Username);

            var user = await _context.Users.Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Username == creds.Username);

            if (user == null || !_hasher.Verify(user.PasswordHash, creds.PasswordHash)) {
                _logger.LogWarning("Invalid login for {Username}", creds.Username);
                return Unauthorized("Invalid credentials.");
            }

            var jwt = _tokenService.CreateAccessToken(user);
            var refresh = _tokenService.CreateRefreshToken();

            user.RefreshToken = refresh;
            user.RefreshTokenExpiry = DateTime.UtcNow
                .AddDays(double.Parse(_config["Jwt:RefreshTokenExpiryDays"]!));
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserId} logged in successfully", user.Id);
            return Ok(new { token = jwt, refreshToken = refresh });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req) {
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(req.Token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = false
                }, out _);

            var userId = int.Parse(principal
                .FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null
                || user.RefreshToken != req.RefreshToken
                || user.RefreshTokenExpiry <= DateTime.UtcNow) {
                return Unauthorized("Invalid refresh request.");
            }

            var newJwt = _tokenService.CreateAccessToken(user);
            var newRefresh = _tokenService.CreateRefreshToken();
            user.RefreshToken = newRefresh;
            user.RefreshTokenExpiry = DateTime.UtcNow
                .AddDays(double.Parse(_config["Jwt:RefreshTokenExpiryDays"]!));
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { token = newJwt, refreshToken = newRefresh });
        }
    }
}
