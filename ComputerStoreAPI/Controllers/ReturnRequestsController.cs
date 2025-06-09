using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Models;
using ComputerStore.Infrastructure.Data;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/returns")]
    public class ReturnRequestsController : ControllerBase {
        private readonly StoreContext _context;
        public ReturnRequestsController(StoreContext context) => _context = context;

        /// <summary>get all return requests</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnRequest>>> GetAll()
            => Ok(await _context.ReturnRequests.ToListAsync());

        /// <summary>get return request by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnRequest>> GetById(int id) {
            var rr = await _context.ReturnRequests.FindAsync(id);
            if (rr == null) return NotFound();
            return Ok(rr);
        }

        /// <summary>create new return request</summary>
        [HttpPost]
        public async Task<ActionResult<ReturnRequest>> Create(ReturnRequest rr) {
            if (!await _context.Orders.AnyAsync(o => o.Id == rr.OrderId))
                return NotFound($"Order {rr.OrderId} not found.");
            _context.ReturnRequests.Add(rr);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = rr.Id }, rr);
        }

        /// <summary>update return request</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ReturnRequest rr) {
            if (id != rr.Id) return BadRequest();
            _context.Entry(rr).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete return request</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var rr = await _context.ReturnRequests.FindAsync(id);
            if (rr == null) return NotFound();
            _context.ReturnRequests.Remove(rr);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
