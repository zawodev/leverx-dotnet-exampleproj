using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
    private readonly StoreContext _context;
    public ProductsController(StoreContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _context.Products.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var prod = await _context.Products.FindAsync(id);
        if (prod == null) return NotFound();
        return Ok(prod);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product) {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product) {
        if (id != product.Id) return BadRequest();
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var prod = await _context.Products.FindAsync(id);
        if (prod == null) return NotFound();
        _context.Products.Remove(prod);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
