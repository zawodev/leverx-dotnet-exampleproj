using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/reviews")]
    public class ProductReviewsController : ControllerBase {
        private readonly StoreContext _context;
        public ProductReviewsController(StoreContext context) => _context = context;

        /// <summary>get all reviews</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReview>>> GetAll()
            => Ok(await _context.ProductReviews.ToListAsync());

        /// <summary>get review by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReview>> GetById(int id) {
            var r = await _context.ProductReviews.FindAsync(id);
            if (r == null) return NotFound();
            return Ok(r);
        }

        /// <summary>create new review</summary>
        [HttpPost]
        public async Task<ActionResult<ProductReview>> Create(ProductReview r) {
            if (!await _context.Products.AnyAsync(p => p.Id == r.ProductId))
                return NotFound($"Product {r.ProductId} not found.");
            if (!await _context.Customers.AnyAsync(c => c.Id == r.CustomerId))
                return NotFound($"Customer {r.CustomerId} not found.");
            _context.ProductReviews.Add(r);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = r.Id }, r);
        }

        /// <summary>update review</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductReview r) {
            if (id != r.Id) return BadRequest();
            _context.Entry(r).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete review</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var r = await _context.ProductReviews.FindAsync(id);
            if (r == null) return NotFound();
            _context.ProductReviews.Remove(r);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
