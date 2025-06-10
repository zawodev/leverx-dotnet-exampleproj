using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace ComputerStoreAPI.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase {
        private readonly StoreContext _context;
        public OrdersController(StoreContext context) => _context = context;

        /// <summary>
        /// get all orders
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll() {
            return Ok(await _context.Orders.ToListAsync());
        }

        /// <summary>
        /// get order by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id) {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        /// <summary>
        /// create new order
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Order>> Create(Order order) {
            var custExists = await _context.Customers.AnyAsync(c => c.Id == order.CustomerId);
            if (!custExists)
                return NotFound($"Customer with id={order.CustomerId} not found.");

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        /// <summary>
        /// update existing order
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order order) {
            if (id != order.Id) return BadRequest();
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// delete order by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
