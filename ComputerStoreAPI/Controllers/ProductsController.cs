using ComputerStore.Application.Repositories;
using ComputerStoreAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStoreAPI.Controllers {
    [Authorize(Policy = "CanManageProducts")]
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase {
        // old EF context
        // private readonly StoreContext _context;
        // public ProductsController(StoreContext context) => _context = context;

        // new Dapper repo
        private readonly IProductRepository _repo;
        // private readonly IMediator _mediator;
        public ProductsController(IProductRepository repo) => _repo = repo;

        /// <summary>
        /// get all products
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll() {
            // old EF code:
            // return Ok(await _context.Products.ToListAsync());

            var products = await _repo.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// get product by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id) {
            // old EF code:
            // var prod = await _context.Products.FindAsync(id);
            // if (prod == null) return NotFound();
            // return Ok(prod);

            // mediator method
            // var prod = await _mediator.Send(new GetProductByIdQuery(id));
            // return prod is null ? NotFound() : Ok(prod);

            var prod = await _repo.GetByIdAsync(id);
            if (prod is null) return NotFound();
            return Ok(prod);
        }

        /// <summary>
        /// create new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product) {
            // old EF code:
            // var catExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            // if (!catExists)
            //     return NotFound($"Category with id={product.CategoryId} not found.");
            // _context.Products.Add(product);
            // await _context.SaveChangesAsync();
            // return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);

            // optionally validate category with repo, but for now i will skip that

            // mediator method
            // var newId = await _mediator.Send(new CreateProductCommand(product));
            // return CreatedAtAction(nameof(GetById), new { id = newId }, product);

            var newId = await _repo.CreateAsync(product);
            product.Id = newId;
            return CreatedAtAction(nameof(GetById), new { id = newId }, product);
        }

        /// <summary>
        /// update existing product
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product) {
            if (id != product.Id) return BadRequest();

            // old EF code:
            // _context.Entry(product).State = EntityState.Modified;
            // await _context.SaveChangesAsync();
            // return NoContent();

            await _repo.UpdateAsync(product);
            return NoContent();
        }

        /// <summary>
        /// delete product by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            // old EF code:
            // var prod = await _context.Products.FindAsync(id);
            // if (prod == null) return NotFound();
            // _context.Products.Remove(prod);
            // await _context.SaveChangesAsync();
            // return NoContent();

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}