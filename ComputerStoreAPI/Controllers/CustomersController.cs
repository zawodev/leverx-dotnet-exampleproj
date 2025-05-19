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

        /// <summary>
        /// get all customers
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll() {
            return Ok(await _context.Customers.ToListAsync());
        }

        /// <summary>
        /// get customer by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id) {
            var cust = await _context.Customers.FindAsync(id);
            if (cust == null) return NotFound();
            return Ok(cust);
        }

        /// <summary>
        /// create new customer
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Customer>> Create(Customer customer) {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// update existing customer
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer customer) {
            if (id != customer.Id) return BadRequest();
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// delete customer by id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var cust = await _context.Customers.FindAsync(id);
            if (cust == null) return NotFound();
            _context.Customers.Remove(cust);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
