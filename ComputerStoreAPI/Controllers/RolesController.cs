using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase {
        private readonly StoreContext _context;
        public RolesController(StoreContext context) => _context = context;

        /// <summary>get all roles</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
            => Ok(await _context.Roles.ToListAsync());

        /// <summary>get role by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetById(int id) {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        /// <summary>create new role</summary>
        [HttpPost]
        public async Task<ActionResult<Role>> Create(Role role) {
            // role name must be unique
            if (await _context.Roles.AnyAsync(x => x.Name == role.Name))
                return Conflict($"role '{role.Name}' already exists.");
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }

        /// <summary>update existing role</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Role role) {
            if (id != role.Id) return BadRequest();
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete role</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var r = await _context.Roles.FindAsync(id);
            if (r == null) return NotFound();
            _context.Roles.Remove(r);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
