using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cmpg323_project.Models;

namespace cmpg323_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly cmpg323sqldbserverContext _context;

        public CustomersController(cmpg323sqldbserverContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(short id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost ("{customerID}, {customerTitle}, {customerName}, {customerSurname}, {cellphone}")]
        public async Task<ActionResult> PostCustomer(short customerID, string customerTitle, string customerName, string customerSurname, string cellphone)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'cmpg323sqldbserverContext.Customers'  is null.");
            }

            Customer customer = new Customer();

            customer.CustomerId = customerID;
            customer.CustomerTitle = customerTitle;
            customer.CustomerName = customerName;
            customer.CustomerSurname = customerSurname;
            customer.CellPhone = cellphone;

            _context.Customers.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{customerID}, {customerTitle}, {customerName}, {customerSurname}, {cellphone}")]
        public async Task<IActionResult> PutCustomer(short customerID, string customerTitle, string customerName, string customerSurname, string cellphone)
        {
            Customer customer = new Customer();
            customer.CustomerId = customerID;

            if (customerID != customer.CustomerId)
            {
                return BadRequest();
            }

            customer.CustomerTitle = customerTitle;
            customer.CustomerName = customerName;
            customer.CustomerSurname = customerSurname;
            customer.CellPhone = cellphone;

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(short id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);

            if (!CustomerExists(id))
            {
                return NotFound();
            }

            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(short id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}