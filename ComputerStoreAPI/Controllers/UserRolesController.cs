using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/userroles")]
    public class UserRolesController : ControllerBase {
        private readonly StoreContext _context;
        public UserRolesController(StoreContext context) => _context = context;

        /// <summary>get all user-role assignments</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAll()
            => Ok(await _context.UserRoles.ToListAsync());

        /// <summary>get specific assignment</summary>
        [HttpGet("{userId}/{roleId}")]
        public async Task<ActionResult<UserRole>> Get(int userId, int roleId) {
            var ur = await _context.UserRoles.FindAsync(userId, roleId);
            if (ur == null) return NotFound();
            return Ok(ur);
        }

        /// <summary>assign role to user</summary>
        [HttpPost]
        public async Task<ActionResult<UserRole>> Create(UserRole ur) {
            if (!await _context.Users.AnyAsync(u => u.Id == ur.UserId))
                return NotFound($"user {ur.UserId} not found.");
            if (!await _context.Roles.AnyAsync(r => r.Id == ur.RoleId))
                return NotFound($"role {ur.RoleId} not found.");
            _context.UserRoles.Add(ur);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { userId = ur.UserId, roleId = ur.RoleId }, ur);
        }

        /// <summary>remove role from user</summary>
        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> Delete(int userId, int roleId) {
            var ur = await _context.UserRoles.FindAsync(userId, roleId);
            if (ur == null) return NotFound();
            _context.UserRoles.Remove(ur);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
