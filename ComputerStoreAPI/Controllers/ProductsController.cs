using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context) => _context = context;

        /// <summary>
        /// get all products
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll() {
            return Ok(await _context.Products.ToListAsync());
        }

        /// <summary>
        /// get product by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id) {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null) return NotFound();
            return Ok(prod);
        }

        /// <summary>
        /// create new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// update existing product
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product) {
            if (id != product.Id) return BadRequest();
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// delete product by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var prod = await _context.Products.FindAsync(id);
            if (prod == null) return NotFound();
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
