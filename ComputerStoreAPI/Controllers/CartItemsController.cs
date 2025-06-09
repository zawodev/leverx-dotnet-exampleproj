using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/cartitems")]
    public class CartItemsController : ControllerBase {
        private readonly StoreContext _context;
        public CartItemsController(StoreContext context) => _context = context;

        /// <summary>get all cart items</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetAll()
            => Ok(await _context.CartItems.ToListAsync());

        /// <summary>get single cart item</summary>
        [HttpGet("{cartId}/{productId}")]
        public async Task<ActionResult<CartItem>> Get(int cartId, int productId) {
            var ci = await _context.CartItems.FindAsync(cartId, productId);
            if (ci == null) return NotFound();
            return Ok(ci);
        }

        /// <summary>create new cart item</summary>
        [HttpPost]
        public async Task<ActionResult<CartItem>> Create(CartItem ci) {
            if (!await _context.Carts.AnyAsync(c => c.Id == ci.CartId))
                return NotFound($"Cart {ci.CartId} not found.");
            if (!await _context.Products.AnyAsync(p => p.Id == ci.ProductId))
                return NotFound($"Product {ci.ProductId} not found.");
            _context.CartItems.Add(ci);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { cartId = ci.CartId, productId = ci.ProductId }, ci);
        }

        /// <summary>update cart item</summary>
        [HttpPut("{cartId}/{productId}")]
        public async Task<IActionResult> Update(int cartId, int productId, CartItem ci) {
            if (cartId != ci.CartId || productId != ci.ProductId) return BadRequest();
            _context.Entry(ci).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete cart item</summary>
        [HttpDelete("{cartId}/{productId}")]
        public async Task<IActionResult> Delete(int cartId, int productId) {
            var ci = await _context.CartItems.FindAsync(cartId, productId);
            if (ci == null) return NotFound();
            _context.CartItems.Remove(ci);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
