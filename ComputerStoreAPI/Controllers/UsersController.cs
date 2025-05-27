using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase {
        private readonly StoreContext _context;
        public UsersController(StoreContext context) => _context = context;

        /// <summary>get all users</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
            => Ok(await _context.Users.ToListAsync());

        /// <summary>get user by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id) {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound();
            return Ok(u);
        }

        /// <summary>create new user</summary>
        [HttpPost]
        public async Task<ActionResult<User>> Create(User user) {
            // username must be unique
            if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                return Conflict($"username '{user.Username}' already exists.");
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>update existing user</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user) {
            if (id != user.Id) return BadRequest();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete user</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var u = await _context.Users.FindAsync(id);
            if (u == null) return NotFound();
            _context.Users.Remove(u);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
