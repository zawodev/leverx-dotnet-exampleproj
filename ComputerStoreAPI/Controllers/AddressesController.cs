using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;
using System.Net;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController : ControllerBase {
        private readonly StoreContext _context;
        public AddressesController(StoreContext context) => _context = context;

        /// <summary>get all addresses</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
            => Ok(await _context.Addresses.ToListAsync());

        /// <summary>get address by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetById(int id) {
            var a = await _context.Addresses.FindAsync(id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        /// <summary>create new address</summary>
        [HttpPost]
        public async Task<ActionResult<Address>> Create(Address address) {
            if (!await _context.Customers.AnyAsync(c => c.Id == address.CustomerId))
                return NotFound($"Customer {address.CustomerId} not found.");

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);
        }

        /// <summary>update address</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Address address) {
            if (id != address.Id) return BadRequest();
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete address</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var a = await _context.Addresses.FindAsync(id);
            if (a == null) return NotFound();
            _context.Addresses.Remove(a);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
