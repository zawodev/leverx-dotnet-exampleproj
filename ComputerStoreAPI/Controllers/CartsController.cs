using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/carts")]
    public class CartsController : ControllerBase {
        private readonly StoreContext _context;
        public CartsController(StoreContext context) => _context = context;

        /// <summary>get all carts</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
            => Ok(await _context.Carts.ToListAsync());

        /// <summary>get cart by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetById(int id) {
            var c = await _context.Carts.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        /// <summary>create new cart</summary>
        [HttpPost]
        public async Task<ActionResult<Cart>> Create(Cart cart) {
            if (!await _context.Customers.AnyAsync(c => c.Id == cart.CustomerId))
                return NotFound($"Customer {cart.CustomerId} not found.");
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
        }

        /// <summary>update cart</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cart cart) {
            if (id != cart.Id) return BadRequest();
            _context.Entry(cart).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete cart</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var c = await _context.Carts.FindAsync(id);
            if (c == null) return NotFound();
            _context.Carts.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
