using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerStoreAPI.Data;
using ComputerStoreAPI.Models;

namespace ComputerStoreAPI.Controllers {
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase {
        private readonly StoreContext _context;
        public CustomersController(StoreContext context) => _context = context;

        /// <summary>get all customers</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
            => Ok(await _context.Customers.ToListAsync());

        /// <summary>get customer by id</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id) {
            var c = await _context.Customers.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(c);
        }

        /// <summary>create new customer</summary>
        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer) {

            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
                return Conflict($"Email '{customer.Email}' is already in use.");

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>update customer</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer) {
            if (id != customer.Id) return BadRequest();
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>delete customer</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var c = await _context.Customers.FindAsync(id);
            if (c == null) return NotFound();
            _context.Customers.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
