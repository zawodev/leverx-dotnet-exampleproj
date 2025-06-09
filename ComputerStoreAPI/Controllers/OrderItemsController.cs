using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;

namespace ComputerStoreAPI.Controllers {
    [Authorize]
    [ApiController]
    [Route("api/orderitems")]
    public class OrderItemsController : ControllerBase {
        private readonly StoreContext _context;
        public OrderItemsController(StoreContext context) => _context = context;

        /// <summary>get all order items</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetAll()
            => Ok(await _context.OrderItems.ToListAsync());

        /// <summary>get specific order item</summary>
        [HttpGet("{orderId}/{productId}")]
        public async Task<ActionResult<OrderItem>> Get(int orderId, int productId) {
            var oi = await _context.OrderItems.FindAsync(orderId, productId);
            if (oi == null) return NotFound();
            return Ok(oi);
        }

        /// <summary>create new order item</summary>
        [HttpPost]
        public async Task<ActionResult<OrderItem>> Create(OrderItem oi) {
            // sprawdź, czy order istnieje
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == oi.OrderId);
            if (!orderExists)
                return NotFound($"Order with id={oi.OrderId} not found.");

            // sprawdź, czy product istnieje
            var productExists = await _context.Products.AnyAsync(p => p.Id == oi.ProductId);
            if (!productExists)
                return NotFound($"Product with id={oi.ProductId} not found.");

            _context.OrderItems.Add(oi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(Get),
                new { orderId = oi.OrderId, productId = oi.ProductId },
                oi
            );
        }

        /// <summary>update existing order item</summary>
        [HttpPut("{orderId}/{productId}")]
        public async Task<IActionResult> Update(int orderId, int productId, OrderItem oi) {
            if (orderId != oi.OrderId || productId != oi.ProductId)
                return BadRequest();

            _context.Entry(oi).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete order item</summary>
        [HttpDelete("{orderId}/{productId}")]
        public async Task<IActionResult> Delete(int orderId, int productId) {
            var oi = await _context.OrderItems.FindAsync(orderId, productId);
            if (oi == null) return NotFound();
            _context.OrderItems.Remove(oi);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
