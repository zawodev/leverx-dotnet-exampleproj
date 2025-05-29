using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/returnitems")]
    public class ReturnItemsController : ControllerBase {
        private readonly StoreContext _context;
        public ReturnItemsController(StoreContext context) => _context = context;

        /// <summary>get all return items</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnItem>>> GetAll()
            => Ok(await _context.ReturnItems.ToListAsync());

        /// <summary>get single return item</summary>
        [HttpGet("{returnId}/{productId}")]
        public async Task<ActionResult<ReturnItem>> Get(int returnId, int productId) {
            var ri = await _context.ReturnItems.FindAsync(returnId, productId);
            if (ri == null) return NotFound();
            return Ok(ri);
        }

        /// <summary>create new return item</summary>
        [HttpPost]
        public async Task<ActionResult<ReturnItem>> Create(ReturnItem ri) {
            if (!await _context.ReturnRequests.AnyAsync(r => r.Id == ri.ReturnId))
                return NotFound($"ReturnRequest {ri.ReturnId} not found.");
            if (!await _context.Products.AnyAsync(p => p.Id == ri.ProductId))
                return NotFound($"Product {ri.ProductId} not found.");
            _context.ReturnItems.Add(ri);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { returnId = ri.ReturnId, productId = ri.ProductId }, ri);
        }

        /// <summary>update return item</summary>
        [HttpPut("{returnId}/{productId}")]
        public async Task<IActionResult> Update(int returnId, int productId, ReturnItem ri) {
            if (returnId != ri.ReturnId || productId != ri.ProductId) return BadRequest();
            _context.Entry(ri).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete return item</summary>
        [HttpDelete("{returnId}/{productId}")]
        public async Task<IActionResult> Delete(int returnId, int productId) {
            var ri = await _context.ReturnItems.FindAsync(returnId, productId);
            if (ri == null) return NotFound();
            _context.ReturnItems.Remove(ri);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
